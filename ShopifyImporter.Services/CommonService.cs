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

        public CommonService(IFileService fileService, IExcelParserService excelParserService, IEmailService emailService, 
            IReportService reportService, IShopifyService shopifyService, Settings settings)
        {
            _settings = settings;
            _fileService = fileService;
            _excelParserService = excelParserService;
            _emailService = emailService;
            _reportService = reportService;
            _shopifyService = shopifyService;
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
                    report = e.Message;
                }

                _emailService.Send(_settings.Smtp.EmailFrom, _settings.Smtp.EmailTo, $"Shopify sync report {DateTime.UtcNow}", report);
            }
        }

        public async Task Authenticate()
        {
            await _fileService.DownloadFiles();
        }
    }
}
