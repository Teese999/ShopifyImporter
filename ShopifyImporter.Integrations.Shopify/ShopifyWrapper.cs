using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using ShopifyImporter.Contracts;
using ShopifyImporter.Integrations.Shopify.Models;
using ShopifyImporter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.Shopify
{
    public class ShopifyWrapper
    {
        private string _shopUrl { get; set; }
        private string _shopAccessToken { get; set; }
        private string _shopApiKey { get; set; }
        private const int _productsPerRequest = 250;
        public ShopifyWrapper(Settings settings)
        {
            _shopUrl = settings.Shopify.ShopUrl;
            _shopAccessToken = settings.Shopify.Password;
            _shopApiKey = settings.Shopify.ShopApiKey;
        }

        public ShopifyRoot GetData()
        {
            ShopifyRoot shopifyRoot = new();


            //configure RestClient
            var client = new RestClient(_shopUrl);
            client.Authenticator = new HttpBasicAuthenticator(_shopApiKey, _shopAccessToken);

            string resource = null;
            RestRequest request = new();
            request.AddUrlSegment("status", "open");
            request.AddHeader("header", "Content-Type: application/json");
            request.AddParameter("limit", _productsPerRequest);
            IRestResponse response;

            //get variants
            resource = "/admin/api/2021-07/products.json";
            request.Resource = resource;
            request.Method = Method.GET;
            response = client.Execute(request);
            var products = JsonConvert.DeserializeObject<ShopifyRoot>(response.Content).Products;
            shopifyRoot.Products = products;
            if (shopifyRoot.Products.Count == _productsPerRequest)
            {
                PanigationEndChecker(shopifyRoot.Products);
            }
          
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
        public void UpdateProductAvailable(string sku, int newAvailableValue, ShopifyRoot root, ReportDto report)
        {
            long inventoryItemId = 0;

            var variant = root.Variants.FirstOrDefault(x => x.Sku == sku);
            if (variant == null)
            {
                report.SkuFailed.Add((sku, "SKU not found"));
                return;
            }
            inventoryItemId = variant.Inventory_item_id;

            var inventoryLevel = root.Inventory_levels.FirstOrDefault(x => x.Inventory_item_id == inventoryItemId);
            if (inventoryLevel == null)
            {
                report.SkuFailed.Add((sku, $"Inventory level not founded by InventoryItemId {inventoryItemId}"));
                return;
            }
            var locationId = inventoryLevel.Location_id;
            var availableCurrentValue = inventoryLevel.Available;

            var availableAdjustment = newAvailableValue - availableCurrentValue;

            //updating on shopify
            var client = new RestClient(_shopUrl);
            client.Authenticator = new HttpBasicAuthenticator(_shopApiKey, _shopAccessToken);
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
        private void PanigationEndChecker(List<Product> products)
        {
            ShopifyRoot shopifyRoot = new();

            var client = new RestClient(_shopUrl);
            client.Authenticator = new HttpBasicAuthenticator(_shopApiKey, _shopAccessToken);

            string resource = null;
            RestRequest request = new();
            request.AddUrlSegment("status", "open");
            request.AddHeader("header", "Content-Type: application/json");
            request.AddParameter("limit", _productsPerRequest);
            request.AddParameter("since_id", products.Last().Id);
            IRestResponse response;
            resource = "/admin/api/2021-07/products.json";
            request.Resource = resource;
            request.Method = Method.GET;
            response = client.Execute(request);
            var gettedProducts = JsonConvert.DeserializeObject<ShopifyRoot>(response.Content).Products;
            products.AddRange(gettedProducts);
            if (gettedProducts.Count == _productsPerRequest)
            {
                PanigationEndChecker(products);
            }            
        }
    }
}
