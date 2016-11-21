namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliShopifyPrice")]
    public partial class AliShopifyPrice
    {
        [Key]
        [Column(Order = 0)]
        public long AliShopifyPriceId { get; set; }

        public long? AliProductId { get; set; }

        public long? AliProductVariantId { get; set; }

        [Column(TypeName = "money")]
        public decimal? MinPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? MaxPrice { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Factor { get; set; }

        public decimal? IncrementTax { get; set; }

        [Column(TypeName = "money")]
        public decimal? FixedPrice { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Enabled { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        public virtual AliProduct AliProduct { get; set; }

        public virtual AliProductVariant AliProductVariant { get; set; }
    }
}
