using Microsoft.Extensions.Configuration;
using ShopifyImporter.Contracts;
using ShopifyImporter.Services;
using System;
using System.IO;
using System.Linq;
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
            var fileService = _container.Resolve<IFileService>();

            if (args.Length > 0)
            {
                if (args[0] == "-run")
                {
                    await commonService.Execute();
                }
                else if (args[0] == "-drives")
                {
                    var availableDrives = await fileService.GetDrives();
                    System.Console.WriteLine("List of available drives:");
                    if (availableDrives != null)
                    {
                        foreach (var availableDrive in availableDrives)
                        {
                            System.Console.WriteLine($"- Drive info:");
                            System.Console.WriteLine(availableDrive.OriginalData);
                        }
                    }
                }
                else if (args[0] == "-drivefolders")
                {
                    var folders = await fileService.ListDriveFolders();
                    System.Console.WriteLine($"List of available folders for drive");
                    if (folders != null && folders.Any())
                    {
                        foreach (var folder in folders)
                        {
                            System.Console.WriteLine($"- \"{folder}\"");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("- No folders found.");
                    }
                }
            }
            else
            {
                try
                {
                    var result = await commonService.CheckFileStorageConfiguration();
                    var folders = result.Item1;
                    var createdFolders = result.Item2;
                    var errorMessages = result.Item3;


                    System.Console.WriteLine("List of errors:");
                    if (errorMessages != null && errorMessages.Any())
                    {
                        foreach (var error in errorMessages)
                        {
                            System.Console.WriteLine($"- \"{error}\"");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("- No errors occurred.");
                    }

                    if (createdFolders != null && createdFolders.Any())
                    {
                        System.Console.WriteLine("List of automatically created folders:");
                        foreach (var createdFolder in createdFolders)
                        {
                            System.Console.WriteLine($"- \"{createdFolder}\"");
                        }
                    }

                    System.Console.WriteLine("List of available folders:");
                    if (folders != null && folders.Any())
                    {
                        foreach (var folder in folders)
                        {
                            System.Console.WriteLine($"- \"{folder}\"");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("- No folders found.");
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"Error: {e.Message}");
                }
            }
        }

    }
}
