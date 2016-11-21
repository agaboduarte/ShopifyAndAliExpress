using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using ISAA.Suppliers.Ali.Automation.Common;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation
{
    public static class AliUpdateShopifyProduct
    {
        public static void Run(int? maxDegreeOfParallelism)
        {
            var parallelOptions = new ParallelOptions();

            parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism ?? AppSettings.UpdateShopifyProductMaxDegreeOfParallelism ?? -1;

            var items = default(UpdateShopifyProductModel[]);

            using (var db = AliShopEntities.New())
            {
                var rules = RulesCreator.NewRules<AliShopifyProductRules>(db);
                var shopifyProducts = rules
                    .GetAll()
                    .Where(i => i.RequiredUpdateOnShopify || i.AliProduct.Enabled != i.Published)
                    .ToArray();

                items = shopifyProducts.Select(i => new UpdateShopifyProductModel
                {
                    AliShopifyProductId = i.AliShopifyProductId,
                    RefName = i.AliShopifyProductId.ToString(),
                    RowVersion = i.RowVersion
                }).ToArray();
            }

            var partitioner = Partitioner.Create(items);

            Parallel.ForEach(partitioner, parallelOptions, AliExpress.UpdateShopifyProduct);
        }
    }
}