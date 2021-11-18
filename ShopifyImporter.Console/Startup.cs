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
    public class Startup
    {
        private IUnityContainer _container = new UnityContainer();
        public async void Run()
        {
            ConfigureContainer(_container);

            var shopifyData = _container.Resolve<IShopifyService>().GetData();
            var updateList = _container.Resolve<IExcelParserService>().GetUpdatingList(@"C:\Schmidts Inventory Report.xlsx");
            _container.Resolve<IShopifyService>().UpdateProductAvailable("222333", 10, shopifyData);


        }
        private void ConfigureContainer(IUnityContainer container)
        {
            ContainerConfiguration.RegisterTypes<HierarchicalLifetimeManager>(_container);
        }
    }
}
