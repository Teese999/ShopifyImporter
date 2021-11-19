using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.MicrosoftOneDrive;
using ShopifyImporter.Integrations.Shopify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<(string, string)>> GetFiles()
        {
            var wrapper = _container.Resolve<MicrosoftOneDriveWrapper>();
            var files = await wrapper.GetFiles(_settings.Azure.MicrosoftOneDrive.IncomingFolderName);
            return files.Select(f => (f.Id, f.Name));
        }
    }
}
