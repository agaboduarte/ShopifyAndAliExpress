-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE CreateProductLink
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