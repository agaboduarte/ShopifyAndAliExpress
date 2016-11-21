namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliProductImage")]
    public partial class AliProductImage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AliProductImage()
        {
            AliProductVariant = new HashSet<AliProductVariant>();
            AliProductVariantCopy = new HashSet<AliProductVariantCopy>();
        }

        public long AliProductImageId { get; set; }

        public long AliStoreId { get; set; }

        public long AliProductLinkId { get; set; }

        public long AliProductId { get; set; }

        public long ProductId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Url { get; set; }

        [StringLength(100)]
        public string SHA1 { get; set; }

        public bool Enabled { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        public virtual AliProduct AliProduct { get; set; }

        public virtual AliProductLink AliProductLink { get; set; }

        public virtual AliStore AliStore { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductVariant> AliProductVariant { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductVariantCopy> AliProductVariantCopy { get; set; }
    }
}
