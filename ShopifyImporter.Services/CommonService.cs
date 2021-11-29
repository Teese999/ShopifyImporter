using Serilog;
using ShopifyImporter.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{
    public class CommonService : ICommonService
    {
        private Settings _settings;
        private IFileService _fileService;
        private IExcelParserService _excelParserService;
        private IEmailService _emailService;
        private IReportService _reportService;
        private IShopifyService _shopifyService;
        private ILogger _logger;

        public CommonService(IFileService fileService, IExcelParserService excelParserService, IEmailService emailService,
            IReportService reportService, IShopifyService shopifyService, Settings settings, ILogger logger)
        {
            _settings = settings;
            _fileService = fileService;
            _excelParserService = excelParserService;
            _emailService = emailService;
            _reportService = reportService;
            _shopifyService = shopifyService;
            _logger = logger;
        }

        public async Task Execute()
        {
            var report = "";
            var fileNames = Enumerable.Empty<string>();

            try
            {
                fileNames = await _fileService.DownloadFiles();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                report = e.Message;
                _emailService.Send(_settings.Smtp.EmailFrom, _settings.Smtp.EmailTo, $"Shopify sync report {DateTime.UtcNow}", report);
            }


            foreach (var fileName in fileNames)
            {
                try
                {
                    var inventories = _excelParserService.ParseFile(fileName);
                    inventories = await _shopifyService.UpdateInventoriesAsync(inventories);
                    await _fileService.UploadFile(fileName);
                    await _fileService.DeleteFile(fileName);
                    report = _reportService.Build(inventories, fileName);
                }
                catch (Exception e)
                {
                    _logger.Error(e, e.Message);
                    report = e.Message;
                }

                _logger.Information(report);
                _emailService.Send(_settings.Smtp.EmailFrom, _settings.Smtp.EmailTo, $"Shopify sync report {DateTime.UtcNow}", report);
            }
        }

        public async Task<(IEnumerable<string>, IEnumerable<string>, IEnumerable<string>)> CheckFileStorageConfiguration()
        {

            var folders = new List<string>();
            var createdFolders = new List<string>();
            var errorMessages = new List<string>();

            try
            {
                //check if folder exists
                await _fileService.CheckFolderExists(_settings.Azure.MicrosoftOneDrive.IncomingFolderName);
            }
            catch (Exception e)
            {
                errorMessages.Add(e.Message);

                if (_settings.Azure.MicrosoftGraph.AutomaticFolderCreationEnabled)
                {
                    try
                    {
                        //create folder if not exists
                        await _fileService.CreateFolder(_settings.Azure.MicrosoftOneDrive.IncomingFolderName);
                        createdFolders.Add(_settings.Azure.MicrosoftOneDrive.IncomingFolderName);
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(ex.Message);
                    }
                }
            }

            try
            {
                //check if folder exists
                await _fileService.CheckFolderExists(_settings.Azure.MicrosoftOneDrive.ProcessedFolderName);
            }
            catch (Exception e)
            {
                errorMessages.Add(e.Message);

                if (_settings.Azure.MicrosoftGraph.AutomaticFolderCreationEnabled)
                {
                    try
                    {
                        //create folder if not exists
                        await _fileService.CreateFolder(_settings.Azure.MicrosoftOneDrive.ProcessedFolderName);
                        createdFolders.Add(_settings.Azure.MicrosoftOneDrive.ProcessedFolderName);
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add(ex.Message);
                    }
                }
            }

            try
            {
                folders = (await _fileService.ListDriveFolders())?.ToList();
            }
            catch (Exception e)
            {
                errorMessages.Add(e.Message);
            }

            return (folders, createdFolders, errorMessages);
        }
    }
}
