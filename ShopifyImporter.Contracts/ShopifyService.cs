using ShopifyImporter.Integrations.Shopify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public interface ShopifyService
    {
        ShopifyRoot GetData();
        void UpdateProductAvailable(string sku, int newAvailableValue, ShopifyRoot root, Report report);
    }
}
