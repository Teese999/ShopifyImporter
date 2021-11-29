using Serilog;
using ShopifyImporter.Contracts;
using ShopifyImporter.Contracts.Models;
using ShopifyImporter.Integrations.Shopify.Contracts;
using ShopifySharp;
using ShopifySharp.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopifyImporter.Integrations.Shopify
{
    public class ShopifyWrapper : IShopifyWrapper
    {
        private string _shopUrl { get; set; }
        private string _shopPassword { get; set; }
        private ILogger _logger;
        private string _shopApiKey { get; set; }
        private const int _productsPerRequest = 250;
        private const long _locationId = 64850624673;
        public ShopifyWrapper(Settings settings, ILogger logger)
        {
            _shopUrl = settings.Shopify.ShopUrl;
            _shopPassword = settings.Shopify.Password;
            _logger = logger;
        }

        public async Task<IEnumerable<InventoryDto>> Run(IEnumerable<InventoryDto> inventories)
        {
            //var service = new LocationService(_shopUrl, _shopPassword);
            //var locations = await service.ListAsync();

            var productService = new ProductService(_shopUrl, _shopPassword);
            var filter = new ProductListFilter()
            {
                Limit = _productsPerRequest,
            };

            List<Product> allProducts = new List<Product>();
            var totalCount = await productService.CountAsync();
            _logger.Information($"{totalCount} products to load");

            var products = await productService.ListAsync(filter);

            while (products.HasNextPage)
            {
                var sw = Stopwatch.StartNew();
                allProducts.AddRange(products.Items);
                products = await productService.ListAsync(products.GetNextPageFilter());
                sw.Stop();
                var delay = 1000 - sw.ElapsedMilliseconds;

                if (delay > 0)
                {
                    await Task.Delay((int)delay);
                }

                _logger.Information($"{allProducts.Count} products loaded");
            }

            foreach (var inventory in inventories)
            {

                var sw = Stopwatch.StartNew();

                var product = allProducts.FirstOrDefault(x => x.Variants.Any(z => z.SKU == inventory.Sku));

                if (product == null)
                {
                    inventory.HasError = true;
                    inventory.ErrorMessage = $"SKU: {inventory.Sku} failed - Product not found in Shopify";
                    continue;
                }

                var variant = product.Variants.FirstOrDefault(x => x.SKU == inventory.Sku);

                if (variant == null)
                {
                    inventory.HasError = true;
                    inventory.ErrorMessage = $"SKU: {inventory.Sku} failed - Product Variant not found in Shopify";
                    continue;
                }

                try
                {
                    var invLocService = new InventoryLevelService(_shopUrl, _shopPassword);
                    var inventoryLevel = new InventoryLevel()
                    {
                        InventoryItemId = variant.InventoryItemId,
                        LocationId = _locationId,
                        Available = inventory.Quantity
                    };
                    await invLocService.SetAsync(inventoryLevel);
                }
                catch (Exception ex)
                {
                    inventory.HasError = true;
                    inventory.ErrorMessage = $"SKU: {inventory.Sku} failed - {ex.Message}";
                    continue;
                }

                sw.Stop();
                var delay = 1000 - sw.ElapsedMilliseconds;

                if (delay > 0)
                {
                    await Task.Delay((int)delay);
                }

                _logger.Information($"SKU: {inventory.Sku} InventoryQuantity: {inventory.Quantity}");

            }
            return inventories;
        }
    }
}
