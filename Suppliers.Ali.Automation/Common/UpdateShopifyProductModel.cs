using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation.Common
{
    public class UpdateShopifyProductModel
    {
        public long AliShopifyProductId { get; set; }

        public byte[] RowVersion { get; set; }

        public string RefName { get; set; }
    }
}
