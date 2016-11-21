namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliProductSpecific")]
    public partial class AliProductSpecific
    {
        public long AliProductSpecificId { get; set; }

        public long AliStoreId { get; set; }

        public long AliProductLinkId { get; set; }

        public long AliProductId { get; set; }

        public long ProductId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Value { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public bool Enabled { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        public virtual AliProduct AliProduct { get; set; }

        public virtual AliProductLink AliProductLink { get; set; }

        public virtual AliStore AliStore { get; set; }
    }
}
