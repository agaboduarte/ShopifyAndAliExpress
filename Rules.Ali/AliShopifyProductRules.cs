using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali
{
    public class AliShopifyProductRules : RulesCreator
    {
        public IQueryable<AliShopifyProduct> GetAll(params string[] includes)
        {
            return Entity.AliShopifyProduct
                .Include(includes);
        }

        public IQueryable<AliShopifyProduct> GetById(long id, params string[] includes)
        {
            return Entity.AliShopifyProduct
                .Include(includes)
                .Where(i => i.AliShopifyProductId == id);
        }

        public void UpdateShopifyProduct(long id, string handle, decimal avgPrice, decimal? avgCompareAtPrice, bool published, bool existOnshopify)
        {
            var shopifyProduct = GetById(id).First();
        }
    }
}
