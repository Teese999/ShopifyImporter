using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Models
{
    public class Option
    {
        public long Product_id { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public List<string> Values { get; set; }
    }
}
