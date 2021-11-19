using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{
    public class ReportDto
    {
        public List<(string, string)> SkuFailed { get; set; } = new();
        public int SuccessImportCounter { get; set; }
    }
}
