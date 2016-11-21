using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali
{
    public class AliParameterRules : RulesCreator
    {
        public AliParameter GetByName(string name, params string[] includes)
        {
            return Entity.AliParameter
                .Include(includes)
                .Where(i => i.Name == name)
                .FirstOrDefault();
        }
    }
}
