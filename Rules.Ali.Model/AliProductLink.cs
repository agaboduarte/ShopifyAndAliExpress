namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliProductLink")]
    public partial class AliProductLink
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AliProductLink()
        {
            AliProduct = new HashSet<AliProduct>();
            AliProductImage = new HashSet<AliProductImage>();
            AliProductOption = new HashSet<AliProductOption>();
            AliProductSpecific = new HashSet<AliProductSpecific>();
            AliProductVariant = new HashSet<AliProductVariant>();
            AliProductVariantCopy = new HashSet<AliProductVariantCopy>();
        }

        public long AliProductLinkId { get; set; }

        public long AliStoreId { get; set; }

        public long ProductId { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProduct> AliProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductImage> AliProductImage { get; set; }

        public virtual AliStore AliStore { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductOption> AliProductOption { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductSpecific> AliProductSpecific { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductVariant> AliProductVariant { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductVariantCopy> AliProductVariantCopy { get; set; }
    }
}
