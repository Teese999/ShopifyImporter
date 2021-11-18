using ShopifyImporter.Contracts;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace ShopifyImporter.Console
{
    public class ContainerConfiguration
    {
        public static void RegisterTypes<TLifetime>(IUnityContainer container)
           where TLifetime : ITypeLifetimeManager, new()
        {
            container.RegisterType<IShopifyService, ShopifyService>(new TLifetime());
            container.RegisterType<IExcelParserService, ExcelParserService>(new TLifetime());
        }
    }
}
