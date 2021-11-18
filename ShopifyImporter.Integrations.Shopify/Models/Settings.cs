using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.Shopify.Models
{
    public class Settings
    {
        public ShopifySettings Shopify { get; set; }
        public AzureSettings Azure { get; set; }

        public class ShopifySettings
        {
            public string ShopUrl { get; set; }
            public string ShopAccessToken { get; set; }
            public string ShopApiKey { get; set; }
        }

        public class AzureSettings
        {
            public MicrosoftgraphSettings MicrosoftGraph { get; set; }
            public MicrosoftOneDriveSettings MicrosoftOneDrive { get; set; }
        }

        public class MicrosoftgraphSettings
        {
            public string[] Scopes { get; set; }
            public string AppClientId { get; set; }
            public string MsalCacheFileName { get; set; }
            public string AppRedirectUrl { get; set; }
        }

        public class MicrosoftOneDriveSettings
        {
            public string IncomingFolderName { get; set; }
            public string ProcessedFolderName { get; set; }
        }
    }
}
