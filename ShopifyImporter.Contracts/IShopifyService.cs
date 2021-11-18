using ShopifyImporter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public interface IShopifyService
    {
        ShopifyRoot GetData();
        void UpdateProductAvailable(string sku, int newAvailableValue, ShopifyRoot root, Report report);
    }
}
