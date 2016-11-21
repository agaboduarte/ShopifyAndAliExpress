using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using ISAA.Suppliers.Ali.Automation.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation
{
    public static class AliGetProduct
    {
        public static void Run(int? maxDegreeOfParallelism)
        {
            var parallelOptions = new ParallelOptions();

            parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism ?? AppSettings.GetProductMaxDegreeOfParallelism ?? -1;

            var items = default(GetProductModel[]);

            using (var db = AliShopEntities.New())
            {
                var parameterRules = RulesCreator.NewRules<AliParameterRules>(db);
                var productLinkRules = RulesCreator.NewRules<AliProductLinkRules>(db);

                var productUrl = parameterRules.GetByName("product_url").Value;
                var courtDate = DateTime.UtcNow.AddDays(-15);
                var allProductLinks = productLinkRules
                    .GetAll("AliStore")
                    .Where(i =>
                        !i.AliProduct.Any() ||
                        i.AliProduct.Any(p => p.Enabled && (p.LastUpdate == null && p.Create <= courtDate || p.LastUpdate <= courtDate)))
                    .ToArray();

                items = allProductLinks.Select(i => new GetProductModel
                {
                    ProductId = i.ProductId,
                    RefName = i.ProductId.ToString(),
                    Url = productUrl
                        .Replace("{store_id}", i.AliStore.StoreId.ToString())
                        .Replace("{product_id}", i.ProductId.ToString())
                }).ToArray();
            }

            var partitioner = Partitioner.Create(items, true);

            Parallel.ForEach(partitioner, parallelOptions, AliExpress.GetProduct);
        }

        public static ProductDetailModel Run(int storeId, long productId)
        {
            var parallelOptions = new ParallelOptions();

            parallelOptions.MaxDegreeOfParallelism = AppSettings.GetProductMaxDegreeOfParallelism.Value;

            using (var db = AliShopEntities.New())
            {
                var parameterRules = RulesCreator.NewRules<AliParameterRules>(db);
                var productLinkRules = RulesCreator.NewRules<AliProductLinkRules>(db);

                var productUrl = parameterRules.GetByName("product_url").Value;

                var model = new GetProductModel
                {
                    ProductId = productId,
                    RefName = productId.ToString(),
                    Url = productUrl
                        .Replace("{store_id}", storeId.ToString())
                        .Replace("{product_id}", productId.ToString())
                };

                return AliExpress.GetProductJSON(model);
            }
        }
    }
}
