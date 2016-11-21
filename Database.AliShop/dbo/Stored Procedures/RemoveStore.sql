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