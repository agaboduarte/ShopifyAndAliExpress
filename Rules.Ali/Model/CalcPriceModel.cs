using ISAA.Rules.Ali.Model;
using System;

namespace ISAA.Rules.Ali.Model
{
    public class CalcPriceModel
    {
        public long AliProductVariantId { get; set; }

        public AliShopifyPrice AliShopifyPrice { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal? DiscountPrice { get; set; }

        public bool UseDiscount { get; set; }

        public bool FixedPrice { get; set; }

        public decimal CalcPrice
        {
            get
            {
                if (FixedPrice)
                {
                    return AliShopifyPrice.FixedPrice.Value;
                }

                if (UseDiscount)
                {
                    return DisplayPrice(DiscountPrice.Value * AliShopifyPrice.Factor + AliShopifyPrice.IncrementTax.Value, false);
                }

                return DisplayPrice(OriginalPrice * AliShopifyPrice.Factor + AliShopifyPrice.IncrementTax.Value, false);
            }
        }

        public decimal? CalcCompareAtPrice
        {
            get
            {
                if (!UseDiscount || DiscountPrice == null || FixedPrice)
                {
                    return null;
                }

                return DisplayPrice(OriginalPrice * AliShopifyPrice.Factor + AliShopifyPrice.IncrementTax.Value, true);
            }
        }

        public static decimal DisplayPrice(decimal value, bool isDiscount)
        {
            var originalPrice = value;
            var priceRounded = Math.Round(value, MidpointRounding.AwayFromZero);
            var returnPrice = default(decimal);
            var diff = originalPrice - priceRounded;

            if (diff > 0)
            {
                returnPrice = priceRounded + .9M;

                if (isDiscount)
                {
                    returnPrice += .09M;
                }
            }
            else if (isDiscount)
            {
                returnPrice = priceRounded - .01M;
            }
            else
            {
                returnPrice = priceRounded - .1M;
            }

            return returnPrice;
        }
    }
}