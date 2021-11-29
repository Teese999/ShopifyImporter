using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public class Settings
    {
        public ShopifySettings Shopify { get; set; }
        public AzureSettings Azure { get; set; }
        public SmtpSettings Smtp { get; set; }
        public string IncomingDownloadFolderName { get; set; }

        public class ShopifySettings
        {
            public string ShopUrl { get; set; }
            public string Password { get; set; }
            public string ShopApiKey { get; set; }
        }

        public class AzureSettings
        {
            public MicrosoftGraphSettings MicrosoftGraph { get; set; }
            public MicrosoftOneDriveSettings MicrosoftOneDrive { get; set; }
        }

        public class MicrosoftGraphSettings
        {
            public string[] Scopes { get; set; }
            public string AppClientId { get; set; }
            public string MsalCacheFileName { get; set; }
            public bool MsalEncryptCacheFile { get; set; }
            public string AppRedirectUrl { get; set; }
            public bool AutomaticFolderCreationEnabled { get; set; }
        }

        public class MicrosoftOneDriveSettings
        {
            public string IncomingFolderName { get; set; }
            public string ProcessedFolderName { get; set; }
            public string DriveId { get; set; }
        }
        public class SmtpSettings
        {
            public string SmtpHost { get; set; }
            public string SmtpUserName { get; set; }
            public string SmtpPassword { get; set; }
            public int SmtpPort { get; set; }

            public string EmailFrom { get; set; }
            public string EmailTo { get; set; }
        }
    }
}
