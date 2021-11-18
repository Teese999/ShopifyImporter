using Microsoft.Graph;
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

namespace ShopifyImporter.Integrations.MicrosoftOneDrive
{
    public class MicrosoftOneDriveWrapper
    {
        public async Task<IEnumerable<FileModel>> GetFiles(string path)
        {
            var graphClient = await new MicrosoftGraphWrapper().GetAuthenticatedClient();

            var items = await graphClient.Drive.Root.ItemWithPath($"/{path}").Children.Request().GetAsync();

            var files = new List<FileModel>();

            var pageIterator = PageIterator<DriveItem>
                .CreatePageIterator(
                    graphClient,
                    items,
                    (item) =>
                    {
                        files.Add(new FileModel { Name = item.Name, Id = item.Id });
                        return true;
                    }
                );

            await pageIterator.IterateAsync();

            return files;
        }
    }
}