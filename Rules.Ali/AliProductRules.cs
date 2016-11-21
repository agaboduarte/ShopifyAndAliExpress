using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali
{
    public class AliProductRules : RulesCreator
    {
        public IQueryable<AliProduct> GetAll(params string[] includes)
        {
            return Entity.AliProduct
                .Include(includes);
        }

        public IQueryable<AliProduct> GetById(long aliProductId, params string[] includes)
        {
            return Entity.AliProduct
                 .Include(includes)
                 .Where(i => i.AliProductId == aliProductId);
        }
    }
}
