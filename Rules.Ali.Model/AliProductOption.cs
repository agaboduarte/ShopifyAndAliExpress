namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliProductOption")]
    public partial class AliProductOption
    {
        public long AliProductOptionId { get; set; }

        public long AliStoreId { get; set; }

        public long AliProductLinkId { get; set; }

        public long AliProductId { get; set; }

        public long ProductId { get; set; }

        public int? Number { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }

        public virtual AliProduct AliProduct { get; set; }

        public virtual AliProductLink AliProductLink { get; set; }

        public virtual AliStore AliStore { get; set; }
    }
}
