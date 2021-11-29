using Microsoft.Graph;
using Newtonsoft.Json;
using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.MicrosoftGraph;
using ShopifyImporter.Integrations.MicrosoftGraph.Contracts;
using ShopifyImporter.Integrations.MicrosoftOneDrive.Contracts;
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
    public class MicrosoftOneDriveWrapper : IMicrosoftOneDriveWrapper
    {
        private IUnityContainer _container;
        private Settings _settings;

        public MicrosoftOneDriveWrapper(IUnityContainer container, Settings settings)
        {
            _container = container;
            _settings = settings;
        }

        public async Task<IEnumerable<string>> ListDriveFolders()
        {
            var driveId = await GetUserDriveId();

            try
            {    
                var wrapper = _container.Resolve<MicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();

                var folderNames = new List<string>();

                var item = await graphClient.Me.Drives[driveId].Root.Request().Expand(i => i.Children).GetAsync();

                foreach (var child in item.Children)
                {
                    if (child.Folder != null)
                    {
                        folderNames.Add(child.Name);
                    }
                }

                return folderNames;
            }
            catch (ServiceException e)
            {
                throw new Exception($"Microsoft OneDrive error: Drive with id \"{driveId}\" - {((ServiceException)e).Error.Message}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CheckFolderExists(string folderName)
        {
            try
            {
                var wrapper = _container.Resolve<MicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();

                var item = await GetFolder(folderName, graphClient);
            }
            catch (ServiceException e)
            {
                throw new Exception($"Microsoft OneDrive error: {folderName} - {((ServiceException)e).Error.Message}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateFolder(string folderName)
        {
            var driveId = await GetUserDriveId();

            try
            {
                var wrapper = _container.Resolve<MicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();

                var driveItem = new DriveItem
                {
                    Name = folderName,
                    Folder = new Folder
                    {
                    },
                    AdditionalData = new Dictionary<string, object>()
                    {
                        {"@microsoft.graph.conflictBehavior","fail"}
                    }
                };

                await graphClient.Me.Drives[driveId].Root.Children.Request().AddAsync(driveItem);
            }
            catch (ServiceException e)
            {
                throw new Exception($"Microsoft OneDrive error: {folderName} - {((ServiceException)e).Error.Message}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> DownloadFiles()
        {
            try
            {
                var wrapper = _container.Resolve<IMicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();

                var fileNames = new List<string>();

                var item = await GetFolder(_settings.Azure.MicrosoftOneDrive.IncomingFolderName, graphClient);

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
            catch (ServiceException e)
            {
                throw new Exception($"Microsoft OneDrive error: {_settings.Azure.MicrosoftOneDrive.IncomingFolderName} - {((ServiceException)e).Error.Message}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UploadFile(string fileName)
        {
            var driveId = await GetUserDriveId();

            try
            {
                var wrapper = _container.Resolve<IMicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();
                using (FileStream fileStream = new FileStream(Path.Combine(_settings.IncomingDownloadFolderName, fileName), FileMode.Open, FileAccess.Read))
                {
                    await graphClient.Me.Drives[driveId].Root.ItemWithPath($"/{_settings.Azure.MicrosoftOneDrive.ProcessedFolderName}/{fileName}").Content.Request().PutAsync<DriveItem>(fileStream);
                }
            }
            catch (ServiceException e)
            {
                throw new Exception($"Microsoft OneDrive error: {_settings.Azure.MicrosoftOneDrive.ProcessedFolderName}/{fileName} - {((ServiceException)e).Error.Message}");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFile(string fileName)
        {
            var driveId = await GetUserDriveId();

            try
            {
                var wrapper = _container.Resolve<IMicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();
                await graphClient.Me.Drives[driveId].Root.ItemWithPath($"/{_settings.Azure.MicrosoftOneDrive.IncomingFolderName}/{fileName}").Request().DeleteAsync();
            }
            catch (ServiceException e)
            {
                throw new Exception($"Microsoft OneDrive error: {_settings.Azure.MicrosoftOneDrive.IncomingFolderName}/{fileName} - {((ServiceException)e).Error.Message}");
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DriveModel>> GetDrives()
        {
            var wrapper = _container.Resolve<IMicrosoftGraphWrapper>();
            var graphClient = await wrapper.GetAuthenticatedClient();
            var drives = await graphClient.Me.Drives.Request().GetAsync();
            var drivesResult = drives?.Select(d => new DriveModel
            {
                Id = d.Id,
                Type = d.DriveType,
                Name = d.Name,
                Description = d.Description,
                OwnerId = d.Owner?.User?.Id,
                OwnerName = d.Owner?.User?.DisplayName,
                OriginalData = JsonConvert.SerializeObject(d)
            });

            return drivesResult;
        }

        private async Task<DriveItem> GetFolder(string folderName, GraphServiceClient graphClient)
        {
            var driveId = await GetUserDriveId();
            var item = await graphClient.Me.Drives[driveId].Root.ItemWithPath($"/{folderName}").Request().Expand(i => i.Children).GetAsync();
            return item;
        }

        private async Task<string> GetUserDriveId()
        {
            var driveId = _settings.Azure.MicrosoftOneDrive.DriveId;

            if (string.IsNullOrEmpty(driveId))
            {
                var wrapper = _container.Resolve<IMicrosoftGraphWrapper>();
                var graphClient = await wrapper.GetAuthenticatedClient();
                var drives = await graphClient.Me.Drives.Request().GetAsync();
                driveId = drives?.FirstOrDefault()?.Id;
            }
            if (string.IsNullOrEmpty(driveId))
            {
                throw new Exception($"Microsoft OneDrive error: DriveId parameter is not specified or drives do not exist for this user.");
            }

            return driveId;
        }
    }
}