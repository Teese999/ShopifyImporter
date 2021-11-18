using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace ShopifyImporter.Autorize
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = LoadAppSettings();
            var token = AuthHelper.GetTokenForUserAsync(config["applicationId"]).Result;
            var graphClient = AuthHelper.GetAuthenticatedClient();
        }
        private static IConfigurationRoot LoadAppSettings()
        {

            var config = new ConfigurationBuilder()
                 .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", false, true)
                 .Build();
            return config;
        }
    }
}
