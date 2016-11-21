using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Suppliers.Ali.Automation
{
    public static class AppSettings
    {
        public static string ShopifyApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["Shopify:ApiKey"];
            }
        }

        public static string ShopifyMyShopifyUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["Shopify:MyShopifyUrl"];
            }
        }

        public static int? GetProductMaxDegreeOfParallelism
        {
            get
            {
                if (ConfigurationManager.AppSettings["GetProduct:MaxDegreeOfParallelism"] != null)
                {
                    return int.Parse(ConfigurationManager.AppSettings["GetProduct:MaxDegreeOfParallelism"]);
                }

                return null;
            }
        }

        public static int? UpdateProductMaxDegreeOfParallelism
        {
            get
            {
                if (ConfigurationManager.AppSettings["UpdateProduct:MaxDegreeOfParallelism"] != null)
                {
                    return int.Parse(ConfigurationManager.AppSettings["UpdateProduct:MaxDegreeOfParallelism"]);
                }

                return null;
            }
        }

        public static int? UpdateShopifyProductMaxDegreeOfParallelism
        {
            get
            {
                if (ConfigurationManager.AppSettings["UpdateShopifyProduct:MaxDegreeOfParallelism"] != null)
                {
                    return int.Parse(ConfigurationManager.AppSettings["UpdateShopifyProduct:MaxDegreeOfParallelism"]);
                }

                return null;
            }
        }
    }
}
