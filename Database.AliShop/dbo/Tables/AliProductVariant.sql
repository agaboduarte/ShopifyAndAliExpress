CREATE TABLE [dbo].[AliProductVariant] (
    [AliProductVariantId]     BIGINT          IDENTITY (1, 1) NOT NULL,
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
    [Enabled]                 BIT             CONSTRAINT [DF_AliProductVariant_Enabled] DEFAULT ((1)) NOT NULL,
    [Create]                  DATETIME        CONSTRAINT [DF_AliProductVariant_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]              DATETIME        CONSTRAINT [DF_AliProductVariant_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliProductVariant] PRIMARY KEY CLUSTERED ([AliProductVariantId] ASC),
    CONSTRAINT [FK_AliProductVariant_AliProduct] FOREIGN KEY ([AliProductId]) REFERENCES [dbo].[AliProduct] ([AliProductId]),
    CONSTRAINT [FK_AliProductVariant_AliProductImage] FOREIGN KEY ([AliProductImageId]) REFERENCES [dbo].[AliProductImage] ([AliProductImageId]),
    CONSTRAINT [FK_AliProductVariant_AliProductLink] FOREIGN KEY ([AliProductLinkId]) REFERENCES [dbo].[AliProductLink] ([AliProductLinkId]),
    CONSTRAINT [FK_AliProductVariant_AliStore] FOREIGN KEY ([AliStoreId]) REFERENCES [dbo].[AliStore] ([AliStoreId])
);


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProduct];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductImage];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliStore];




GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProduct];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductImage];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliStore];




GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProduct];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductImage];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliStore];




GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProduct];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductImage];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliStore];




GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProduct];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductImage];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliStore];




GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProduct];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliStore];




GO
CREATE NONCLUSTERED INDEX [IX_AliProductLinkId]
    ON [dbo].[AliProductVariant]([AliProductLinkId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SkuPropIds]
    ON [dbo].[AliProductVariant]([SkuPropIds] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AliProductId]
    ON [dbo].[AliProductVariant]([AliProductId] ASC);

