namespace ISAA.Rules.Ali.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AliShopEntities : DbContext
    {
        public AliShopEntities()
            : base("name=AliShopEntities")
        {
        }

        public virtual DbSet<AliParameter> AliParameter { get; set; }
        public virtual DbSet<AliProduct> AliProduct { get; set; }
        public virtual DbSet<AliProductImage> AliProductImage { get; set; }
        public virtual DbSet<AliProductLink> AliProductLink { get; set; }
        public virtual DbSet<AliProductOption> AliProductOption { get; set; }
        public virtual DbSet<AliProductSpecific> AliProductSpecific { get; set; }
        public virtual DbSet<AliProductVariant> AliProductVariant { get; set; }
        public virtual DbSet<AliProductVariantCopy> AliProductVariantCopy { get; set; }
        public virtual DbSet<AliShopifyProduct> AliShopifyProduct { get; set; }
        public virtual DbSet<AliStore> AliStore { get; set; }
        public virtual DbSet<AliShopifyPrice> AliShopifyPrice { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AliParameter>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AliProduct>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<AliProduct>()
                .Property(e => e.Rating)
                .HasPrecision(18, 1);

            modelBuilder.Entity<AliProduct>()
                .HasMany(e => e.AliProductImage)
                .WithRequired(e => e.AliProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProduct>()
                .HasMany(e => e.AliProductOption)
                .WithRequired(e => e.AliProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProduct>()
                .HasMany(e => e.AliProductSpecific)
                .WithRequired(e => e.AliProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProduct>()
                .HasMany(e => e.AliProductVariant)
                .WithRequired(e => e.AliProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProduct>()
                .HasMany(e => e.AliProductVariantCopy)
                .WithRequired(e => e.AliProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProduct>()
                .HasMany(e => e.AliShopifyProduct)
                .WithRequired(e => e.AliProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductImage>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductImage>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductImage>()
                .Property(e => e.SHA1)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductLink>()
                .HasMany(e => e.AliProduct)
                .WithRequired(e => e.AliProductLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductLink>()
                .HasMany(e => e.AliProductImage)
                .WithRequired(e => e.AliProductLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductLink>()
                .HasMany(e => e.AliProductOption)
                .WithRequired(e => e.AliProductLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductLink>()
                .HasMany(e => e.AliProductSpecific)
                .WithRequired(e => e.AliProductLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductLink>()
                .HasMany(e => e.AliProductVariant)
                .WithRequired(e => e.AliProductLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductLink>()
                .HasMany(e => e.AliProductVariantCopy)
                .WithRequired(e => e.AliProductLink)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductOption>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductSpecific>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductSpecific>()
                .Property(e => e.Value)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductSpecific>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariant>()
                .Property(e => e.SkuPropIds)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariant>()
                .Property(e => e.Option1)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariant>()
                .Property(e => e.Option2)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariant>()
                .Property(e => e.Option3)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariant>()
                .Property(e => e.Weight)
                .HasPrecision(18, 3);

            modelBuilder.Entity<AliProductVariant>()
                .Property(e => e.OriginalPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AliProductVariant>()
                .Property(e => e.DiscountPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AliProductVariant>()
                .HasMany(e => e.AliProductVariantCopy)
                .WithRequired(e => e.AliProductVariant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliProductVariantCopy>()
                .Property(e => e.SkuPropIds)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariantCopy>()
                .Property(e => e.Option1)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariantCopy>()
                .Property(e => e.Option2)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariantCopy>()
                .Property(e => e.Option3)
                .IsUnicode(false);

            modelBuilder.Entity<AliProductVariantCopy>()
                .Property(e => e.Weight)
                .HasPrecision(18, 3);

            modelBuilder.Entity<AliProductVariantCopy>()
                .Property(e => e.OriginalPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AliProductVariantCopy>()
                .Property(e => e.DiscountPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AliShopifyProduct>()
                .Property(e => e.HandleFriendlyName)
                .IsUnicode(false);

            modelBuilder.Entity<AliShopifyProduct>()
                .Property(e => e.AvgPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AliShopifyProduct>()
                .Property(e => e.AvgCompareAtPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AliShopifyProduct>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<AliStore>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AliStore>()
                .Property(e => e.Score)
                .HasPrecision(18, 3);

            modelBuilder.Entity<AliStore>()
                .HasMany(e => e.AliProduct)
                .WithRequired(e => e.AliStore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliStore>()
                .HasMany(e => e.AliProductImage)
                .WithRequired(e => e.AliStore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliStore>()
                .HasMany(e => e.AliProductLink)
                .WithRequired(e => e.AliStore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliStore>()
                .HasMany(e => e.AliProductOption)
                .WithRequired(e => e.AliStore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliStore>()
                .HasMany(e => e.AliProductSpecific)
                .WithRequired(e => e.AliStore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliStore>()
                .HasMany(e => e.AliProductVariant)
                .WithRequired(e => e.AliStore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliStore>()
                .HasMany(e => e.AliProductVariantCopy)
                .WithRequired(e => e.AliStore)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AliShopifyPrice>()
                .Property(e => e.MinPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AliShopifyPrice>()
                .Property(e => e.MaxPrice)
                .HasPrecision(19, 4);
        }
    }
}
