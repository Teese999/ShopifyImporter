using ShopifyImporter.Contracts;
using ShopifyImporter.Models;
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
    public class Startup
    {
        private IUnityContainer _container = new UnityContainer();
        private Report _report = new();
        public async void Run()
        {
            ConfigureContainer(_container);

            var shopifyData = _container.Resolve<IShopifyService>().GetData();
            var updateList = _container.Resolve<IExcelParserService>().GetUpdatingList(@"C:\Schmidts Inventory Report.xlsx", _report);
            _container.Resolve<IShopifyService>().UpdateProductAvailable("222333", 10, shopifyData, _report);


        }
        private void ConfigureContainer(IUnityContainer container)
        {
            ContainerConfiguration.RegisterTypes<HierarchicalLifetimeManager>(_container);
        }
    }
}
