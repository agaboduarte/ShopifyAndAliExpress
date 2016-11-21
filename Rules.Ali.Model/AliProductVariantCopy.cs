namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliProductVariantCopy")]
    public partial class AliProductVariantCopy
    {
        public long AliProductVariantCopyId { get; set; }

        public long AliStoreId { get; set; }

        public long AliProductLinkId { get; set; }

        public long AliProductId { get; set; }

        public long AliProductVariantId { get; set; }

        public long? AliProductImageId { get; set; }

        public long ProductId { get; set; }

        [StringLength(200)]
        public string SkuPropIds { get; set; }

        [StringLength(200)]
        public string Option1 { get; set; }

        [StringLength(200)]
        public string Option2 { get; set; }

        [StringLength(200)]
        public string Option3 { get; set; }

        public int? AvailableQuantity { get; set; }

        public int? InventoryQuantity { get; set; }

        public decimal? Weight { get; set; }

        [Column(TypeName = "money")]
        public decimal? OriginalPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountPrice { get; set; }

        public int? DiscountTimeLeftMinutes { get; set; }

        public DateTime? DiscountUpdateTime { get; set; }

        [Column(TypeName = "xml")]
        public string SkuProductXml { get; set; }

        public bool Enabled { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        public virtual AliProduct AliProduct { get; set; }

        public virtual AliProductImage AliProductImage { get; set; }

        public virtual AliProductLink AliProductLink { get; set; }

        public virtual AliProductVariant AliProductVariant { get; set; }

        public virtual AliStore AliStore { get; set; }
    }
}
