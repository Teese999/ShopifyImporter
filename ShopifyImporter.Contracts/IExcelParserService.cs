using ShopifyImporter.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public interface IExcelParserService
    {
        IEnumerable<InventoryDto> ParseFile(string fileName);
    }
}
