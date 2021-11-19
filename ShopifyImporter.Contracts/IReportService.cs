using ShopifyImporter.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public interface IReportService
    {
        string Build(IEnumerable<InventoryDto> inventories, string fileName);
    }
}
