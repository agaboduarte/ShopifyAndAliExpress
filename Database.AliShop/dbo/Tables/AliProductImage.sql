CREATE TABLE [dbo].[AliProductImage] (
    [AliProductImageId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [AliStoreId]        BIGINT        NOT NULL,
    [AliProductLinkId]  BIGINT        NOT NULL,
    [AliProductId]      BIGINT        NOT NULL,
    [ProductId]         BIGINT        NOT NULL,
    [Name]              VARCHAR (50)  NULL,
    [Url]               VARCHAR (MAX) NULL,
    [SHA1]              VARCHAR (100) NULL,
    [Enabled]           BIT           CONSTRAINT [DF_AliProductImage_Enabled] DEFAULT ((1)) NOT NULL,
    [Create]            DATETIME      CONSTRAINT [DF_AliProductImage_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]        DATETIME      CONSTRAINT [DF_AliProductImage_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliProductImage] PRIMARY KEY CLUSTERED ([AliProductImageId] ASC),
    CONSTRAINT [FK_AliProductImage_AliProduct] FOREIGN KEY ([AliProductId]) REFERENCES [dbo].[AliProduct] ([AliProductId]),
    CONSTRAINT [FK_AliProductImage_AliProductLink] FOREIGN KEY ([AliProductLinkId]) REFERENCES [dbo].[AliProductLink] ([AliProductLinkId]),
    CONSTRAINT [FK_AliProductImage_AliStore] FOREIGN KEY ([AliStoreId]) REFERENCES [dbo].[AliStore] ([AliStoreId])
);


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProduct];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliStore];




GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProduct];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliStore];




GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProduct];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliStore];




GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProduct];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliStore];




GO
CREATE NONCLUSTERED INDEX [IX_AliProductId]
    ON [dbo].[AliProductImage]([AliProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AliProductLinkId]
    ON [dbo].[AliProductImage]([AliProductLinkId] ASC);

