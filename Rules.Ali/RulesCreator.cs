using ISAA.Rules.Ali.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali
{
    public class RulesCreator
    {
        public AliShopEntities Entity { get; set; }

        public T NewRules<T>()
            where T : RulesCreator, new()
        {
            return NewRules<T>(Entity);
        }

        public static T NewRules<T>(AliShopEntities db)
            where T : RulesCreator, new()
        {
            return new T
            {
                Entity = db
            };
        }
    }
}
