using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.GoogleDrive
{
    public class GoogleApplication
    {
        public readonly string ApplicationName = "ShopifyImporter";
        public UserCredential Credential { get; set; }
        public IList<Google.Apis.Drive.v3.Data.File> Files { get; set; }
    }
}
