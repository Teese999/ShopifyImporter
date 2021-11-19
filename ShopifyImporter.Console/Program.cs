using Microsoft.Extensions.Configuration;
using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.Shopify;
using ShopifyImporter.Integrations.Shopify.Models;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporter.Console
{
    public class Program
    {

        private static IUnityContainer _container = new UnityContainer();
        private static ReportDto _report = new();

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var settings = builder.GetSection("Settings");
            _container.RegisterInstance(settings.Get<Settings>());

            ContainerConfiguration.RegisterTypes<HierarchicalLifetimeManager>(_container);

            var shopifyData = _container.Resolve<ShopifyWrapper>().GetData();
            var updateList = _container.Resolve<ExcelParserService>().GetUpdatingList(@"C:\Schmidts Inventory Report.xlsx", _report);
            //_container.Resolve<ShopifyWrapper>().UpdateProductAvailable("222333", 10, shopifyData, _report);

            //System.Console.WriteLine("Hello World!");
            //var files = await _container.Resolve<FileService>().GetFiles();
            //foreach (var file in files)
            //{
            //    System.Console.WriteLine($"Id: {file.Item1}, Name: {file.Item2}");
            //}
        }
    }
}
