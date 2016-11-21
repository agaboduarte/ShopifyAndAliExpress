using ISAA.Rules.Ali;
using ISAA.Rules.Ali.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISAA.Rules.Ali
{
    public class AliShopifyPriceRules : RulesCreator
    {
        public IQueryable<AliShopifyPrice> GetAll(params string[] includes)
        {
            return Entity.AliShopifyPrice
                .Include(includes);
        }

        public CalcPriceModel[] GetCalculationPricesByAliProductId(long aliProductId)
        {
            var productRules = NewRules<AliProductRules>();
            var parameterRules = NewRules<AliParameterRules>();

            var product = productRules.GetById(aliProductId, "AliProductVariant").First();
            var useDiscountPriceMinTimeLeft = parameterRules.GetByName("use_discount_min_time_left");
            var byProductId = GetByAliProductId(aliProductId).Where(i => i.Enabled).FirstOrDefault();

            if (byProductId == null)
            {
                var byDefault = GetByDefault().Where(i => i.Enabled).First();
                var returnValues = new HashSet<CalcPriceModel>();
                var shopifyPrice = default(AliShopifyPrice);

                foreach (var variant in product.AliProductVariant)
                {
                    var useDiscountPrice = UseDiscountPrice(useDiscountPriceMinTimeLeft, variant);

                    shopifyPrice = GetByAliProductVariantId(variant.AliProductVariantId).Where(i => i.Enabled).FirstOrDefault();

                    if (shopifyPrice == null)
                    {
                        shopifyPrice = GetByMinAndMaxPrice(variant.OriginalPrice.Value).Where(p => p.Enabled).FirstOrDefault();
                    }

                    returnValues.Add(new CalcPriceModel
                    {
                        AliProductVariantId = variant.AliProductVariantId,
                        AliShopifyPrice = shopifyPrice ?? byDefault,
                        UseDiscount = useDiscountPrice,
                        OriginalPrice = variant.OriginalPrice.Value,
                        DiscountPrice = variant.DiscountPrice
                    });
                }

                return returnValues.ToArray();
            }

            return product.AliProductVariant.Select(variant => new CalcPriceModel
            {
                AliProductVariantId = variant.AliProductVariantId,
                AliShopifyPrice = byProductId,
                UseDiscount = UseDiscountPrice(useDiscountPriceMinTimeLeft, variant),
                OriginalPrice = variant.OriginalPrice.Value,
                DiscountPrice = variant.DiscountPrice
            }).ToArray();
        }

        public CalcPriceModel[] GetFixedCalculationPrices(long aliProductId)
        {
            var productRules = NewRules<AliProductRules>();
            var parameterRules = NewRules<AliParameterRules>();

            var product = productRules.GetById(aliProductId, "AliProductVariant").First();
            var useDiscountPriceMinTimeLeft = parameterRules.GetByName("use_discount_min_time_left");
            var byDefault = GetByDefault().Where(i => i.Enabled).First();

            return product.AliProductVariant.Select(variant => new CalcPriceModel
            {
                FixedPrice = true,
                AliProductVariantId = variant.AliProductVariantId,
                AliShopifyPrice = byDefault,
                UseDiscount = UseDiscountPrice(useDiscountPriceMinTimeLeft, variant),
                OriginalPrice = variant.OriginalPrice.Value,
                DiscountPrice = variant.DiscountPrice
            }).ToArray();
        }

        private bool UseDiscountPrice(AliParameter useDiscountPriceMinTimeLeft, AliProductVariant variant)
        {
            var useDiscountPrice = false;

            if (variant.DiscountUpdateTime != null)
            {
                var elapsedTime = (DateTime.UtcNow - variant.DiscountUpdateTime.Value).TotalMinutes;
                var timeLeft = variant.DiscountTimeLeftMinutes - elapsedTime;

                useDiscountPrice = timeLeft > int.Parse(useDiscountPriceMinTimeLeft.Value);
            }

            return useDiscountPrice;
        }

        public IQueryable<AliShopifyPrice> GetByAliProductId(long aliProductId, params string[] includes)
        {
            return Entity.AliShopifyPrice
                .Include(includes)
                .Where(i => i.AliProductId == aliProductId);
        }

        public IQueryable<AliShopifyPrice> GetByAliProductVariantId(long aliProductVariantId, params string[] includes)
        {
            return Entity.AliShopifyPrice
                .Include(includes)
                .Where(i => i.AliProductVariantId == aliProductVariantId);
        }

        public IQueryable<AliShopifyPrice> GetByMinAndMaxPrice(decimal price, params string[] includes)
        {
            return Entity.AliShopifyPrice
                .Include(includes)
                .Where(i =>
                    price >= i.MinPrice &&
                    price <= i.MaxPrice ||
                    i.MinPrice == null && price <= i.MaxPrice ||
                    i.MaxPrice == null && price >= i.MinPrice
                );
        }

        public IQueryable<AliShopifyPrice> GetByDefault(params string[] includes)
        {
            return Entity.AliShopifyPrice
                .Include(includes)
                .Where(i => i.AliProductId == null && i.AliProductVariantId == null && i.MinPrice == null && i.MaxPrice == null);
        }
    }
}
