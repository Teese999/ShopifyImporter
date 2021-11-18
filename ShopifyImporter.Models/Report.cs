using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Models
{
    public class Report
    {
        public List<(string, string)> SkuFailed { get; set; } = new();
        public int SuccessImportCounter { get; set; }
    }
}
