using ISAA.Rules.Ali.Model;
using ShopifySharp;
using ShopifySharp.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation.Common
{
    public static class ShopifyHelper
    {
        public static ShopifyProduct ChangeShopifyProduct(AliShopifyProduct refProduct, CalcPriceModel[] calculationPrices, ShopifyProduct shopifyProduct, int stockSafetyMargin, string handleFriendly, string title)
        {
            var shopifyVariants = default(IEnumerable<ShopifyProductVariant>);
            var shopifyImages = default(IEnumerable<ShopifyProductImage>);
            var aliProductSpecificBrandName = refProduct.AliProduct.AliProductSpecific.Where(i => i.Name.ToLower() == "brand name").FirstOrDefault();
            var shopsFromCountry = "China";
            var shipsFromQuery = refProduct.AliProduct.AliProductOption
                .Where(i => i.Name.Equals("Ships From", StringComparison.InvariantCultureIgnoreCase));

            if (shopifyProduct == null)
            {
                shopifyProduct = new ShopifyProduct();
                shopifyVariants = new ShopifyProductVariant[0];
                shopifyImages = new ShopifyProductImage[0];
            }
            else
            {
                shopifyVariants = shopifyProduct.Variants;
                shopifyImages = shopifyProduct.Images;
            }

            shopifyProduct.Title = title;
            shopifyProduct.Handle = handleFriendly;
            shopifyProduct.BodyHtml = "<h3 style='margin-top: 25px;'>Item Specifics</h3>";
            shopifyProduct.BodyHtml += string.Join("<br />",
                refProduct.AliProduct.AliProductSpecific
                    .Where(i => i.Type == "item-specifics")
                    .Select(i => $"{Program.ToTitle(i.Name)}: {Program.ToTitle(i.Value)}")
            );
            shopifyProduct.BodyHtml += "<br /><h3 style='margin-top: 15px;'>Packaging Details</h3>";
            shopifyProduct.BodyHtml += string.Join("<br />",
            refProduct.AliProduct.AliProductSpecific
                .Where(i => i.Type == "packaging-details")
                .Select(i => $"{Program.ToTitle(i.Name)}: {Program.ToTitle(i.Value)}")
            );

            if (!shipsFromQuery.Any() ||
                refProduct.AliProduct.AliProductOption
                    .Any(i =>
                        i.Name.Equals("Ships From", StringComparison.InvariantCultureIgnoreCase) &&
                        refProduct.AliProduct.AliProductVariant
                            .Where(v => v.Enabled)
                            .Select(v => i.Number == 1 ? v.Option1 : i.Number == 2 ? v.Option2 : v.Option3)
                            .Contains(shopsFromCountry, StringComparer.InvariantCultureIgnoreCase)
                        ))
            {
                if (!refProduct.AliProduct.Enabled && shopifyProduct.PublishedAt != null)
                {
                    shopifyProduct.Published = false;
                    shopifyProduct.PublishedAt = null;
                }

                if (refProduct.AliProduct.Enabled && shopifyProduct.PublishedAt == null)
                {
                    shopifyProduct.Published = true;
                    shopifyProduct.PublishedAt = DateTime.UtcNow;
                }
            }
            else
            {
                shopifyProduct.Published = false;
                shopifyProduct.PublishedAt = null;
            }

            if (aliProductSpecificBrandName == null)
            {
                shopifyProduct.Vendor = null;
            }
            else
            {
                shopifyProduct.Vendor = Program.ToTitle(aliProductSpecificBrandName.Value);
            }

            if (shipsFromQuery.Any())
            {
                shopifyProduct.Options = refProduct.AliProduct.AliProductOption
                    .Where(i => !i.Name.Equals("Ships From", StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy(i => i.Number)
                    .Select(i => new ShopifyProductOption
                    {
                        Name = Program.ToTitle(i.Name),
                        Values = refProduct.AliProduct.AliProductVariant
                            .Where(v => v.Enabled)
                            .Select(v => i.Number == 1 ? v.Option1 : i.Number == 2 ? v.Option2 : v.Option3)
                            .Distinct()
                    });
            }
            else
            {
                shopifyProduct.Options = refProduct.AliProduct.AliProductOption
                    .OrderBy(i => i.Number)
                    .Select(i => new ShopifyProductOption
                    {
                        Name = Program.ToTitle(i.Name),
                        Values = refProduct.AliProduct.AliProductVariant
                            .Where(v => v.Enabled)
                            .Select(v => i.Number == 1 ? v.Option1 : i.Number == 2 ? v.Option2 : v.Option3)
                            .Distinct()
                    });
            }

            if (shopifyProduct.Options.Any())
            {
                shopifyProduct.Options = shopifyProduct.Options.ToArray();

                var position = 0;

                foreach (var option in shopifyProduct.Options)
                {
                    option.Position = ++position;
                }
            }
            else
            {
                shopifyProduct.Options = null;
            }

            shopifyProduct.Images = refProduct.AliProduct.AliProductImage
                .Select(i => i.Url)
                .Distinct()
                .Select(delegate (string url)
                {
                    var shopifyImage = GetShopifyImageFromUrl(url, shopifyImages);

                    if (shopifyImage == null)
                    {
                        return new ShopifyProductImage
                        {
                            Src = url,
                            VariantIds = new long[0]
                        };
                    }

                    return new ShopifyProductImage
                    {
                        Id = shopifyImage.Id,
                        VariantIds = new long[0]
                    };
                });

            shopifyProduct.Variants = refProduct.AliProduct.AliProductVariant
                .Where(i => i.Enabled && i.AvailableQuantity > 0)
                .OrderByDescending(i => i.AvailableQuantity)
                .Select(delegate (AliProductVariant variant)
                {
                    var skuRefID = variant.AliProductVariantId.ToString();
                    var shopifyVariant = shopifyVariants.FirstOrDefault(i => i.SKU == skuRefID);
                    var currentPrice = calculationPrices.First(i => i.AliProductVariantId == variant.AliProductVariantId);
                    var shopifyImage = default(ShopifyProductImage);

                    if (variant.AliProductImage != null)
                    {
                        shopifyImage = GetShopifyImageFromUrl(variant.AliProductImage.Url, shopifyImages);
                    }

                    var model = new ShopifyProductVariant
                    {
                        Barcode = skuRefID,
                        FulfillmentService = ShopifyProductFulfillmentService.Manual,
                        InventoryManagement = ShopifyProductInventoryManagement.Shopify,
                        InventoryPolicy = ShopifyProductInventoryPolicy.Deny,
                        ImageId = shopifyImage == null ? null : shopifyImage.Id,
                        Price = (double)currentPrice.CalcPrice,
                        CompareAtPrice = currentPrice.CalcCompareAtPrice == null ? 0 : (double)currentPrice.CalcCompareAtPrice,
                        RequiresShipping = true,
                        SKU = skuRefID,
                        Taxable = false,
                        Title = refProduct.AliProduct.AliProductVariant
                            .Where(i => i.Enabled).Count() - shipsFromQuery.Count() <= 1 ?
                                Program.ToTitle(refProduct.AliProduct.Title) :
                                null,
                        Weight = 0.1,
                        WeightUnit = "kg",
                    };

                    var shipsFrom = shipsFromQuery.FirstOrDefault();

                    if (currentPrice.UseDiscount && variant.DiscountPrice.Value > 1.99M ||
                        !currentPrice.UseDiscount && variant.OriginalPrice > 1.99M ||
                        shipsFrom != null && (
                            shipsFrom.Number == 1 && !variant.Option1.Equals(shopsFromCountry, StringComparison.InvariantCultureIgnoreCase) ||
                            shipsFrom.Number == 2 && !variant.Option2.Equals(shopsFromCountry, StringComparison.InvariantCultureIgnoreCase) ||
                            shipsFrom.Number == 3 && !variant.Option3.Equals(shopsFromCountry, StringComparison.InvariantCultureIgnoreCase)
                        ))
                    {
                        return null;
                    }

                    if (shipsFrom == null)
                    {
                        model.Option1 = Program.ToTitle(variant.Option1);
                        model.Option2 = Program.ToTitle(variant.Option2);
                        model.Option3 = Program.ToTitle(variant.Option3);
                    }
                    else
                    {
                        if (shipsFrom.Number == 1)
                        {
                            model.Option1 = Program.ToTitle(variant.Option2);
                            model.Option2 = Program.ToTitle(variant.Option3);
                        }

                        if (shipsFrom.Number == 2)
                        {
                            model.Option1 = Program.ToTitle(variant.Option1);
                            model.Option2 = Program.ToTitle(variant.Option3);
                        }

                        if (shipsFrom.Number == 3)
                        {
                            model.Option1 = Program.ToTitle(variant.Option1);
                            model.Option2 = Program.ToTitle(variant.Option2);
                        }
                    }

                    if (string.IsNullOrWhiteSpace(model.Option1) ||
                        string.IsNullOrWhiteSpace(model.Option2) &&
                        (
                            refProduct.AliProduct.AliProductVariant.GroupBy(i => i.Option1).Any(i => i.Count() > 1) ||
                            Regex.IsMatch(model.Option1, "^\\d*$"))
                        )
                    {
                        model.Option1 = "SKU " + variant.AliProductVariantId;
                    }

                    if (variant.AliProductImageId != null)
                    {
                        model.Option1 += " (As Picture)";
                    }

                    var stockQuantity = variant.AvailableQuantity.Value - stockSafetyMargin;

                    if (stockQuantity < 0)
                    {
                        stockQuantity = 0;
                    }

                    if (shopifyVariant == null)
                    {
                        model.InventoryQuantity = stockQuantity;
                    }
                    else
                    {
                        model.Id = shopifyVariant.Id;
                        model.Position = shopifyVariant.Position;
                        model.InventoryQuantityAdjustment = stockQuantity - shopifyVariant.InventoryQuantity;
                    }

                    return model;
                });

            shopifyProduct.Variants = shopifyProduct.Variants
                .Where(i => i != null)
                .Take(100)
                .ToArray();

            if (!shopifyProduct.Variants.Any())
            {
                shopifyProduct.Options = null;
                shopifyProduct.Variants = null;
                shopifyProduct.Published = false;
                shopifyProduct.PublishedAt = null;
            }

            return shopifyProduct;
        }

        public static ShopifyProductImage GetShopifyImageFromUrl(string url, IEnumerable<ShopifyProductImage> shopifyImages)
        {
            if (shopifyImages == null || !shopifyImages.Any())
            {
                return null;
            }

            var imageFileName = Path.GetFileNameWithoutExtension(url);
            var shopifyImage = shopifyImages.FirstOrDefault(delegate (ShopifyProductImage i)
            {
                var fileName = Path.GetFileNameWithoutExtension(i.Src);

                return fileName.Equals(imageFileName, StringComparison.InvariantCultureIgnoreCase);
            });

            return shopifyImage;
        }

        public static string ShopifyHandleFriendlyName(long attachId, string title)
        {
            var segment = title + "-" + attachId;
            var bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(segment);
            var handleFriendly = Regex.Replace(Encoding.UTF8.GetString(bytes), @"[^\w\d]", "-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            handleFriendly = Regex.Replace(handleFriendly, "\\-{1,}", "-");

            if (handleFriendly.StartsWith("-"))
            {
                handleFriendly = handleFriendly.Substring(1);
            }

            return handleFriendly.ToLowerInvariant();
        }

        public static string[] GetShopifyTags(string tags)
        {
            return tags
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToArray();
        }
    }
}
