using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali.Model
{
    public partial class AliShopEntities
    {
        public AliShopEntities(string connectionStringName)
              : base(string.Format("name={0}", connectionStringName))
        {
        }


        public static AliShopEntities New()
        {
            var db = new AliShopEntities("alishop");

            db.Database.CommandTimeout = 0;
            db.Configuration.LazyLoadingEnabled = false;

            return db;
        }
    }
}
