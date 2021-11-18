using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body_html { get; set; }
        public string Vendor { get; set; }
        public string Product_type { get; set; }
        public DateTime Created_at { get; set; }
        public string Handle { get; set; }
        public DateTime Updated_at { get; set; }
        public object Published_at { get; set; }
        public string Template_suffix { get; set; }
        public string Status { get; set; }
        public string Published_scope { get; set; }
        public string Tags { get; set; }
        public string Admin_graphql_api_id { get; set; }
        public List<Variant> Variants { get; set; }
        public List<Option> Options { get; set; }
        public List<object> Images { get; set; }
        public object Image { get; set; }
    }
}
