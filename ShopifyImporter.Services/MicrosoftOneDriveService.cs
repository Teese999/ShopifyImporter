using ShopifyImporter.Integrations.MicrosoftOneDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{
    public class MicrosoftOneDriveService
    {
        public async Task<IEnumerable<(string, string)>> GetFiles()
        {
            var files = await new MicrosoftOneDriveWrapper().GetFiles("ShopifyIncoming");
            return files.Select(f => (f.Id, f.Name));
        }
    }
}
