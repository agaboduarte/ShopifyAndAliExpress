CREATE TABLE [dbo].[AliShopifyProduct] (
    [AliShopifyProductId]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [AliProductId]            BIGINT        NOT NULL,
    [ShopifyProductId]        BIGINT        NULL,
    [HandleFriendlyName]      VARCHAR (900) NULL,
    [AvgPrice]                MONEY         CONSTRAINT [DF_AliShopifyProduct_Price] DEFAULT ((0)) NULL,
    [AvgCompareAtPrice]       MONEY         NULL,
    [Published]               BIT           CONSTRAINT [DF_AliShopifyProduct_Published] DEFAULT ((1)) NOT NULL,
    [ExistsOnShopify]         BIT           CONSTRAINT [DF_AliShopifyProduct_ExistsOnShopify] DEFAULT ((1)) NOT NULL,
    [RequiredUpdateOnShopify] BIT           NOT NULL,
    [RowVersion]              ROWVERSION    NULL,
    [Create]                  DATETIME      CONSTRAINT [DF_AliShopifyProduct_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]              DATETIME      NULL,
    CONSTRAINT [PK_AliShopifyProduct] PRIMARY KEY CLUSTERED ([AliShopifyProductId] ASC),
    CONSTRAINT [FK_AliShopifyProduct_AliProduct] FOREIGN KEY ([AliProductId]) REFERENCES [dbo].[AliProduct] ([AliProductId])
);


GO
ALTER TABLE [dbo].[AliShopifyProduct] NOCHECK CONSTRAINT [FK_AliShopifyProduct_AliProduct];




GO
ALTER TABLE [dbo].[AliShopifyProduct] NOCHECK CONSTRAINT [FK_AliShopifyProduct_AliProduct];


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ShopifyProductId]
    ON [dbo].[AliShopifyProduct]([ShopifyProductId] ASC) WHERE ([ShopifyProductId] IS NOT NULL);



