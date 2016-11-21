CREATE TABLE [dbo].[AliProductSpecific] (
    [AliProductSpecificId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [AliStoreId]           BIGINT       NOT NULL,
    [AliProductLinkId]     BIGINT       NOT NULL,
    [AliProductId]         BIGINT       NOT NULL,
    [ProductId]            BIGINT       NOT NULL,
    [Name]                 VARCHAR (50) NULL,
    [Value]                VARCHAR (200) NULL,
    [Type]                 VARCHAR (50) NULL,
    [Enabled]              BIT          CONSTRAINT [DF_AliProductSpecific_Enabled] DEFAULT ((1)) NOT NULL,
    [Create]               DATETIME     CONSTRAINT [DF_AliProductSpecific_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]           DATETIME     CONSTRAINT [DF_AliProductSpecific_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliProductSpecific] PRIMARY KEY CLUSTERED ([AliProductSpecificId] ASC),
    CONSTRAINT [CK_Type] CHECK ([Type]='packaging-details' OR [Type]='item-specifics'),
    CONSTRAINT [FK_AliProductSpecific_AliProduct] FOREIGN KEY ([AliProductId]) REFERENCES [dbo].[AliProduct] ([AliProductId]),
    CONSTRAINT [FK_AliProductSpecific_AliProductLink] FOREIGN KEY ([AliProductLinkId]) REFERENCES [dbo].[AliProductLink] ([AliProductLinkId]),
    CONSTRAINT [FK_AliProductSpecific_AliStore] FOREIGN KEY ([AliStoreId]) REFERENCES [dbo].[AliStore] ([AliStoreId])
);


GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProduct];


GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliStore];




GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProduct];


GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliStore];




GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProduct];


GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliStore];




GO
CREATE NONCLUSTERED INDEX [IX_AliProductLinkId]
    ON [dbo].[AliProductSpecific]([AliProductLinkId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AliProductId]
    ON [dbo].[AliProductSpecific]([AliProductId] ASC);

