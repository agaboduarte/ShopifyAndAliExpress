using ISAA.Rules.StockAutomation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISAA.Rules.StockAutomation
{
    public class QueueSummary
    {
        public Ali_Product Product { get; set; }

        public Ali_ProductSku[] SKUs { get; set; }

        public Ali_ProductStock[] Stock { get; set; }

        public IEnumerable<Ali_ProductSkuImage> Images { get; set; }

        public Ali_ShopifyProduct ShopifyProduct { get; set; }

        public Ali_ShopifyQueue Queue { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal? CompareAtPrice { get; set; }

        public string ListShopifyProductBy { get; set; }

        public string NewShopifyHandleFriendly { get; set; }

        public long ShopifyRefID { get; set; }

        public bool OnSale { get; set; }

        public bool NotPublish { get; set; }
    }
}
