namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AliParameter")]
    public partial class AliParameter
    {
        public long AliParameterId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Value { get; set; }

        public DateTime Create { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
