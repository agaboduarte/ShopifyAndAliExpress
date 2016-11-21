CREATE TABLE [dbo].[AliProductOption] (
    [AliProductOptionId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [AliStoreId]         BIGINT        NOT NULL,
    [AliProductLinkId]   BIGINT        NOT NULL,
    [AliProductId]       BIGINT        NOT NULL,
    [ProductId]          BIGINT        NOT NULL,
    [Number]             INT           NULL,
    [Name]               VARCHAR (200) NULL,
    [Create]             DATETIME      CONSTRAINT [DF_AliProductOption_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]         DATETIME      CONSTRAINT [DF_AliProductOption_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliProductOption] PRIMARY KEY CLUSTERED ([AliProductOptionId] ASC),
    CONSTRAINT [CK_Number] CHECK ([Number]>=(1) AND [Number]<=(3)),
    CONSTRAINT [FK_AliProductOption_AliProduct] FOREIGN KEY ([AliProductId]) REFERENCES [dbo].[AliProduct] ([AliProductId]),
    CONSTRAINT [FK_AliProductOption_AliProductLink] FOREIGN KEY ([AliProductLinkId]) REFERENCES [dbo].[AliProductLink] ([AliProductLinkId]),
    CONSTRAINT [FK_AliProductOption_AliStore] FOREIGN KEY ([AliStoreId]) REFERENCES [dbo].[AliStore] ([AliStoreId])
);


GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProduct];


GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliStore];




GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProduct];


GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliStore];




GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProduct];


GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProductLink];


GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliStore];



