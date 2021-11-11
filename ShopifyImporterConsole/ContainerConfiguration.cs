using Microsoft.Extensions.Configuration;
using ShopifyImporter.Contracts;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporterConsole
{
    public class ContainerConfiguration
    {
        public static void RegisterTypes<TLifetime>(IUnityContainer container)
           where TLifetime : ITypeLifetimeManager, new()
        {
            container.RegisterType<IGoogleService, GoogleService>(new TLifetime());
        }
    }
}
