using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.Shopify.Models
{
    public class InventoryLevel
    {
        public long Inventory_item_id { get; set; }
        public long Location_id { get; set; }
        public int Available { get; set; }
        public DateTime Updated_at { get; set; }
        public string Admin_graphql_api_id { get; set; }
    }
}
