using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShopifyImporter.Contracts;
using ShopifyImporter.Models;
using ShopifyImporter.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopifyImporter.Services
{

    public class ShopifyService : IShopifyService
    {
        private Settings _settings;

        public ShopifyService(Settings settings)
        {
            _settings = settings;
        }

        public ShopifyRoot GetData()
        {
            ShopifyRoot shopifyRoot = new();


            //configure RestClient
            var client = new RestClient(_settings.Shopify.ShopUrl);
            client.Authenticator = new HttpBasicAuthenticator(_settings.Shopify.ShopApiKey, _settings.Shopify.ShopAccessToken);

            string resource = null;
            RestRequest request = new();
            request.AddUrlSegment("status", "open");
            request.AddHeader("header", "Content-Type: application/json");
            IRestResponse response;

            //get variants
            resource = "/admin/api/2021-07/products.json";
            request.Resource = resource;
            request.Method = Method.GET;
            response = client.Execute(request);

            shopifyRoot.Products = JsonConvert.DeserializeObject<ShopifyRoot>(response.Content).Products;
            foreach (Product p in shopifyRoot.Products)
            {
                foreach (Variant v in p.Variants)
                {
                    shopifyRoot.Variants.Add(v);
                }
            }

            //get locations
            resource = "/admin/api/2021-10/locations.json";
            request.Resource = resource;
            response = client.Execute(request);
            shopifyRoot.Locations = JsonConvert.DeserializeObject<ShopifyRoot>(response.Content).Locations;

            //get inventory levels
            foreach (var id in shopifyRoot.Locations.Select(x => x.Id).ToList())
            {
                resource = $"/admin/api/2021-10/locations/{id}/inventory_levels.json";
                request.Resource = resource;
                response = client.Execute(request);

                shopifyRoot.Inventory_levels.AddRange(JsonConvert.DeserializeObject<ShopifyRoot>(response.Content).Inventory_levels);
            }
            return shopifyRoot;
        }
        public void UpdateProductAvailable(string sku, int newAvailableValue, ShopifyRoot root, Report report)
        {
            long inventoryItemId = 0;
            try
            {
               inventoryItemId = root.Variants.FirstOrDefault(x => x.Sku == sku).Inventory_item_id;
            }
            catch (NullReferenceException)
            {

                report.SkuFailed.Add((sku, "SKU not found"));
            }
            var locationId = root.Inventory_levels.FirstOrDefault(x => x.Inventory_item_id == inventoryItemId).Location_id;
            var availableCurrentValue = root.Inventory_levels.FirstOrDefault(x => (x.Inventory_item_id == inventoryItemId && x.Location_id == locationId)).Available;

            var availableAdjustment = newAvailableValue - availableCurrentValue;

            //updating on shopify
            var client = new RestClient(_settings.Shopify.ShopUrl);
            client.Authenticator = new HttpBasicAuthenticator(_settings.Shopify.ShopApiKey, _settings.Shopify.ShopAccessToken);
            string resource = "/admin/api/2021-10/inventory_levels/adjust.json";
            var requestres = new RestRequest(resource, Method.POST);
            requestres.AddUrlSegment("status", "open");
            requestres.AddHeader("header", "Content-Type: application/json");
            requestres.AddParameter("inventory_item_id", inventoryItemId);
            requestres.AddParameter("location_id", locationId);
            requestres.AddParameter("available_adjustment", availableAdjustment);
            IRestResponse response = client.Execute(requestres);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                report.SuccessImportCounter++;
            }
            else
            {
                report.SkuFailed.Add((sku, response.ErrorMessage));
            }
        }
    }
}
