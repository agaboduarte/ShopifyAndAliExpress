CREATE DATABASE [alishop];
GO
USE [alishop]
GO
/****** Object:  UserDefinedFunction [dbo].[ToTitleCase]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ToTitleCase] (@InputString VARCHAR(4000) )
RETURNS VARCHAR(4000)
AS
BEGIN
DECLARE @Index INT
DECLARE @Char CHAR(1)
DECLARE @OutputString VARCHAR(255)
SET @OutputString = LOWER(@InputString)
SET @Index = 2
SET @OutputString =
STUFF(@OutputString, 1, 1,UPPER(SUBSTRING(@InputString,1,1)))
WHILE @Index <= LEN(@InputString)
BEGIN
SET @Char = SUBSTRING(@InputString, @Index, 1)
IF @Char IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&','''','(')
IF @Index + 1 <= LEN(@InputString)
BEGIN
IF @Char != ''''
OR
UPPER(SUBSTRING(@InputString, @Index + 1, 1)) != 'S'
SET @OutputString =
STUFF(@OutputString, @Index + 1, 1,UPPER(SUBSTRING(@InputString, @Index + 1, 1)))
END
SET @Index = @Index + 1
END
RETURN ISNULL(@OutputString,'')
END
GO
/****** Object:  Table [dbo].[AliParameter]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliParameter](
	[AliParameterId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Create] [datetime] NOT NULL CONSTRAINT [DF_AliParameter_Create]  DEFAULT (getutcdate()),
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliParameter] PRIMARY KEY CLUSTERED 
(
	[AliParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliProduct]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliProduct](
	[AliProductId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliStoreId] [bigint] NOT NULL,
	[AliProductLinkId] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Title] [varchar](max) NULL,
	[ProcessingTime] [int] NULL,
	[OrdersCount] [int] NULL,
	[Rating] [decimal](18, 1) NULL,
	[RunParamsXml] [xml] NULL,
	[Enabled] [bit] NOT NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliProduct] PRIMARY KEY CLUSTERED 
(
	[AliProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliProductImage]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliProductImage](
	[AliProductImageId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliStoreId] [bigint] NOT NULL,
	[AliProductLinkId] [bigint] NOT NULL,
	[AliProductId] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Name] [varchar](50) NULL,
	[Url] [varchar](max) NULL,
	[SHA1] [varchar](100) NULL,
	[Enabled] [bit] NOT NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliProductImage] PRIMARY KEY CLUSTERED 
(
	[AliProductImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliProductLink]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AliProductLink](
	[AliProductLinkId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliStoreId] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliProductLink] PRIMARY KEY CLUSTERED 
(
	[AliProductLinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AliProductOption]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliProductOption](
	[AliProductOptionId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliStoreId] [bigint] NOT NULL,
	[AliProductLinkId] [bigint] NOT NULL,
	[AliProductId] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Number] [int] NULL,
	[Name] [varchar](200) NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliProductOption] PRIMARY KEY CLUSTERED 
(
	[AliProductOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliProductSpecific]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliProductSpecific](
	[AliProductSpecificId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliStoreId] [bigint] NOT NULL,
	[AliProductLinkId] [bigint] NOT NULL,
	[AliProductId] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Name] [varchar](50) NULL,
	[Value] [varchar](200) NULL,
	[Type] [varchar](50) NULL,
	[Enabled] [bit] NOT NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliProductSpecific] PRIMARY KEY CLUSTERED 
(
	[AliProductSpecificId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliProductVariant]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliProductVariant](
	[AliProductVariantId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliStoreId] [bigint] NOT NULL,
	[AliProductLinkId] [bigint] NOT NULL,
	[AliProductId] [bigint] NOT NULL,
	[AliProductImageId] [bigint] NULL,
	[ProductId] [bigint] NOT NULL,
	[SkuPropIds] [varchar](200) NULL,
	[Option1] [varchar](200) NULL,
	[Option2] [varchar](200) NULL,
	[Option3] [varchar](200) NULL,
	[AvailableQuantity] [int] NULL,
	[InventoryQuantity] [int] NULL,
	[Weight] [decimal](18, 3) NULL,
	[OriginalPrice] [money] NULL,
	[DiscountPrice] [money] NULL,
	[DiscountTimeLeftMinutes] [int] NULL,
	[DiscountUpdateTime] [datetime] NULL,
	[SkuProductXml] [xml] NULL,
	[Enabled] [bit] NOT NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliProductVariant] PRIMARY KEY CLUSTERED 
(
	[AliProductVariantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliProductVariantCopy]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliProductVariantCopy](
	[AliProductVariantCopyId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliProductVariantId] [bigint] NOT NULL,
	[AliStoreId] [bigint] NOT NULL,
	[AliProductLinkId] [bigint] NOT NULL,
	[AliProductId] [bigint] NOT NULL,
	[AliProductImageId] [bigint] NULL,
	[ProductId] [bigint] NOT NULL,
	[SkuPropIds] [varchar](200) NULL,
	[Option1] [varchar](200) NULL,
	[Option2] [varchar](200) NULL,
	[Option3] [varchar](200) NULL,
	[AvailableQuantity] [int] NULL,
	[InventoryQuantity] [int] NULL,
	[Weight] [decimal](18, 3) NULL,
	[OriginalPrice] [money] NULL,
	[DiscountPrice] [money] NULL,
	[DiscountTimeLeftMinutes] [int] NULL,
	[DiscountUpdateTime] [datetime] NULL,
	[SkuProductXml] [xml] NULL,
	[Enabled] [bit] NOT NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliProductVariantCopy] PRIMARY KEY CLUSTERED 
(
	[AliProductVariantCopyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliShopifyPrice]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AliShopifyPrice](
	[AliShopifyPriceId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliProductId] [bigint] NULL,
	[AliProductVariantId] [bigint] NULL,
	[MinPrice] [money] NULL,
	[MaxPrice] [money] NULL,
	[Factor] [decimal](18, 2) NOT NULL,
	[IncrementTax] [decimal](18, 2) NULL,
	[FixedPrice] [money] NULL,
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_AliShopifyPrice_Enabled]  DEFAULT ((1)),
	[Create] [datetime] NOT NULL CONSTRAINT [DF_AliShopifyPrice_Create]  DEFAULT (getutcdate()),
	[LastUpdate] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AliShopifyProduct]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliShopifyProduct](
	[AliShopifyProductId] [bigint] IDENTITY(1,1) NOT NULL,
	[AliProductId] [bigint] NOT NULL,
	[ShopifyProductId] [bigint] NULL,
	[HandleFriendlyName] [varchar](900) NULL,
	[AvgPrice] [money] NULL,
	[AvgCompareAtPrice] [money] NULL,
	[Published] [bit] NOT NULL,
	[ExistsOnShopify] [bit] NOT NULL,
	[RequiredUpdateOnShopify] [bit] NOT NULL,
	[RowVersion] [timestamp] NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliShopifyProduct] PRIMARY KEY CLUSTERED 
(
	[AliShopifyProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AliStore]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AliStore](
	[AliStoreId] [bigint] IDENTITY(1,1) NOT NULL,
	[StoreId] [bigint] NOT NULL,
	[Name] [varchar](800) NULL,
	[Feedback] [decimal](18, 2) NULL,
	[Score] [decimal](18, 3) NULL,
	[Since] [date] NULL,
	[PageConfigXml] [xml] NULL,
	[Create] [datetime] NOT NULL,
	[LastUpdate] [datetime] NULL,
 CONSTRAINT [PK_AliStore] PRIMARY KEY CLUSTERED 
(
	[AliStoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AliParameter] ON 

GO
INSERT [dbo].[AliParameter] ([AliParameterId], [Name], [Value], [Create], [LastUpdate]) VALUES (1, N'store_url', N'http://www.aliexpress.com/store/{store_id}', CAST(N'2016-01-26 19:24:49.843' AS DateTime), NULL)
GO
INSERT [dbo].[AliParameter] ([AliParameterId], [Name], [Value], [Create], [LastUpdate]) VALUES (2, N'product_url', N'http://www.aliexpress.com/store/product//{store_id}_{product_id}.html', CAST(N'2016-01-26 19:24:49.843' AS DateTime), NULL)
GO
INSERT [dbo].[AliParameter] ([AliParameterId], [Name], [Value], [Create], [LastUpdate]) VALUES (3, N'score_url', N'http://www.aliexpress.com/store/feedback-score/{store_id}.html', CAST(N'2016-01-26 19:24:49.843' AS DateTime), NULL)
GO
INSERT [dbo].[AliParameter] ([AliParameterId], [Name], [Value], [Create], [LastUpdate]) VALUES (4, N'stock_safety_margin', N'15', CAST(N'2016-01-28 21:31:38.207' AS DateTime), NULL)
GO
INSERT [dbo].[AliParameter] ([AliParameterId], [Name], [Value], [Create], [LastUpdate]) VALUES (5, N'use_discount_min_time_left', N'4320', CAST(N'2016-01-28 21:31:38.207' AS DateTime), NULL)
GO
INSERT [dbo].[AliParameter] ([AliParameterId], [Name], [Value], [Create], [LastUpdate]) VALUES (6, N'product_title_invalid_keywords', N'ships free, ship free, free ships,free ship, free shipping,1 free shipping!, wholesales, wholesale, + free shipping, shipping, factory, pc/lote, 1 pc, 1pc, lote, retail, $10 (Mix Order)', CAST(N'2016-02-02 15:51:26.010' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[AliParameter] OFF
GO
SET IDENTITY_INSERT [dbo].[AliShopifyPrice] ON 

GO
INSERT [dbo].[AliShopifyPrice] ([AliShopifyPriceId], [AliProductId], [AliProductVariantId], [MinPrice], [MaxPrice], [Factor], [IncrementTax], [FixedPrice], [Enabled], [Create], [LastUpdate]) VALUES (1, NULL, NULL, NULL, NULL, CAST(2.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), 0.3900, 1, CAST(N'2016-01-28 21:36:16.437' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[AliShopifyPrice] OFF
GO
/****** Object:  Index [IX_AliProductLInkId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_AliProductLInkId] ON [dbo].[AliProduct]
(
	[AliProductLinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductId] ON [dbo].[AliProduct]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AliProductId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_AliProductId] ON [dbo].[AliProductImage]
(
	[AliProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AliProductLinkId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_AliProductLinkId] ON [dbo].[AliProductImage]
(
	[AliProductLinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductId] ON [dbo].[AliProductLink]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AliProductId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_AliProductId] ON [dbo].[AliProductSpecific]
(
	[AliProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AliProductLinkId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_AliProductLinkId] ON [dbo].[AliProductSpecific]
(
	[AliProductLinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AliProductId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_AliProductId] ON [dbo].[AliProductVariant]
(
	[AliProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AliProductLinkId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_AliProductLinkId] ON [dbo].[AliProductVariant]
(
	[AliProductLinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_SkuPropIds]    Script Date: 21/11/2016 11:25:19 ******/
