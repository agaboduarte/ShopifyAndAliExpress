namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliProduct")]
    public partial class AliProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AliProduct()
        {
            AliProductImage = new HashSet<AliProductImage>();
            AliProductOption = new HashSet<AliProductOption>();
            AliProductSpecific = new HashSet<AliProductSpecific>();
            AliProductVariant = new HashSet<AliProductVariant>();
            AliProductVariantCopy = new HashSet<AliProductVariantCopy>();
            AliShopifyPrice = new HashSet<AliShopifyPrice>();
            AliShopifyProduct = new HashSet<AliShopifyProduct>();
        }

        public long AliProductId { get; set; }

        public long AliStoreId { get; set; }

        public long AliProductLinkId { get; set; }

        public long ProductId { get; set; }

        public string Title { get; set; }

        public int? ProcessingTime { get; set; }

        public int? OrdersCount { get; set; }

        public decimal? Rating { get; set; }

        [Column(TypeName = "xml")]
        public string RunParamsXml { get; set; }

        public bool Enabled { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        public virtual AliProductLink AliProductLink { get; set; }

        public virtual AliStore AliStore { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductImage> AliProductImage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductOption> AliProductOption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductSpecific> AliProductSpecific { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductVariant> AliProductVariant { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductVariantCopy> AliProductVariantCopy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliShopifyPrice> AliShopifyPrice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliShopifyProduct> AliShopifyProduct { get; set; }
    }
}
