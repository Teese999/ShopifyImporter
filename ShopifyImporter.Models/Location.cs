using System;

namespace ShopifyImporter.Models
{
    public class Location
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public object Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public string Country_code { get; set; }
        public string Country_name { get; set; }
        public string Province_code { get; set; }
        public bool Legacy { get; set; }
        public bool Active { get; set; }
        public string Admin_graphql_api_id { get; set; }
        public string Localized_country_name { get; set; }
        public string Localized_province_name { get; set; }
    }
}
