using ShopifyImporter.Contracts;
using ShopifyImporter.Contracts.Models;
using ShopifyImporter.Integrations.MicrosoftOneDrive;
using ShopifyImporter.Integrations.MicrosoftOneDrive.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace ShopifyImporter.Services
{
    public class FileService : IFileService
    {
        private IUnityContainer _container;
        private Settings _settings;

        public FileService(Settings settings, IUnityContainer container)
        {
            _container = container;
            _settings = settings;
        }

        public async Task<IEnumerable<string>> ListDriveFolders()
        {
            var wrapper = _container.Resolve<IMicrosoftOneDriveWrapper>();
            var folders = await wrapper.ListDriveFolders();
            return folders;
        }

        public async Task CheckFolderExists(string folderName)
        {
            var wrapper = _container.Resolve<IMicrosoftOneDriveWrapper>();
            await wrapper.CheckFolderExists(folderName);
        }

        public async Task CreateFolder(string folderName)
        {
            var wrapper = _container.Resolve<IMicrosoftOneDriveWrapper>();
            await wrapper.CreateFolder(folderName);
        }

        public async Task<IEnumerable<string>> DownloadFiles()
        {
            var wrapper = _container.Resolve<IMicrosoftOneDriveWrapper>();
            var fileNames = await wrapper.DownloadFiles();
            return fileNames;
        }

        public async Task UploadFile(string fileName)
        {
            var wrapper = _container.Resolve<IMicrosoftOneDriveWrapper>();
            await wrapper.UploadFile(fileName);
        }

        public async Task DeleteFile(string fileName)
        {
            var wrapper = _container.Resolve<IMicrosoftOneDriveWrapper>();
            File.Delete(Path.Combine(_settings.IncomingDownloadFolderName, fileName));
            await wrapper.DeleteFile(fileName);
        }

        public async Task<IEnumerable<DriveDto>> GetDrives()
        {
            var wrapper = _container.Resolve<IMicrosoftOneDriveWrapper>();
            var data = await wrapper.GetDrives();
            var result = data?.Select(d => new DriveDto
            {
                Id = d.Id,
                OwnerId = d.OwnerId,
                OwnerName = d.OwnerName,
                Type = d.Type,
                Description = d.Description,
                Name = d.Name,
                OriginalData = d.OriginalData
            });
            return result;
        }
    }
}
