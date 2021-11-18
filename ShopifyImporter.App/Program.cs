using ShopifyImporter.Services;
using System;
using System.Threading.Tasks;

namespace ShopifyImporter.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var files = await new MicrosoftOneDriveService().GetFiles();
            foreach (var file in files)
            {
                Console.WriteLine($"Id: {file.Item1}, Name: {file.Item2}");
            }
        }
    }
}
