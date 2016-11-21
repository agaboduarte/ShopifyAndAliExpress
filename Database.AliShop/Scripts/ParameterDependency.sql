USE [alishop]
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