CREATE NONCLUSTERED INDEX [IX_SkuPropIds] ON [dbo].[AliProductVariant]
(
	[SkuPropIds] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShopifyProductId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ShopifyProductId] ON [dbo].[AliShopifyProduct]
(
	[ShopifyProductId] ASC
)
WHERE ([ShopifyProductId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StoreId]    Script Date: 21/11/2016 11:25:19 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_StoreId] ON [dbo].[AliStore]
(
	[StoreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AliProduct] ADD  CONSTRAINT [DF_AliProduct_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[AliProduct] ADD  CONSTRAINT [DF_AliProduct_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliProduct] ADD  CONSTRAINT [DF_AliProduct_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliProductImage] ADD  CONSTRAINT [DF_AliProductImage_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[AliProductImage] ADD  CONSTRAINT [DF_AliProductImage_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliProductImage] ADD  CONSTRAINT [DF_AliProductImage_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliProductLink] ADD  CONSTRAINT [DF_AliProductLink_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliProductLink] ADD  CONSTRAINT [DF_AliProductLink_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliProductOption] ADD  CONSTRAINT [DF_AliProductOption_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliProductOption] ADD  CONSTRAINT [DF_AliProductOption_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliProductSpecific] ADD  CONSTRAINT [DF_AliProductSpecific_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[AliProductSpecific] ADD  CONSTRAINT [DF_AliProductSpecific_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliProductSpecific] ADD  CONSTRAINT [DF_AliProductSpecific_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliProductVariant] ADD  CONSTRAINT [DF_AliProductVariant_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[AliProductVariant] ADD  CONSTRAINT [DF_AliProductVariant_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliProductVariant] ADD  CONSTRAINT [DF_AliProductVariant_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliProductVariantCopy] ADD  CONSTRAINT [DF_AliProductVariantCopy_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[AliProductVariantCopy] ADD  CONSTRAINT [DF_AliProductVariantCopy_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliProductVariantCopy] ADD  CONSTRAINT [DF_AliProductVariantCopy_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliShopifyProduct] ADD  CONSTRAINT [DF_AliShopifyProduct_Price]  DEFAULT ((0)) FOR [AvgPrice]
GO
ALTER TABLE [dbo].[AliShopifyProduct] ADD  CONSTRAINT [DF_AliShopifyProduct_Published]  DEFAULT ((1)) FOR [Published]
GO
ALTER TABLE [dbo].[AliShopifyProduct] ADD  CONSTRAINT [DF_AliShopifyProduct_ExistsOnShopify]  DEFAULT ((1)) FOR [ExistsOnShopify]
GO
ALTER TABLE [dbo].[AliShopifyProduct] ADD  CONSTRAINT [DF_AliShopifyProduct_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliStore] ADD  CONSTRAINT [DF_AliStore_Create]  DEFAULT (getutcdate()) FOR [Create]
GO
ALTER TABLE [dbo].[AliStore] ADD  CONSTRAINT [DF_AliStore_Update]  DEFAULT (getutcdate()) FOR [LastUpdate]
GO
ALTER TABLE [dbo].[AliProduct]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProduct_AliProductLink] FOREIGN KEY([AliProductLinkId])
REFERENCES [dbo].[AliProductLink] ([AliProductLinkId])
GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliProductLink]
GO
ALTER TABLE [dbo].[AliProduct]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProduct_AliStore] FOREIGN KEY([AliStoreId])
REFERENCES [dbo].[AliStore] ([AliStoreId])
GO
ALTER TABLE [dbo].[AliProduct] NOCHECK CONSTRAINT [FK_AliProduct_AliStore]
GO
ALTER TABLE [dbo].[AliProductImage]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductImage_AliProduct] FOREIGN KEY([AliProductId])
REFERENCES [dbo].[AliProduct] ([AliProductId])
GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProduct]
GO
ALTER TABLE [dbo].[AliProductImage]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductImage_AliProductLink] FOREIGN KEY([AliProductLinkId])
REFERENCES [dbo].[AliProductLink] ([AliProductLinkId])
GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliProductLink]
GO
ALTER TABLE [dbo].[AliProductImage]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductImage_AliStore] FOREIGN KEY([AliStoreId])
REFERENCES [dbo].[AliStore] ([AliStoreId])
GO
ALTER TABLE [dbo].[AliProductImage] NOCHECK CONSTRAINT [FK_AliProductImage_AliStore]
GO
ALTER TABLE [dbo].[AliProductLink]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductLink_AliStore] FOREIGN KEY([AliStoreId])
REFERENCES [dbo].[AliStore] ([AliStoreId])
GO
ALTER TABLE [dbo].[AliProductLink] NOCHECK CONSTRAINT [FK_AliProductLink_AliStore]
GO
ALTER TABLE [dbo].[AliProductOption]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductOption_AliProduct] FOREIGN KEY([AliProductId])
REFERENCES [dbo].[AliProduct] ([AliProductId])
GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProduct]
GO
ALTER TABLE [dbo].[AliProductOption]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductOption_AliProductLink] FOREIGN KEY([AliProductLinkId])
REFERENCES [dbo].[AliProductLink] ([AliProductLinkId])
GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliProductLink]
GO
ALTER TABLE [dbo].[AliProductOption]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductOption_AliStore] FOREIGN KEY([AliStoreId])
REFERENCES [dbo].[AliStore] ([AliStoreId])
GO
ALTER TABLE [dbo].[AliProductOption] NOCHECK CONSTRAINT [FK_AliProductOption_AliStore]
GO
ALTER TABLE [dbo].[AliProductSpecific]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductSpecific_AliProduct] FOREIGN KEY([AliProductId])
REFERENCES [dbo].[AliProduct] ([AliProductId])
GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProduct]
GO
ALTER TABLE [dbo].[AliProductSpecific]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductSpecific_AliProductLink] FOREIGN KEY([AliProductLinkId])
REFERENCES [dbo].[AliProductLink] ([AliProductLinkId])
GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliProductLink]
GO
ALTER TABLE [dbo].[AliProductSpecific]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductSpecific_AliStore] FOREIGN KEY([AliStoreId])
REFERENCES [dbo].[AliStore] ([AliStoreId])
GO
ALTER TABLE [dbo].[AliProductSpecific] NOCHECK CONSTRAINT [FK_AliProductSpecific_AliStore]
GO
ALTER TABLE [dbo].[AliProductVariant]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariant_AliProduct] FOREIGN KEY([AliProductId])
REFERENCES [dbo].[AliProduct] ([AliProductId])
GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProduct]
GO
ALTER TABLE [dbo].[AliProductVariant]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariant_AliProductImage] FOREIGN KEY([AliProductImageId])
REFERENCES [dbo].[AliProductImage] ([AliProductImageId])
GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductImage]
GO
ALTER TABLE [dbo].[AliProductVariant]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariant_AliProductLink] FOREIGN KEY([AliProductLinkId])
REFERENCES [dbo].[AliProductLink] ([AliProductLinkId])
GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliProductLink]
GO
ALTER TABLE [dbo].[AliProductVariant]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariant_AliStore] FOREIGN KEY([AliStoreId])
REFERENCES [dbo].[AliStore] ([AliStoreId])
GO
ALTER TABLE [dbo].[AliProductVariant] NOCHECK CONSTRAINT [FK_AliProductVariant_AliStore]
GO
ALTER TABLE [dbo].[AliProductVariantCopy]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariantCopy_AliProduct] FOREIGN KEY([AliProductId])
REFERENCES [dbo].[AliProduct] ([AliProductId])
GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProduct]
GO
ALTER TABLE [dbo].[AliProductVariantCopy]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariantCopy_AliProductImage] FOREIGN KEY([AliProductImageId])
REFERENCES [dbo].[AliProductImage] ([AliProductImageId])
GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProductImage]
GO
ALTER TABLE [dbo].[AliProductVariantCopy]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariantCopy_AliProductLink] FOREIGN KEY([AliProductLinkId])
REFERENCES [dbo].[AliProductLink] ([AliProductLinkId])
GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProductLink]
GO
ALTER TABLE [dbo].[AliProductVariantCopy]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariantCopy_AliProductVariant] FOREIGN KEY([AliProductVariantId])
REFERENCES [dbo].[AliProductVariant] ([AliProductVariantId])
GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliProductVariant]
GO
ALTER TABLE [dbo].[AliProductVariantCopy]  WITH NOCHECK ADD  CONSTRAINT [FK_AliProductVariantCopy_AliStore] FOREIGN KEY([AliStoreId])
REFERENCES [dbo].[AliStore] ([AliStoreId])
GO
ALTER TABLE [dbo].[AliProductVariantCopy] NOCHECK CONSTRAINT [FK_AliProductVariantCopy_AliStore]
GO
ALTER TABLE [dbo].[AliShopifyPrice]  WITH NOCHECK ADD  CONSTRAINT [FK_AliShopifyPrice_AliProduct] FOREIGN KEY([AliProductId])
REFERENCES [dbo].[AliProduct] ([AliProductId])
GO
ALTER TABLE [dbo].[AliShopifyPrice] NOCHECK CONSTRAINT [FK_AliShopifyPrice_AliProduct]
GO
ALTER TABLE [dbo].[AliShopifyPrice]  WITH NOCHECK ADD  CONSTRAINT [FK_AliShopifyPrice_AliProductVariant] FOREIGN KEY([AliProductVariantId])
REFERENCES [dbo].[AliProductVariant] ([AliProductVariantId])
GO
ALTER TABLE [dbo].[AliShopifyPrice] NOCHECK CONSTRAINT [FK_AliShopifyPrice_AliProductVariant]
GO
ALTER TABLE [dbo].[AliShopifyProduct]  WITH NOCHECK ADD  CONSTRAINT [FK_AliShopifyProduct_AliProduct] FOREIGN KEY([AliProductId])
REFERENCES [dbo].[AliProduct] ([AliProductId])
GO
ALTER TABLE [dbo].[AliShopifyProduct] NOCHECK CONSTRAINT [FK_AliShopifyProduct_AliProduct]
GO
ALTER TABLE [dbo].[AliProductOption]  WITH CHECK ADD  CONSTRAINT [CK_Number] CHECK  (([Number]>=(1) AND [Number]<=(3)))
GO
ALTER TABLE [dbo].[AliProductOption] CHECK CONSTRAINT [CK_Number]
GO
ALTER TABLE [dbo].[AliProductSpecific]  WITH CHECK ADD  CONSTRAINT [CK_Type] CHECK  (([Type]='packaging-details' OR [Type]='item-specifics'))
GO
ALTER TABLE [dbo].[AliProductSpecific] CHECK CONSTRAINT [CK_Type]
GO
/****** Object:  StoredProcedure [dbo].[CreateProductLink]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateProductLink]
	@ProductId BIGINT,
	@StoreId BIGINT 
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @AliStoreId BIGINT;

	IF NOT EXISTS (
		SELECT TOP 1 1 
		FROM AliStore S
		WHERE S.StoreId = @StoreId	
	) 
	BEGIN
		INSERT INTO AliStore (StoreId) VALUES (@StoreId);
	END

	SET @AliStoreId = (
		SELECT TOP 1 S.AliStoreId
		FROM AliStore S
		WHERE S.StoreId = @StoreId	
	);

	IF NOT EXISTS (
		SELECT TOP 1 1 
		FROM AliProductLink PL
		WHERE PL.ProductId = @ProductId
	) 
	BEGIN
		INSERT INTO AliProductLink (AliStoreId, ProductId) VALUES (@AliStoreId, @ProductId);
	END
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveAllStores]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveAllStores]
AS
BEGIN
	DECLARE @vendor_id bigint

DECLARE vendor_cursor CURSOR FOR 
	SELECT AliStoreId
	FROM [dbo].[AliStore]

	OPEN vendor_cursor

	FETCH NEXT FROM vendor_cursor 
	INTO @vendor_id

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [dbo].[RemoveStore] @vendor_id

		FETCH NEXT FROM vendor_cursor 
		INTO @vendor_id
	END 

	CLOSE vendor_cursor;
	DEALLOCATE vendor_cursor;
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveStore]    Script Date: 21/11/2016 11:25:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RemoveStore]
	@AliStoreId BIGINT
AS
BEGIN
	DELETE FROM AliShopifyProduct WHERE AliProductId IN (SELECT AliProductId FROM AliProduct WHERE AliStoreId = @AliStoreId);
	DELETE FROM AliStore WHERE AliStoreId = @AliStoreId;
	DELETE FROM AliProductLink WHERE AliStoreId = @AliStoreId;
	DELETE FROM AliProduct WHERE AliStoreId = @AliStoreId;
	DELETE FROM AliProductImage WHERE AliStoreId = @AliStoreId;
	DELETE FROM AliProductOption WHERE AliStoreId = @AliStoreId;
	DELETE FROM AliProductSpecific WHERE AliStoreId = @AliStoreId;
	DELETE FROM AliProductVariant WHERE AliStoreId = @AliStoreId;
END
GO
