using ShopifyImporter.Contracts;
using ShopifyImporter.Contracts.Models;
using ShopifyImporter.Integrations.Shopify;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{
    public class ShopifyService : IShopifyService
    {
        private ShopifyWrapper _shopifyWrapper;

        public ShopifyService(ShopifyWrapper shopifyWrapper)
        {
            _shopifyWrapper = shopifyWrapper;
        }

        public async Task<IEnumerable<InventoryDto>> UpdateInventoriesAsync(IEnumerable<InventoryDto> inventories)
        {
            var availableInventories = inventories.Where(x => !x.HasError);
            inventories = await _shopifyWrapper.Run(inventories);
            return inventories;
        }
    }
}
