using Microsoft.Extensions.Configuration;
using System;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporterConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            new Startup().StartProgram();
            Console.ReadLine();
        }

    }
}
