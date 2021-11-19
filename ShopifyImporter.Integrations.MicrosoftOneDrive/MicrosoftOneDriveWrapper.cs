using Microsoft.Graph;
using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.MicrosoftGraph;
using ShopifyImporter.Integrations.MicrosoftOneDrive.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Unity;

namespace ShopifyImporter.Integrations.MicrosoftOneDrive
{
    public class MicrosoftOneDriveWrapper
    {
        private IUnityContainer _container;
        private Settings _settings;

        public MicrosoftOneDriveWrapper(IUnityContainer container, Settings settings)
        {
            _container = container;
            _settings = settings;
        }

        public async Task<IEnumerable<string>> DownloadFiles()
        {
            try
            {
                var wrapper = _container.Resolve<MicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();

                var fileNames = new List<string>();

                var item = await graphClient.Drive.Root.ItemWithPath($"/{_settings.Azure.MicrosoftOneDrive.IncomingFolderName}").Request().Expand(i => i.Children).GetAsync();

                foreach (var child in item.Children)
                {
                    if (child.File != null)
                    {
                        fileNames.Add(child.Name);
                        var fileContent = await graphClient.Drives[child.ParentReference.DriveId].Items[child.Id].Content.Request()
                                    .GetAsync();
                        using (var fileStream = new FileStream(Path.Combine(_settings.IncomingDownloadFolderName, child.Name), FileMode.Create, FileAccess.Write))
                        {
                            fileContent.CopyTo(fileStream);

                        }
                    }
                }

                return fileNames;
            }
            catch (Exception e)
            {
                if (e is ServiceException)
                {
                    throw new Exception($"Microsoft OneDrive error: {_settings.Azure.MicrosoftOneDrive.IncomingFolderName} - {((ServiceException)e).Error.Message}");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task UploadFile(string fileName)
        {
            try
            {
                var wrapper = _container.Resolve<MicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();
                using (FileStream fileStream = new FileStream(Path.Combine(_settings.IncomingDownloadFolderName, fileName), FileMode.Open, FileAccess.Read))
                {
                    await graphClient.Drive.Root.ItemWithPath($"/{_settings.Azure.MicrosoftOneDrive.ProcessedFolderName}/{fileName}").Content.Request().PutAsync<DriveItem>(fileStream);
                }
            }
            catch (Exception e)
            {
                if (e is ServiceException)
                {
                    throw new Exception($"Microsoft OneDrive error: {_settings.Azure.MicrosoftOneDrive.ProcessedFolderName}/{fileName} - {((ServiceException)e).Error.Message}");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeleteFile(string fileName)
        {
            try
            {
                var wrapper = _container.Resolve<MicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();
                await graphClient.Drive.Root.ItemWithPath($"/{_settings.Azure.MicrosoftOneDrive.IncomingFolderName}/{fileName}").Request().DeleteAsync();
            }
            catch (Exception e)
            {
                if (e is ServiceException)
                {
                    throw new Exception($"Microsoft OneDrive error: {_settings.Azure.MicrosoftOneDrive.IncomingFolderName}/{fileName} - {((ServiceException)e).Error.Message}");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}