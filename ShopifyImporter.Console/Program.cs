using Microsoft.Extensions.Configuration;
using ShopifyImporter.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Configure();
            new Startup().Run();
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            Settings.ShopUrl = (builder as IConfigurationRoot)["shopUrl"];
            Settings.ShopAccessToken = (builder as IConfigurationRoot)["shopAccessToken"];
            Settings.ShopApiKey = (builder as IConfigurationRoot)["shopApiKey"];

        }
    }
}
