using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using ISAA.Suppliers.Ali.Automation.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation
{
    public static class AliUpdateProduct
    {
        public static void Run(int? maxDegreeOfParallelism)
        {
            var parallelOptions = new ParallelOptions();

            parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism ?? AppSettings.UpdateProductMaxDegreeOfParallelism ?? -1;

            var items = default(UpdateProductModel[]);

            using (var db = AliShopEntities.New())
            {
                var parameterRules = RulesCreator.NewRules<AliParameterRules>(db);
                var productRules = RulesCreator.NewRules<AliProductRules>(db);

                var productUrl = parameterRules.GetByName("product_url").Value;
                var courtDate = DateTime.UtcNow.AddMinutes(-1);
                var allProducts = productRules
                    .GetAll("AliStore")
                    .Where(i => 
                        i.Enabled &&
                        i.AliProductVariant.Any(v => v.Enabled && (v.LastUpdate == null && v.Create <= courtDate || v.LastUpdate <= courtDate)))
                    .ToArray();

                items = allProducts.Select(i => new UpdateProductModel
                {
                    AliProductId = i.AliProductId,
                    RefName = i.AliProductId.ToString(),
                    Url = productUrl
                        .Replace("{store_id}", i.AliStore.StoreId.ToString())
                        .Replace("{product_id}", i.ProductId.ToString())
                }).ToArray();
            }

            var partitioner = Partitioner.Create(items, true);

            Parallel.ForEach(partitioner, parallelOptions, AliExpress.UpdateProduct);
        }
    }
}
