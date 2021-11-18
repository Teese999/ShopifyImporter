using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Contracts
{
    public interface IExcelParserService
    {
        List<(string, int)> GetUpdatingList(string filePath);
    }
}
