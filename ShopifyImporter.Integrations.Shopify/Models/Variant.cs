using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.Shopify.Models
{
    public class Variant
    {
        public long Product_id { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public string Sku { get; set; }
        public int Position { get; set; }
        public string Inventory_policy { get; set; }
        public string Compare_at_price { get; set; }
        public string Fulfillment_service { get; set; }
        public string Inventory_management { get; set; }
        public string Option1 { get; set; }
        public object Option2 { get; set; }
        public object Option3 { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public bool Taxable { get; set; }
        public string Barcode { get; set; }
        public int Grams { get; set; }
        public object Image_id { get; set; }
        public double Weight { get; set; }
        public string Weight_unit { get; set; }
        public long Inventory_item_id { get; set; }
        public int Inventory_quantity { get; set; }
        public int Old_inventory_quantity { get; set; }
        public bool Requires_shipping { get; set; }
        public string Admin_graphql_api_id { get; set; }
    }
}
