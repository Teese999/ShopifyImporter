using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopifyImporter.Contracts.Models;
using ShopifyImporter.Integrations.Shopify.Contracts;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ShopifyImporter.Services.Tests
{
    [TestClass()]
    public class ShopifyServiceTests : AbstractTest
    {
        IShopifyWrapper _shopifyWrapper;

        public ShopifyServiceTests()
        {
            _shopifyWrapper = _container.Resolve<IShopifyWrapper>();
        }

        [TestMethod()]
        public void UpdateInventoriesAsyncTest()
        {
            var inventories = new List<InventoryDto>();
            inventories.Add(new InventoryDto() { Sku = "001", ErrorMessage = "errorMessage1", HasError = false, Quantity = 1 });
            inventories.Add(new InventoryDto() { Sku = "002", ErrorMessage = "errorMessage2", HasError = false, Quantity = 2 });
            inventories.Add(new InventoryDto() { Sku = "003", ErrorMessage = "errorMessage3", HasError = true, Quantity = 3 });
            inventories.Add(new InventoryDto() { Sku = "004", ErrorMessage = "errorMessage4", HasError = true, Quantity = 4 });

           
            try
            {
                var answer = _shopifyWrapper.Run(inventories).Result;
            }
            catch (Exception e)
            {

                Assert.Fail(e.Message);
            }
        }
    }
}