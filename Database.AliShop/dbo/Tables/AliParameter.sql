CREATE TABLE [dbo].[AliParameter] (
    [AliParameterId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (100)  NOT NULL,
    [Value]          NVARCHAR (MAX) NULL,
    [Create]         DATETIME       CONSTRAINT [DF_AliParameter_Create] DEFAULT (getutcdate()) NOT NULL,
    [LastUpdate]     DATETIME       NULL,
    CONSTRAINT [PK_AliParameter] PRIMARY KEY CLUSTERED ([AliParameterId] ASC)
);

