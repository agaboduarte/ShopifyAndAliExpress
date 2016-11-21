CREATE TABLE [dbo].[AliProduct] (
    [AliProductId]     BIGINT          IDENTITY (1, 1) NOT NULL,
    [AliStoreId]       BIGINT          NOT NULL,
    [AliProductLinkId] BIGINT          NOT NULL,
    [ProductId]        BIGINT          NOT NULL,
    [Title]            VARCHAR (MAX)   NULL,
    [ProcessingTime]   INT             NULL,
    [OrdersCount]      INT             NULL,
    [Rating]           DECIMAL (18, 1) NULL,
    [RunParamsXml]     XML             NULL,
    [Enabled]          BIT             CONSTRAINT [DF_AliProduct_Enabled] DEFAULT ((1)) NOT NULL,
    [Create]           DATETIME        CONSTRAINT [DF_AliProduct_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]       DATETIME        CONSTRAINT [DF_AliProduct_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliProduct] PRIMARY KEY CLUSTERED ([AliProductId] ASC),
    CONSTRAINT [FK_AliProduct_AliProductLink] FOREIGN KEY ([AliProductLinkId]) REFERENCES [dbo].[AliProductLink] ([AliProductLinkId]),
    CONSTRAINT [FK_AliProduct_AliStore] FOREIGN KEY ([AliStoreId]) REFERENCES [dbo].[AliStore] ([AliStoreId])
);


GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliProductLink];


GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliStore];




GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliProductLink];


GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliStore];




GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliProductLink];


GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliStore];




GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliProductLink];


GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliStore];




GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliProductLink];


GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliStore];




GO
CREATE NONCLUSTERED INDEX [IX_AliProductLInkId]
    ON [dbo].[AliProduct]([AliProductLinkId] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductId]
    ON [dbo].[AliProduct]([ProductId] ASC);

