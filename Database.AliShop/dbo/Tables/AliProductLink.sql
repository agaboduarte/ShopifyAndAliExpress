CREATE TABLE [dbo].[AliProductLink] (
    [AliProductLinkId] BIGINT   IDENTITY (1, 1) NOT NULL,
    [AliStoreId]       BIGINT   NOT NULL,
    [ProductId]        BIGINT   NOT NULL,
    [Create]           DATETIME CONSTRAINT [DF_AliProductLink_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]       DATETIME CONSTRAINT [DF_AliProductLink_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliProductLink] PRIMARY KEY CLUSTERED ([AliProductLinkId] ASC),
    CONSTRAINT [FK_AliProductLink_AliStore] FOREIGN KEY ([AliStoreId]) REFERENCES [dbo].[AliStore] ([AliStoreId])
);


GO
ALTER TABLE [dbo].[AliProductLink] NOCHECK CONSTRAINT [FK_AliProductLink_AliStore];




GO
ALTER TABLE [dbo].[AliProductLink] NOCHECK CONSTRAINT [FK_AliProductLink_AliStore];




GO
ALTER TABLE [dbo].[AliProductLink] NOCHECK CONSTRAINT [FK_AliProductLink_AliStore];




GO
ALTER TABLE [dbo].[AliProductLink] NOCHECK CONSTRAINT [FK_AliProductLink_AliStore];




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductId]
    ON [dbo].[AliProductLink]([ProductId] ASC);

