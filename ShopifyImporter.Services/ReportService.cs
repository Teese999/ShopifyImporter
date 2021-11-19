using ShopifyImporter.Contracts;
using ShopifyImporter.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{
    public class ReportService : IReportService
    {
        public string Build(IEnumerable<InventoryDto> inventories, string fileName)
        {
            var successfullInventories = inventories.Where(i => !i.HasError);
            var failedInventories = inventories.Where(i => i.HasError);
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"{fileName} file import details");
            strBuilder.AppendLine($"Successfully imported: {successfullInventories.Count()}");
            strBuilder.AppendLine($"Failed ot import: {failedInventories.Count()}");
            if (failedInventories.Count() > 0)
            {
                strBuilder.AppendLine($"Details:");
                foreach (var failedInventory in failedInventories)
                {
                    strBuilder.AppendLine($"SKU: \"{failedInventory.Sku}\", Error: \"{failedInventory.ErrorMessage}\"");
                }
            }

            return strBuilder.ToString();
        }
    }
}
