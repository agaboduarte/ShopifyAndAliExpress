using ISAA.Rules.StockAutomation.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ISAA.Rules.StockAutomation
{
    public class StockAutomationRules : RulesCreator
    {
        public IQueryable<Ali_Product> GetProductByReferenceID(long referenceID, params string[] includes)
        {
            return Entity.Ali_Product
                .Include(includes)
                .Where(i => i.ReferenceID == referenceID);
        }

        public IQueryable<Ali_ProductSku> GetProductSkuByReferenceIDAndFeatureCode(long referenceID, int featureCode, params string[] includes)
        {
            return Entity.Ali_ProductSku
                .Include(includes)
                .Where(i => i.ReferenceID == referenceID && i.FeatureCode == featureCode);
        }

        public IQueryable<Ali_ShopifyQueue> GetShopifyQueue(params string[] includes)
        {
            return Entity.Ali_ShopifyQueue
                .Include(includes)
                .Where(i =>
                    i.Complete == null &&
                    i.Ignore == false);
        }

        public IQueryable<Ali_ShopifyQueue> GetShopifyQueueByID(long[] queueIDs, params string[] includes)
        {
            return Entity.Ali_ShopifyQueue
                .Include(includes)
                .Where(i => queueIDs.Contains(i.Ali_ShopifyQueueID));
        }

        public IQueryable<Ali_ProductStock> GetProductStockByProcessIDAndProductID(long processID, long productID, params string[] includes)
        {
            return Entity.Ali_ProductStock
                .Include(includes)
                .Where(i => i.Ali_ProcessID == processID && i.Ali_ProductID == productID);
        }

        public void UpdateQueue(long[] queueIDs)
        {
            var cmd = "UPDATE Ali_ShopifyQueue SET Queue = GETUTCDATE() WHERE Ali_ShopifyQueueID IN ({0})";

            cmd = string.Format(cmd, string.Join(",", queueIDs));

            Entity.Database.ExecuteSqlCommand(cmd);
        }

        public void UpdateQueueComplete(long[] queueIDs, bool completeWithError)
        {
            var cmd = "UPDATE Ali_ShopifyQueue SET Complete = GETUTCDATE(), CompleteWithError = @completeWithError WHERE Ali_ShopifyQueueID IN ({0});";

            cmd = string.Format(cmd, string.Join(",", queueIDs));

            Entity.Database.ExecuteSqlCommand(cmd, new SqlParameter("@completeWithError", completeWithError));
        }

        public void UpdateToIgnorePreviousQueue(long queueID)
        {
            var cmd = @"
UPDATE OTHERS_Q 
SET OTHERS_Q.Ignore = 1, OTHERS_Q.[Update] = GETUTCDATE() 
FROM Ali_ShopifyQueue Q 
JOIN Ali_Product P ON P.Ali_ProductID = Q.Ali_ProductID
JOIN Ali_Product OTHERS_P ON OTHERS_P.ReferenceID = P.ReferenceID
JOIN Ali_ShopifyQueue OTHERS_Q ON OTHERS_Q.Ali_ProductID = OTHERS_P.Ali_ProductID
WHERE Q.Ali_ShopifyQueueID = @queueID
AND OTHERS_Q.Ali_ShopifyQueueID < @queueID
AND OTHERS_Q.Complete IS NULL
AND OTHERS_Q.Ignore = 0";

            Entity.Database.ExecuteSqlCommand(cmd, new SqlParameter("@queueID", queueID));
        }

        public IQueryable<Ali_ProductSku> GetProductSkuByProcessID(long processID, params string[] includes)
        {
            return Entity.Ali_ProductSku
                .Include(includes)
                .Where(i => i.Ali_ProcessID == processID);
        }

        public IQueryable<Ali_Product> GetProductByProcessID(long processID, params string[] includes)
        {
            return Entity.Ali_Product
                .Include(includes)
                .Where(i => i.Ali_ProcessID == processID);
        }

        public IQueryable<Ali_ProductSku> GetProductSkuByProductID(long productID, params string[] includes)
        {
            return Entity.Ali_ProductSku
                .Include(includes)
                .Where(i => i.Ali_ProductID == productID);
        }

        public IQueryable<Ali_ProductSkuImage> GetProductSkuImageByProductID(long productID, params string[] includes)
        {
            return Entity.Ali_ProductSkuImage
                .Include(includes)
                .Where(i => i.Ali_ProductID == productID);
        }

        public long GetProcessSuccessOrderByDesc(string type, params string[] includes)
        {
            return Entity.Ali_Process
                .Include(includes)
                .Where(i => i.EndDate != null && i.Type == type)
                .OrderByDescending(i => i.Ali_ProcessID)
                .First()
                .Ali_ProcessID;
        }

        public long CreateProcess(string type, bool keepAlive = false)
        {
            var now = DateTime.UtcNow;

            var process = new Ali_Process
            {
                Type = type,
                StartDate = now,
                KeepAlive = keepAlive
            };

            Entity.Ali_Process.Add(process);

            Entity.SaveChanges();

            return process.Ali_ProcessID;
        }

        public Ali_ShopifyProduct GetShopifyProduct(string shopifyHandleFriendly, long? shopifyProductID, long? referenceID, long? shopifyRefID)
        {
            var model = default(Ali_ShopifyProduct);

            if (shopifyProductID != null)
            {
                Entity.Ali_ShopifyProduct.FirstOrDefault(i => i.ShopifyProductID == shopifyProductID.Value);
            }

            if (model == null && referenceID != null)
            {
                model = Entity.Ali_ShopifyProduct.FirstOrDefault(i => i.ProductRefID == referenceID.Value);
            }

            if (model == null && shopifyRefID != null)
            {
                model = Entity.Ali_ShopifyProduct.FirstOrDefault(i => i.ShopifyProductRefID == shopifyRefID.Value);
            }

            if (model == null && shopifyHandleFriendly != null)
            {
                model = Entity.Ali_ShopifyProduct.FirstOrDefault(i => i.HandleFriendlyName == shopifyHandleFriendly);
            }

            return model;
        }

        public Ali_ShopifyProduct CreateOrUpdateShopifyProduct(long shopifyProductID, string shopifyHandleFriendly, decimal price, decimal? compareAtPrice, long? referenceID, long? shopifyRefID, bool published, bool existsOnShopify)
        {
            var model = GetShopifyProduct(shopifyHandleFriendly, shopifyProductID, referenceID, shopifyRefID);

            if (model == null)
            {
                model = new Ali_ShopifyProduct
                {
                    Create = DateTime.UtcNow
                };

                Entity.Ali_ShopifyProduct.Add(model);
            }
            else
            {
                model.LastUpdate = DateTime.UtcNow;
            }

            model.HandleFriendlyName = shopifyHandleFriendly;
            model.ShopifyProductID = shopifyProductID;
            model.Published = published;
            model.Price = price;
            model.CompareAtPrice = compareAtPrice;
            model.ExistsOnShopify = existsOnShopify;

            if (referenceID != null)
            {
                model.ProductRefID = referenceID;
            }

            if (shopifyRefID != null)
            {
                model.ShopifyProductRefID = shopifyRefID;
            }

            Entity.SaveChanges();

            return model;
        }

        public IQueryable<Ali_ShopCart> GetShopCartByID(long shopCartID, params string[] includes)
        {
            return Entity.Ali_ShopCart
               .Include(includes)
               .Where(i => i.Ali_ShopCartID == shopCartID);
        }

        public Ali_ShopCart GetLastShopCart(long productReferenceID, params string[] includes)
        {
            return Entity.Ali_ShopCart
                .Include(includes)
                .Where(i => i.ProductReferenceID == productReferenceID)
                .OrderByDescending(i => i.Ali_ShopCartID)
                .FirstOrDefault();
        }

        public Ali_ShopCart CreateShopCart(long productReferenceID, string shopCartID)
        {
            var model = new Ali_ShopCart
            {
                ProductReferenceID = productReferenceID,
                ShopCartID = shopCartID,
                Create = DateTime.UtcNow
            };

            Entity.Ali_ShopCart.Add(model);

            Entity.SaveChanges();

            return model;
        }

        public Ali_Process UpdateProcessByID(long processID)
        {
            var now = DateTime.UtcNow;

            var process = Entity.Ali_Process.Single(i => i.Ali_ProcessID == processID);

            process.EndDate = now;

            Entity.SaveChanges();

            return process;
        }

        public IQueryable<Ali_ProductSku> GetProductSkuByID(long productSkuID, params string[] includes)
        {
            return Entity.Ali_ProductSku
                   .Include(includes)
                   .Where(i => i.Ali_ProductSkuID == productSkuID);
        }

        public IQueryable<Ali_Product> GetProductByID(long productID, params string[] includes)
        {
            return Entity.Ali_Product
                   .Include(includes)
                   .Where(i => i.Ali_ProductID == productID);
        }

        public bool SaveProductStock(long productRefID, IEnumerable<Ali_ProductStock> productStock)
        {
            var lastQueue = GetLastShopifyQueue(productRefID).FirstOrDefault();

            if (lastQueue != null)
            {
                var lastStock = GetProductStockByProcessIDAndProductID(lastQueue.Ali_ProcessID, lastQueue.Ali_ProductID).ToArray();
                var currentStock = productStock.ToArray();

                var required = (
                    from last in lastStock
                    join current in currentStock on last.FeatureCode equals current.FeatureCode
                    where last.Quantity != current.Quantity
                    select 1
                ).Any();

                if (!required && lastStock.Length == currentStock.Length)
                {
                    return false;
                }
            }

            Entity.Ali_ProductStock.AddRange(productStock.Where(i => i != null));

            Entity.SaveChanges();

            return true;
        }

        public bool SaveShopifyQueue(long stockProcessID, long productID, long referenceID, bool priority = false)
        {
            if (!priority)
            {
                var lastQueue = GetLastShopifyQueue(referenceID).FirstOrDefault();

                if (lastQueue != null)
                {
                    var currentStock = GetProductStockByProcessIDAndProductID(stockProcessID, productID).ToArray();
                    var lastStock = GetProductStockByProcessIDAndProductID(lastQueue.Ali_ProcessID, lastQueue.Ali_ProductID).ToArray();

                    var required = (
                        from last in lastStock
                        join current in currentStock on last.FeatureCode equals current.FeatureCode
                        where last.Quantity != current.Quantity
                        select 1
                    ).Any();

                    if (!required && lastStock.Length == currentStock.Length)
                    {
                        return false;
                    }
                }
            }

            var queueModel = new Ali_ShopifyQueue
            {
                Complete = null,
                Queue = null,
                Ignore = false,
                Create = DateTime.UtcNow,
                Update = null,
                Ali_ProcessID = stockProcessID,
                Ali_ProductID = productID
            };

            Entity.Ali_ShopifyQueue.Add(queueModel);

            Entity.SaveChanges();

            UpdateToIgnorePreviousQueue(queueModel.Ali_ShopifyQueueID);

            return true;
        }

        public IQueryable<Ali_ShopifyQueue> GetLastShopifyQueue(long productRefID, params string[] includes)
        {
            return Entity.Ali_ShopifyQueue
                .Include(includes)
                .Where(i => i.Ali_Product.ReferenceID == productRefID)
                .OrderByDescending(i => i.Ali_ShopifyQueueID);
        }

        public IQueryable<Ali_CalculatePrice> GetCalculatePriceByReferenceID(long? productRefID, params string[] includes)
        {
            return Entity.Ali_CalculatePrice
              .Include(includes)
              .Where(i => i.ProductRefID == productRefID);
        }

        public IQueryable<Ali_NotPublish> GetNotPublishByReferenceID(long? productRefID, params string[] includes)
        {
            return Entity.Ali_NotPublish
                .Include(includes)
                .Where(i =>
                    i.ProductRefID == productRefID &&
                    i.Enabled == true);
        }

        public QueueSummary GetLastQueueSummary(long productRefID)
        {
            var queue = GetLastShopifyQueue(productRefID).First();
            var product = GetProductByID(queue.Ali_ProductID).First();

            if (product.Title.ToLowerInvariant().EndsWith(" " + product.Branch.ToLowerInvariant()))
            {
                product.Title = product.Title.Substring(0, product.Title.Length - product.Branch.Length - 1);
            }

            var skus = GetProductSkuByProductID(product.Ali_ProductID).ToArray();
            var imageQuery = GetProductSkuImageByProductID(product.Ali_ProductID);
            var productImages = imageQuery.Where(i => i.Name == "FRONT").ToArray();
            var backImage = imageQuery.FirstOrDefault(i => i.Name == "BACK");
            var notPublish = GetNotPublishByReferenceID(product.ReferenceID).Any();
            var stocks = GetProductStockByProcessIDAndProductID(queue.Ali_ProcessID, queue.Ali_ProductID).ToArray();
            var shopifyRefID = ShopifyProductRefID(product.ReferenceID);
            var newShopifyHandleFriendly = ShopifyProductHandleFriendly(shopifyRefID, product.Title);
            var shopifyProduct = GetShopifyProduct(newShopifyHandleFriendly, null, product.ReferenceID, shopifyRefID);
            var listShopifyProductBy = shopifyProduct == null ? newShopifyHandleFriendly : shopifyProduct.HandleFriendlyName;
            var calculatePrice = GetCalculatePriceByReferenceID(product.ReferenceID).FirstOrDefault() ?? GetCalculatePriceByReferenceID(null).First();
            var onSale = calculatePrice.CompareAtPriceFactor != null;
            var productPrice = CalculatePriceFactor(product.Price * calculatePrice.PriceFactor, onSale);
            var compareAtPrice = default(decimal?);

            if (onSale)
            {
                compareAtPrice = CalculatePriceFactor(product.Price * calculatePrice.CompareAtPriceFactor.Value, false);
            }

            if (backImage != null)
            {
                productImages = productImages.Concat(new[] { backImage }).ToArray();
            }

            return new QueueSummary
            {
                Queue = queue,
                Product = product,
                SKUs = skus,
                Stock = stocks,
                Images = productImages,
                ShopifyProduct = shopifyProduct,
                NotPublish = notPublish,
                ShopifyRefID = shopifyRefID,
                NewShopifyHandleFriendly = newShopifyHandleFriendly,
                ListShopifyProductBy = listShopifyProductBy,
                ProductPrice = productPrice,
                CompareAtPrice = compareAtPrice,
                OnSale = onSale
            };
        }

        public static string ShopifyProductHandleFriendly(long shopifyRefID, string title)
        {
            var segment = title + "-" + shopifyRefID;
            var bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(segment);
            var handleFriendly = Regex.Replace(Encoding.UTF8.GetString(bytes), @"[^\w\d]", "-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return handleFriendly.ToLowerInvariant();
        }

        public static long ShopifyProductRefID(long referenceID)
        {
            return long.Parse(string.Concat("100", referenceID.ToString().PadLeft(6, '0')));
        }

        public static long ShopifyProductSkuRefID(long referenceID, long featureCode)
        {
            return long.Parse(string.Concat(ShopifyProductRefID(referenceID), featureCode.ToString().PadLeft(6, '0')));
        }

        public static long ImageRefID(string imageUrl)
        {
            return long.Parse(string.Concat("100", HashID(imageUrl, 999999).ToString().PadLeft(6, '0')));
        }

        public static long HashID(string stringValue, int maxValue)
        {
            int intLength = stringValue.Length / 4;
            long sum = 0;
            char[] c;
            long mult;

            for (int j = 0; j < intLength; j++)
            {
                c = stringValue.Substring(j * 4, 4).ToCharArray();
                mult = 1;

                for (int k = 0; k < c.Length; k++)
                {
                    sum += c[k] * mult;
                    mult *= 256;
                }
            }

            c = stringValue.Substring(intLength * 4).ToCharArray();
            mult = 1;

            for (int k = 0; k < c.Length; k++)
            {
                sum += c[k] * mult;
                mult *= 256;
            }

            return Math.Abs(sum) % maxValue;
        }

        public static decimal CalculatePriceFactor(decimal price, bool discount)
        {
            var originalPrice = price;
            var priceRounded = Math.Round(price, MidpointRounding.AwayFromZero);
            var returnPrice = default(decimal);
            var diff = originalPrice - priceRounded;

            if (diff > 0)
            {
                returnPrice = priceRounded + .9M;

                if (discount)
                {
                    returnPrice += .09M;
                }
            }
            else if (discount)
            {
                returnPrice = priceRounded - .01M;
            }
            else
            {
                returnPrice = priceRounded - .1M;
            }

            return returnPrice;
        }

        public static int CalculateStock(int internalQuantity, int soldQuantity)
        {
            var stockQuantity = internalQuantity - soldQuantity - 3; // 3 is the safety margin

            if (stockQuantity < 0)
            {
                stockQuantity = 0;
            }

            return stockQuantity;
        }
    }
}
