using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali
{
    public class AliProductLinkRules : RulesCreator
    {
        public IQueryable<AliProductLink> GetAll(params string[] includes)
        {
            return Entity.AliProductLink
                .Include(includes);
        }
    }
}
