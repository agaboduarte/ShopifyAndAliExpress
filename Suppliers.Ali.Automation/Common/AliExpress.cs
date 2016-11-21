using ISAA.Helper.Abbreviation;
using ISAA.Helper.ConsoleHelper;
using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using Newtonsoft.Json;
using RestSharp.Serializers;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ISAA.Suppliers.Ali.Automation.Common
{
    public static class AliExpress
    {
        public static ProductDetailModel GetProductJSON(GetProductModel item)
        {
            var job = new Job(item.Url, JobConfiguration.AliExpressProductScript);

            Print.PrintStatus(item.RefName, "BeforeRun", item.Url);

            Program.Try(item.RefName, item.Url, 3, () => job.Run());

            var productDetail = default(ProductDetailModel);

            if (job.ReturnedJSON != null)
            {
                productDetail = JsonConvert.DeserializeObject<ProductDetailModel>(job.ReturnedJSON);

                Print.PrintStatus(item.RefName, "RunComplete", item.Url, ConsoleColor.Green);
            }
            else
            {
                Print.PrintStatus(item.RefName, "Error", item.Url + Environment.NewLine + job.ReturnedValue, ConsoleColor.Red);
            }

            return productDetail;
        }

        public static void GetProduct(GetProductModel item)
        {
            var job = new Job(item.Url, JobConfiguration.AliExpressProductScript);

            Print.PrintStatus(item.RefName, "BeforeRun", item.Url);

            Program.Try(item.RefName, item.Url, 3, () => job.Run());

            if (job.ReturnedJSON != null)
            {
                var productDetail = JsonConvert.DeserializeObject<ProductDetailModel>(job.ReturnedJSON);

                SaveProduct(productDetail);

                Print.PrintStatus(item.RefName, "RunComplete", item.Url, ConsoleColor.Green);
            }
            else
            {
                Print.PrintStatus(item.RefName, "Error", item.Url + Environment.NewLine + job.ReturnedValue, ConsoleColor.Red);
            }
        }

        public static void UpdateProduct(UpdateProductModel item)
        {
            Print.PrintStatus(item.RefName, "BeforeRun", item.Url);

            var tryStatusOK = Program.Try(item.RefName, item.Url, delegate ()
            {
                var requestUrl = new Uri(item.Url);
                var getRequest = (HttpWebRequest)HttpWebRequest.Create(item.Url);

                getRequest.Method = "GET";
                getRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
                getRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/web";
                getRequest.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.6,en;q=0.4,es;q=0.2");

                getRequest.CookieContainer = new CookieContainer();
                getRequest.CookieContainer.Add(new Cookie("aep_usuc_f", "site=glo&region=US&b_locale=en_US&c_tp=USD", "/", requestUrl.Host.Replace("www.", "")));

                using (var getResponse = getRequest.GetResponse())
                using (var getStream = getResponse.GetResponseStream())
                using (var getReader = new StreamReader(getStream))
                {
                    var contentHtml = getReader.ReadToEnd();
                    var skuProductMatch = Regex.Match(contentHtml, "var skuProducts=.*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    var skuProductsJSON = skuProductMatch.Value.Replace("var skuProducts=", "");
                    var noLongerAvailable = Regex.IsMatch(contentHtml, "id=\"no-longer-available\"", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    var discountTimeLeftMatch = Regex.Match(contentHtml, "class=\"time-left\".*", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    var discountTimeLeftText = discountTimeLeftMatch.Length > 0 ? Regex.Replace(discountTimeLeftMatch.Value, "[^:\\d]", "", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) : null;
                    var discountTimeLeftMinutes = default(int?);
                    var minutesOnDay = 1440;

                    skuProductsJSON = skuProductsJSON.Substring(0, skuProductsJSON.Length - 1);

                    if (discountTimeLeftText != null)
                    {
                        if (discountTimeLeftText.Contains(":"))
                        {
                            var s = discountTimeLeftText.Split(':');

                            discountTimeLeftMinutes = int.Parse(s[0]) * 60 + int.Parse(s[1]);
                        }
                        else
                        {
                            discountTimeLeftMinutes = int.Parse(discountTimeLeftText) * minutesOnDay;
                        }
                    }

                    var skuProductsModel = JsonConvert.DeserializeObject<ProductSkuModel[]>(skuProductsJSON);

                    SaveVariant(item, discountTimeLeftMinutes, skuProductsModel, noLongerAvailable);
                }
            });

            if (!tryStatusOK)
            {
                using (var db = AliShopEntities.New())
                {
                    var rules = RulesCreator.NewRules<AliProductRules>(db);
                    var product = rules.GetById(item.AliProductId, "AliShopifyProduct").First();

                    if (product.AliShopifyProduct.Any())
                    {
                        var aliShopifyProduct = product.AliShopifyProduct.First();

                        aliShopifyProduct.RequiredUpdateOnShopify = true;
                        aliShopifyProduct.LastUpdate = DateTime.UtcNow;
                    }

                    product.Enabled = false;
                    product.LastUpdate = DateTime.UtcNow;

                    db.SaveChanges();
                }

                Print.PrintStatus(item.RefName, "ProductDisabled", item.Url, ConsoleColor.Green);
            }

            Print.PrintStatus(item.RefName, "RunComplete", item.Url, ConsoleColor.Green);
        }

        public static void UpdateShopifyProduct(UpdateShopifyProductModel item)
        {
            using (var db = AliShopEntities.New())
            {
                var shopifyProductRules = RulesCreator.NewRules<AliShopifyProductRules>(db);
                var shopifyPriceRules = RulesCreator.NewRules<AliShopifyPriceRules>(db);
                var parameterRules = RulesCreator.NewRules<AliParameterRules>(db);

                var shopifyProduct = shopifyProductRules.GetById(
                    item.AliShopifyProductId,
                    "AliProduct.AliProductVariant",
                    "AliProduct.AliProductImage",
                    "AliProduct.AliProductLink",
                    "AliProduct.AliProductOption",
                    "AliProduct.AliProductSpecific",
                    "AliProduct.AliStore"
                ).First();

                foreach (var image in shopifyProduct.AliProduct.AliProductImage)
                {
                    if (!image.Url.EndsWith("_640x640.jpg"))
                    {
                        image.Url += "_640x640.jpg";
                    }
                }

                var title = shopifyProduct.AliProduct.Title;
                var invalidKeywords = ShopifyHelper.GetShopifyTags(parameterRules.GetByName("product_title_invalid_keywords").Value);

                foreach (var keyword in invalidKeywords)
                {
                    title = Regex.Replace(title, Regex.Escape(keyword), "", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Trim();
                }

                title = Regex.Replace(title, "\\s{1,}", " ", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                title = new TitleCollection(title).Short(100).Edited;

                Print.PrintStatus(item.RefName, "BeforeRun", title);

                var calculationPrices = shopifyPriceRules.GetFixedCalculationPrices(shopifyProduct.AliProductId);
                var stockSafetyMargin = int.Parse(parameterRules.GetByName("stock_safety_margin").Value);
                var handleFriendlyName = ShopifyHelper.ShopifyHandleFriendlyName(shopifyProduct.AliProductId, title);
                var shopifyProductService = new ShopifyProductService(AppSettings.ShopifyMyShopifyUrl, AppSettings.ShopifyApiKey);
                var shopifyOrderService = new ShopifyOrderService(AppSettings.ShopifyMyShopifyUrl, AppSettings.ShopifyApiKey);
                var getShopifyProduct = default(ShopifyProduct);

                var tryStatusOK = Program.Try(item.RefName, title, 3, () => getShopifyProduct = ShopifyMethod.GetProductOnShopify(shopifyProductService, shopifyProduct.HandleFriendlyName ?? handleFriendlyName));

                Print.PrintStatus(item.RefName, tryStatusOK ? "GetProduct" : "FailGetProduct", title, ConsoleColor.Blue);

                if (tryStatusOK)
                {
                    var changeShopifyProduct = ShopifyHelper.ChangeShopifyProduct(shopifyProduct, calculationPrices, getShopifyProduct, stockSafetyMargin, handleFriendlyName, title);

                    tryStatusOK = Program.Try(item.RefName, title, 3, () => getShopifyProduct = ShopifyMethod.SaveProductOnShopify(shopifyProductService, changeShopifyProduct));

                    Print.PrintStatus(item.RefName, tryStatusOK ? "SaveProduct" : "FailSaveProduct", title, ConsoleColor.Blue);
                }

                if (getShopifyProduct != null && tryStatusOK)
                {
                    tryStatusOK = Program.Try(item.RefName, title, 3, () => getShopifyProduct = ShopifyMethod.UpdateVariantsOnShopify(shopifyProductService, shopifyProduct, getShopifyProduct));

                    Print.PrintStatus(item.RefName, tryStatusOK ? "UpdateVariant" : "FailUpdateVariant", title, ConsoleColor.Blue);
                }

                var hasProductOnShopify = getShopifyProduct != null;

                if (hasProductOnShopify && (!tryStatusOK || !shopifyProduct.AliProduct.Enabled && getShopifyProduct.PublishedAt != null))
                {
                    var unPublishTryStatusOK = Program.Try(item.RefName, title, 3, () => getShopifyProduct = ShopifyMethod.UnpublishProductOnShopify(shopifyProductService, getShopifyProduct.Id.Value));

                    Print.PrintStatus(item.RefName, unPublishTryStatusOK ? "Unpublish" : "FailUnpublish", title, ConsoleColor.Blue);
                }

                shopifyProduct = shopifyProductRules.GetById(item.AliShopifyProductId).Where(i => i.RowVersion == shopifyProduct.RowVersion).FirstOrDefault();

                if (hasProductOnShopify)
                {
                    if (shopifyProduct != null)
                    {
                        shopifyProduct.AvgCompareAtPrice = (decimal?)getShopifyProduct.Variants.Average(i => i.CompareAtPrice);
                        shopifyProduct.AvgPrice = (decimal?)getShopifyProduct.Variants.Average(i => i.Price);
                        shopifyProduct.ExistsOnShopify = true;
                        shopifyProduct.HandleFriendlyName = getShopifyProduct.Handle;
                        shopifyProduct.LastUpdate = DateTime.UtcNow;
                        shopifyProduct.Published = getShopifyProduct.PublishedAt != null;
                        shopifyProduct.RequiredUpdateOnShopify = !tryStatusOK;
                        shopifyProduct.ShopifyProductId = getShopifyProduct.Id;

                        db.SaveChanges();
                    }
                }
                else
                {
                    if (shopifyProduct != null)
                    {
                        shopifyProduct.ExistsOnShopify = false;
                        shopifyProduct.LastUpdate = DateTime.UtcNow;
                        shopifyProduct.RequiredUpdateOnShopify = true;

                        db.SaveChanges();
                    }

                    Print.PrintStatus(item.RefName, "DoesNotHaveProductOnShopify", title, ConsoleColor.Blue);
                }

                Print.PrintStatus(item.RefName, shopifyProduct == null ? "NotUpdateOnDatabase" : "UpdateOnDatabase", title, ConsoleColor.Blue);
                Print.PrintStatus(item.RefName, "RunComplete", title, ConsoleColor.Green);
            }
        }

        public static void SaveProduct(ProductDetailModel model)
        {
            using (var db = AliShopEntities.New())
            {
                var options = model.Product.Options;
                var store = db.AliStore.First(i => i.StoreId == model.Store.StoreId);
                var imageList = model.RunParams["imageBigViewURL"]
                    .ToObject<string[]>()
                    .Concat(model.Product.SkuValues
                        .Where(i => !string.IsNullOrWhiteSpace(i.ImageUrl))
                        .Select(i => i.ImageUrl))
                    .Select(delegate (string imageUrl)
                    {
                        var imageSHA1 = new ImageSHA1();

                        imageSHA1.OriginalUrl.Add(imageUrl);
                        imageSHA1.NewUrl = imageUrl.Replace("/" + Path.GetFileName(imageUrl), ".jpg");

                        return imageSHA1;
                    });

                var imageListSHA1 = new HashSet<ImageSHA1>();

                Parallel.ForEach(imageList, delegate (ImageSHA1 currentImage)
                {
                    currentImage.SHA1 = ComputeSHA1ForWebResource(currentImage.NewUrl);

                    if (currentImage.SHA1 == null)
                    {
                        return;
                    }

                    lock (imageListSHA1)
                    {
                        var imageSHA1 = imageListSHA1.FirstOrDefault(i => i.SHA1 == currentImage.SHA1);

                        if (imageSHA1 == null)
                        {
                            imageListSHA1.Add(currentImage);
                        }
                        else
                        {
                            imageSHA1.OriginalUrl.AddRange(currentImage.OriginalUrl);
                        }
                    }
                });

                store.Feedback = model.Store.Feedback;
                store.LastUpdate = DateTime.UtcNow;
                store.Name = Program.ToTitle(model.Store.StoreName);
                store.PageConfigXml = JsonConvert.DeserializeXNode(model.PageConfig.ToString(), "Root").ToString();
                store.Score = model.Store.Rating;
                store.Since = model.Store.Since;

                db.SaveChanges();

                var product = db.AliProduct.Include("AliProductOption", "AliProductImage", "AliProductSpecific", "AliProductVariant").FirstOrDefault(i => i.ProductId == model.Product.ProductId);
                var productLink = db.AliProductLink.First(i => i.ProductId == model.Product.ProductId);

                if (product == null)
                {
                    product = new AliProduct();
                    product.Enabled = true;
                    product.Create = DateTime.UtcNow;

                    db.AliProduct.Add(product);
                }
                else
                {
                    product.LastUpdate = DateTime.UtcNow;
                }

                product.AliStoreId = store.AliStoreId;
                product.AliProductLinkId = productLink.AliProductLinkId;
                product.ProductId = model.Product.ProductId;
                product.OrdersCount = model.Product.OrderCount;
                product.ProcessingTime = model.Product.ProcessingTime;
                product.Rating = model.Product.Rating;
                product.RunParamsXml = JsonConvert.DeserializeXNode(model.RunParams.ToString(), "Root").ToString();
                product.Title = Program.ToTitle(model.Product.Title);
                product.Enabled = model.Product.NoLongerAvailable != true;

                db.SaveChanges();

                db.AliProductOption.RemoveRange(product.AliProductOption.Where(i => !options.Contains(i.Name, StringComparer.InvariantCultureIgnoreCase)));

                for (var i = 0; i < model.Product.Options.Length; i++)
                {
                    var option = model.Product.Options[i];
                    var productOption = product.AliProductOption.FirstOrDefault(item => item.Name.Equals(option, StringComparison.InvariantCultureIgnoreCase));

                    if (productOption == null)
                    {
                        product.AliProductOption.Add(new AliProductOption
                        {
                            AliProductId = product.AliProductId,
                            AliProductLinkId = productLink.AliProductLinkId,
                            AliStoreId = store.AliStoreId,
                            Create = DateTime.UtcNow,
                            Name = Program.ToTitle(option),
                            Number = i + 1,
                            ProductId = model.Product.ProductId
                        });
                    }
                    else
                    {
                        productOption.Number = i + 1;
                        productOption.LastUpdate = DateTime.UtcNow;
                    }
                }

                db.SaveChanges();

                db.AliProductImage.RemoveRange(product.AliProductImage.Where(i => !imageListSHA1.Any(s => s.SHA1 == i.SHA1)));

                foreach (var image in imageListSHA1.Where(s => !product.AliProductImage.Any(i => i.SHA1 == s.SHA1)))
                {
                    product.AliProductImage.Add(new AliProductImage
                    {
                        AliProductId = product.AliProductId,
                        AliProductLinkId = productLink.AliProductLinkId,
                        AliStoreId = store.AliStoreId,
                        Create = DateTime.UtcNow,
                        ProductId = model.Product.ProductId,
                        Enabled = true,
                        Url = image.NewUrl,
                        SHA1 = image.SHA1
                    });
                }

                db.SaveChanges();

                var productSpecs = model.Product.ProductSpecs.Select(i => new AliProductSpecific
                {
                    AliProductId = product.AliProductId,
                    AliProductLinkId = productLink.AliProductLinkId,
                    AliStoreId = store.AliStoreId,
                    Create = DateTime.UtcNow,
                    ProductId = model.Product.ProductId,
                    Name = Program.ToTitle(i.Name),
                    Value = Program.ToTitle(i.Value),
                    Type = "item-specifics"
                }).ToList();

                productSpecs.AddRange(model.Product.PackagingSpecs.Select(i => new AliProductSpecific
                {
                    AliProductId = product.AliProductId,
                    AliProductLinkId = productLink.AliProductLinkId,
                    AliStoreId = store.AliStoreId,
                    Create = DateTime.UtcNow,
                    ProductId = model.Product.ProductId,
                    Name = Program.ToTitle(i.Name),
                    Value = Program.ToTitle(i.Value),
                    Type = "packaging-details"
                }));

                db.AliProductSpecific.RemoveRange(product.AliProductSpecific.Where(i => !productSpecs.Any(s => s.Type == i.Type && s.Name.Equals(i.Name, StringComparison.InvariantCultureIgnoreCase))));

                foreach (var item in productSpecs.Where(spec => !product.AliProductSpecific.Any(i => i.Type == spec.Type && i.Name.Equals(spec.Name, StringComparison.InvariantCultureIgnoreCase))))
                {
                    product.AliProductSpecific.Add(item);
                }

                db.SaveChanges();

                foreach (var variant in product.AliProductVariant.Where(i => !model.Product.ProductSku.Select(s => s.SkuPropIds).Contains(i.SkuPropIds)))
                {
                    variant.Enabled = false;
                    variant.LastUpdate = DateTime.UtcNow;
                }

                foreach (var sku in model.Product.ProductSku)
                {
                    var skuIds = sku.SkuPropIds.Split(',');
                    var skuValues = model.Product.SkuValues.Where(i => skuIds.Contains(i.SkuId.ToString(), StringComparer.InvariantCultureIgnoreCase)).ToArray();
                    var variant = product.AliProductVariant.FirstOrDefault(i => i.SkuPropIds == sku.SkuPropIds);
                    var skuValueWithImage = skuValues.FirstOrDefault(i => i.ImageUrl != null);
                    var aliProductImageId = default(long?);

                    if (skuValueWithImage != null)
                    {
                        var imageSHA1 = imageListSHA1.FirstOrDefault(i => i.OriginalUrl.Contains(skuValueWithImage.ImageUrl, StringComparer.InvariantCultureIgnoreCase));

                        if (imageSHA1 != null)
                        {
                            aliProductImageId = product.AliProductImage.First(i => i.SHA1 == imageSHA1.SHA1).AliProductImageId;
                        }
                    }

                    if (variant == null)
                    {
                        variant = new AliProductVariant();

                        variant.Enabled = true;
                        variant.Create = DateTime.UtcNow;

                        product.AliProductVariant.Add(variant);
                    }
                    else
                    {
                        variant.LastUpdate = DateTime.UtcNow;
                    }

                    variant.AliProductId = product.AliProductId;
                    variant.AliProductImageId = aliProductImageId;
                    variant.AliProductLinkId = productLink.AliProductLinkId;
                    variant.AliStoreId = store.AliStoreId;
                    variant.AvailableQuantity = sku.SkuVal.AvailQuantity;
                    variant.DiscountPrice = sku.SkuVal.IsActivity ? sku.SkuVal.ActSkuPrice : (decimal?)null;
                    variant.DiscountTimeLeftMinutes = model.Product.DiscountTimeLeftMinutes;
                    variant.DiscountUpdateTime = variant.DiscountTimeLeftMinutes == null ? default(DateTime?) : DateTime.UtcNow;
                    variant.InventoryQuantity = sku.SkuVal.Inventory;
                    variant.Option1 = skuValues.Length > 0 ? !string.IsNullOrWhiteSpace(skuValues[0].Name) ? skuValues[0].Name : Program.ToTitle(skuValues[0].Title) : null;
                    variant.Option2 = skuValues.Length > 1 ? !string.IsNullOrWhiteSpace(skuValues[1].Name) ? skuValues[1].Name : Program.ToTitle(skuValues[1].Title) : null;
                    variant.Option3 = skuValues.Length > 2 ? !string.IsNullOrWhiteSpace(skuValues[2].Name) ? skuValues[2].Name : Program.ToTitle(skuValues[2].Title) : null;
                    variant.OriginalPrice = sku.SkuVal.SkuPrice;
                    variant.ProductId = model.Product.ProductId;
                    variant.SkuProductXml = JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(sku), "Root").ToString();
                    variant.SkuPropIds = sku.SkuPropIds;
                    variant.Weight = model.Product.Weight;
                }

                db.SaveChanges();

                var shopifyProduct = db.AliShopifyProduct.FirstOrDefault(i => i.AliProductId == product.AliProductId);

                if (shopifyProduct == null)
                {
                    shopifyProduct = new AliShopifyProduct();
                    shopifyProduct.Create = DateTime.UtcNow;
                    shopifyProduct.ExistsOnShopify = false;
                    shopifyProduct.Published = false;

                    db.AliShopifyProduct.Add(shopifyProduct);
                }
                else
                {
                    shopifyProduct.LastUpdate = DateTime.UtcNow;
                }

                shopifyProduct.AliProductId = product.AliProductId;
                shopifyProduct.RequiredUpdateOnShopify = true;

                db.SaveChanges();
            }
        }

        public static void SaveVariant(UpdateProductModel item, int? discountTimeLeftMinutes, ProductSkuModel[] skus, bool noLongerAvailable)
        {
            using (var db = AliShopEntities.New())
            {
                var product = db.AliProduct.First(i => i.AliProductId == item.AliProductId);
                var requiredUpdateOnShopify = false;

                product.Enabled = !noLongerAvailable;

                foreach (var sku in skus)
                {
                    var variant = db.AliProductVariant.First(i => i.AliProductId == item.AliProductId && i.SkuPropIds == sku.SkuPropIds);
                    var discountPrice = sku.SkuVal.IsActivity ? sku.SkuVal.ActSkuPrice : (decimal?)null;

                    if (variant.AvailableQuantity != sku.SkuVal.AvailQuantity ||
                        variant.DiscountPrice != discountPrice ||
                        variant.OriginalPrice != sku.SkuVal.SkuPrice)
                    {
                        requiredUpdateOnShopify = true;
                    }

                    if (variant.DiscountTimeLeftMinutes != discountTimeLeftMinutes)
                    {
                        if (variant.DiscountTimeLeftMinutes != null && discountTimeLeftMinutes != null)
                        {
                            var elapsedTime = DateTime.UtcNow - variant.DiscountUpdateTime.Value;
                            var timeLeft = variant.DiscountTimeLeftMinutes.Value - elapsedTime.TotalMinutes;
                            var diff = timeLeft - discountTimeLeftMinutes.Value;

                            if (diff > 1 || diff < -1)
                            {
                                requiredUpdateOnShopify = true;
                            }
                        }
                        else
                        {
                            requiredUpdateOnShopify = true;
                        }
                    }

                    if (requiredUpdateOnShopify)
                    {
                        var copy = new AliProductVariantCopy();

                        copy.AliProductId = variant.AliProductId;
                        copy.AliProductImageId = variant.AliProductImageId;
                        copy.AliProductLinkId = variant.AliProductLinkId;
                        copy.AliProductVariantId = variant.AliProductVariantId;
                        copy.AliStoreId = variant.AliStoreId;
                        copy.AvailableQuantity = variant.AvailableQuantity;
                        copy.Create = variant.Create;
                        copy.DiscountPrice = variant.DiscountPrice;
                        copy.DiscountTimeLeftMinutes = variant.DiscountTimeLeftMinutes;
                        copy.DiscountUpdateTime = variant.DiscountUpdateTime;
                        copy.Enabled = variant.Enabled;
                        copy.InventoryQuantity = variant.InventoryQuantity;
                        copy.LastUpdate = variant.LastUpdate;
                        copy.Option1 = variant.Option1;
                        copy.Option2 = variant.Option2;
                        copy.Option3 = variant.Option3;
                        copy.OriginalPrice = variant.OriginalPrice;
                        copy.ProductId = variant.ProductId;
                        copy.SkuProductXml = variant.SkuProductXml;
                        copy.SkuPropIds = variant.SkuPropIds;
                        copy.Weight = variant.Weight;

                        db.AliProductVariantCopy.Add(copy);
                    }

                    variant.AvailableQuantity = sku.SkuVal.AvailQuantity;
                    variant.DiscountPrice = discountPrice;
                    variant.DiscountTimeLeftMinutes = discountTimeLeftMinutes;
                    variant.DiscountUpdateTime = variant.DiscountTimeLeftMinutes == null ? default(DateTime?) : DateTime.UtcNow;
                    variant.InventoryQuantity = sku.SkuVal.Inventory;
                    variant.OriginalPrice = sku.SkuVal.SkuPrice;
                    variant.SkuProductXml = JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(sku), "Root").ToString();
                    variant.LastUpdate = DateTime.UtcNow;
                }

                if (requiredUpdateOnShopify)
                {
                    var shopifyProduct = db.AliShopifyProduct.First(i => i.AliProductId == item.AliProductId);

                    shopifyProduct.RequiredUpdateOnShopify = true;
                    shopifyProduct.LastUpdate = DateTime.UtcNow;
                }

                db.SaveChanges();
            }
        }

        public static string ComputeSHA1ForWebResource(string resourceUrl)
        {
            using (var webClient = new WebClient())
            {
                var data = default(byte[]);
                var tryStatusOK = Program.Try("SHA1", resourceUrl, 3, () => data = webClient.DownloadData(resourceUrl));

                if (!tryStatusOK)
                {
                    return null;
                }

                var sha1 = new SHA1Managed();
                var hash = sha1.ComputeHash(data);
                var hashBuilder = new StringBuilder();

                foreach (var h in hash)
                {
                    hashBuilder.Append(h.ToString("x2"));
                }

                return hashBuilder.ToString();
            }
        }
    }
}
