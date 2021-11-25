using ShopifyImporter.Contracts;
using ShopifyImporter.Contracts.Models;
using ShopifyImporter.Integrations.Shopify;
using ShopifyImporter.Integrations.Shopify.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace ShopifyImporter.Services
{
    public class ShopifyService : IShopifyService
    {
        private IShopifyWrapper _shopifyWrapper;
        public ShopifyService(IUnityContainer container)
        {
            _shopifyWrapper = container.Resolve<IShopifyWrapper>();
        }

        public async Task<IEnumerable<InventoryDto>> UpdateInventoriesAsync(IEnumerable<InventoryDto> inventories)
        {
            var availableInventories = inventories.Where(x => !x.HasError);
            inventories = await _shopifyWrapper.Run(inventories);
            return inventories;
        }
    }
}
