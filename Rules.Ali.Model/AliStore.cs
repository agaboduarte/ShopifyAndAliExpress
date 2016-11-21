namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliStore")]
    public partial class AliStore
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AliStore()
        {
            AliProduct = new HashSet<AliProduct>();
            AliProductImage = new HashSet<AliProductImage>();
            AliProductLink = new HashSet<AliProductLink>();
            AliProductOption = new HashSet<AliProductOption>();
            AliProductSpecific = new HashSet<AliProductSpecific>();
            AliProductVariant = new HashSet<AliProductVariant>();
            AliProductVariantCopy = new HashSet<AliProductVariantCopy>();
        }

        public long AliStoreId { get; set; }

        public long StoreId { get; set; }

        [StringLength(800)]
        public string Name { get; set; }

        public decimal? Feedback { get; set; }

        public decimal? Score { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Since { get; set; }

        [Column(TypeName = "xml")]
        public string PageConfigXml { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProduct> AliProduct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductImage> AliProductImage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AliProductLink> AliProductLink { get; set; }

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
