using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts.Models
{
    public class InventoryDto
    {
        public string Sku { get; set; }
        public int? Quantity { get; set; }
        public bool HasError { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
