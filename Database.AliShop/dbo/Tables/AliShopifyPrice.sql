CREATE TABLE [dbo].[AliShopifyPrice] (
    [AliShopifyPriceId]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [AliProductId]        BIGINT          NULL,
    [AliProductVariantId] BIGINT          NULL,
    [MinPrice]            MONEY           NULL,
    [MaxPrice]            MONEY           NULL,
    [Factor]              DECIMAL (18, 2) NOT NULL,
    [IncrementTax]        DECIMAL (18, 2) NULL,
    [FixedPrice]          MONEY           NULL,
    [Enabled]             BIT             CONSTRAINT [DF_AliShopifyPrice_Enabled] DEFAULT ((1)) NOT NULL,
    [Create]              DATETIME        CONSTRAINT [DF_AliShopifyPrice_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]          DATETIME        NULL,
    CONSTRAINT [FK_AliShopifyPrice_AliProduct] FOREIGN KEY ([AliProductId]) REFERENCES [dbo].[AliProduct] ([AliProductId]),
    CONSTRAINT [FK_AliShopifyPrice_AliProductVariant] FOREIGN KEY ([AliProductVariantId]) REFERENCES [dbo].[AliProductVariant] ([AliProductVariantId])
);


GO
ALTER TABLE [dbo].[AliShopifyPrice] NOCHECK CONSTRAINT [FK_AliShopifyPrice_AliProduct];


GO
ALTER TABLE [dbo].[AliShopifyPrice] NOCHECK CONSTRAINT [FK_AliShopifyPrice_AliProductVariant];




GO
ALTER TABLE [dbo].[AliShopifyPrice] NOCHECK CONSTRAINT [FK_AliShopifyPrice_AliProduct];


GO
ALTER TABLE [dbo].[AliShopifyPrice] NOCHECK CONSTRAINT [FK_AliShopifyPrice_AliProductVariant];




GO
ALTER TABLE [dbo].[AliShopifyPrice] NOCHECK CONSTRAINT [FK_AliShopifyPrice_AliProduct];





