using Microsoft.Extensions.Configuration;
using ShopifyImporter.Contracts;
using ShopifyImporter.Services;
using System.IO;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporter.Console
{
    public class Program
    {

        private static IUnityContainer _container = new UnityContainer();

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var settings = builder.GetSection("Settings").Get<Settings>();
            _container.RegisterInstance(settings);

            ContainerConfiguration.RegisterTypes<HierarchicalLifetimeManager>(_container);

            if (!Directory.Exists(settings.IncomingDownloadFolderName))
            {
                Directory.CreateDirectory(settings.IncomingDownloadFolderName);
            }

            var commonService = _container.Resolve<ICommonService>();
            
            if (args.Length > 0 && args[0] == "-r")
            {
                await commonService.Execute();
            }
            else
            {
                await commonService.Authenticate();
            }
        }
        public static Settings GetSettings()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.json", false, true)
               .AddEnvironmentVariables()
               .Build();

            return builder.GetSection("Settings").Get<Settings>();
        }
    }
}
