namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliShopifyProduct")]
    public partial class AliShopifyProduct
    {
        public long AliShopifyProductId { get; set; }

        public long AliProductId { get; set; }

        public long? ShopifyProductId { get; set; }

        [StringLength(900)]
        public string HandleFriendlyName { get; set; }

        [Column(TypeName = "money")]
        public decimal? AvgPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? AvgCompareAtPrice { get; set; }

        public bool Published { get; set; }

        public bool ExistsOnShopify { get; set; }

        public bool RequiredUpdateOnShopify { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] RowVersion { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        public virtual AliProduct AliProduct { get; set; }
    }
}
