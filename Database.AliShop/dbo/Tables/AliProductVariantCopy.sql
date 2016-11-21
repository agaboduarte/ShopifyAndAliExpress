CREATE TABLE [dbo].[AliProductVariantCopy] (
    [AliProductVariantCopyId] BIGINT          IDENTITY (1, 1) NOT NULL,
    [AliProductVariantId]     BIGINT          NOT NULL,
    [AliStoreId]              BIGINT          NOT NULL,
    [AliProductLinkId]        BIGINT          NOT NULL,
    [AliProductId]            BIGINT          NOT NULL,
    [AliProductImageId]       BIGINT          NULL,
    [ProductId]               BIGINT          NOT NULL,
    [SkuPropIds]              VARCHAR (200)   NULL,
    [Option1]                 VARCHAR (200)   NULL,
    [Option2]                 VARCHAR (200)   NULL,
    [Option3]                 VARCHAR (200)   NULL,
    [AvailableQuantity]       INT             NULL,
    [InventoryQuantity]       INT             NULL,
    [Weight]                  DECIMAL (18, 3) NULL,
    [OriginalPrice]           MONEY           NULL,
    [DiscountPrice]           MONEY           NULL,
    [DiscountTimeLeftMinutes] INT             NULL,
    [DiscountUpdateTime]      DATETIME        NULL,
    [SkuProductXml]           XML             NULL,
    [Enabled]                 BIT             CONSTRAINT [DF_AliProductVariantCopy_Enabled] DEFAULT ((1)) NOT NULL,
    [Create]                  DATETIME        CONSTRAINT [DF_AliProductVariantCopy_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]              DATETIME        CONSTRAINT [DF_AliProductVariantCopy_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliProductVariantCopy] PRIMARY KEY CLUSTERED ([AliProductVariantCopyId] ASC),
    CONSTRAINT [FK_AliProductVariantCopy_AliProduct] FOREIGN KEY ([AliProductId]) REFERENCES [dbo].[AliProduct] ([AliProductId]),
    CONSTRAINT [FK_AliProductVariantCopy_AliProductImage] FOREIGN KEY ([AliProductImageId]) REFERENCES [dbo].[AliProductImage] ([AliProductImageId]),
    CONSTRAINT [FK_AliProductVariantCopy_AliProductLink] FOREIGN KEY ([AliProductLinkId]) REFERENCES [dbo].[AliProductLink] ([AliProductLinkId]),
    CONSTRAINT [FK_AliProductVariantCopy_AliProductVariant] FOREIGN KEY ([AliProductVariantId]) REFERENCES [dbo].[AliProductVariant] ([AliProductVariantId]),
    CONSTRAINT [FK_AliProductVariantCopy_AliStore] FOREIGN KEY ([AliStoreId]) REFERENCES [dbo].[AliStore] ([AliStoreId])
);


GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProduct];


GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProductImage];


GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProductVariant];


GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliStore];

