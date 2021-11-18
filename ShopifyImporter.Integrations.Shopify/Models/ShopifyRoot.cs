using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.Shopify.Models
{
    public class ShopifyRoot
    {
        public List<Product> Products { get; set; } = new();
        public List<Variant> Variants { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<InventoryLevel> Inventory_levels { get; set; } = new();
    }
}
