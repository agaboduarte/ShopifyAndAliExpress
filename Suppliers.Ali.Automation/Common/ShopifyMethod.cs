using ISAA.Rules.Ali.Model;
using ShopifySharp;
using ShopifySharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation.Common
{
    public static class ShopifyMethod
    {
        internal static ShopifyProduct UnpublishProductOnShopify(ShopifyProductService shopifyProductService, long shopifyProductId)
        {
            var updateShopifyProduct = default(ShopifyProduct);
            var shopifyProduct = new ShopifyProduct
            {
                Id = shopifyProductId,
                Published = false
            };

            ShopifyCall.ExecuteCall(delegate ()
            {
                var task = shopifyProductService.UpdateAsync(shopifyProduct);

                task.Wait();

                updateShopifyProduct = task.Result;
            });

            return updateShopifyProduct;
        }

        internal static ShopifyProduct GetProductOnShopify(ShopifyProductService shopifyProductService, string handle)
        {
            var shopifyProduct = default(ShopifyProduct);

            ShopifyCall.ExecuteCall(delegate ()
            {
                var task = shopifyProductService.ListAsync(new ShopifyProductFilterOptions
                {
                    Handle = handle
                });

                task.Wait();

                shopifyProduct = task.Result.FirstOrDefault();
            });

            return shopifyProduct;
        }

        internal static ShopifyProduct SaveProductOnShopify(ShopifyProductService shopifyProductService, ShopifyProduct shopifyProduct)
        {
            var saveShopifyProduct = default(ShopifyProduct);

            ShopifyCall.ExecuteCall(delegate ()
            {
                var task = default(Task<ShopifyProduct>);

                if (shopifyProduct.Id == null)
                {
                    task = shopifyProductService.CreateAsync(shopifyProduct);
                }
                else
                {
                    task = shopifyProductService.UpdateAsync(shopifyProduct);
                }

                task.Wait();

                saveShopifyProduct = task.Result;
            });

            return saveShopifyProduct;
        }

        internal static ShopifyProduct UpdateVariantsOnShopify(ShopifyProductService shopifyProductService, AliShopifyProduct refProduct, ShopifyProduct shopifyProduct)
        {
            var updateShopifyProduct = default(ShopifyProduct);

            if (shopifyProduct.Variants.Any())
            {
                shopifyProduct.Variants = shopifyProduct.Variants
                    .Select(delegate (ShopifyProductVariant shopifyVariant)
                    {
                        if (string.IsNullOrWhiteSpace(shopifyVariant.SKU) && shopifyProduct.Variants.Count() == 1)
                        {
                            return shopifyVariant;
                        }

                        var variant = refProduct.AliProduct.AliProductVariant.FirstOrDefault(i => i.AliProductVariantId.ToString() == shopifyVariant.SKU);

                        if (variant == null || !variant.Enabled)
                        {
                            return null;
                        }

                        var shopifyImage = default(ShopifyProductImage);

                        if (variant.AliProductImage != null)
                        {
                            shopifyImage = ShopifyHelper.GetShopifyImageFromUrl(variant.AliProductImage.Url, shopifyProduct.Images);
                        }

                        if (shopifyImage != null)
                        {
                            shopifyVariant.ImageId = shopifyImage.Id;
                        }

                        return shopifyVariant;
                    })
                    .Where(i => i != null)
                    .ToArray();

                ShopifyCall.ExecuteCall(delegate ()
                {
                    var task = shopifyProductService.UpdateAsync(shopifyProduct);

                    task.Wait();

                    updateShopifyProduct = task.Result;
                });
            }

            return updateShopifyProduct ?? shopifyProduct;
        }

        internal static IEnumerable<ShopifyOrder> GetShopifyOpenOrders(ShopifyOrderService shopifyOrderService)
        {
            var listOrder = default(Task<IEnumerable<ShopifyOrder>>);
            var orders = new List<ShopifyOrder>();
            var page = 1;

            do
            {
                ShopifyCall.ExecuteCall(delegate ()
                {
                    listOrder = shopifyOrderService.ListAsync(new ShopifyOrderFilterOptions
                    {
                        Limit = 250,
                        Status = ShopifyOrderStatus.Open,
                        Page = page
                    });

                    listOrder.Wait();
                });

                orders.AddRange(listOrder.Result);

                page++;
            }
            while (listOrder.Result.Count() == 250);

            return orders;
        }

        internal static IEnumerable<ShopifyProduct> GetShopifyAllProduct(ShopifyProductService shopifyProductService, string fields = null)
        {
            var listProduct = default(Task<IEnumerable<ShopifyProduct>>);
            var allProducts = new List<ShopifyProduct>();
            var page = 1;

            do
            {
                ShopifyCall.ExecuteCall(delegate ()
                {
                    listProduct = shopifyProductService.ListAsync(new ShopifyProductFilterOptions
                    {
                        PublishedStatus = "any",
                        Limit = 250,
                        Page = page,
                        Fields = fields
                    });

                    listProduct.Wait();
                });

                allProducts.AddRange(listProduct.Result);

                page++;
            }
            while (listProduct.Result.Count() == 250);

            return allProducts;
        }
    }
}
