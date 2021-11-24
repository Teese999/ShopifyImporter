using ShopifyImporter.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.Contracts
{
    public interface IShopifyWrapper
    {
        Task<IEnumerable<InventoryDto>> Run(IEnumerable<InventoryDto> inventories);
    }
}
