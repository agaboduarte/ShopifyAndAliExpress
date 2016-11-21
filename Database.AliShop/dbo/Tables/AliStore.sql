CREATE TABLE [dbo].[AliStore] (
    [AliStoreId]    BIGINT          IDENTITY (1, 1) NOT NULL,
    [StoreId]       BIGINT          NOT NULL,
    [Name]          VARCHAR (800)   NULL,
    [Feedback]      DECIMAL (18, 2) NULL,
    [Score]         DECIMAL (18, 3) NULL,
    [Since]         DATE            NULL,
    [PageConfigXml] XML             NULL,
    [Create]        DATETIME        CONSTRAINT [DF_AliStore_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]    DATETIME        CONSTRAINT [DF_AliStore_Update] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_AliStore] PRIMARY KEY CLUSTERED ([AliStoreId] ASC)
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_StoreId]
    ON [dbo].[AliStore]([StoreId] ASC);

