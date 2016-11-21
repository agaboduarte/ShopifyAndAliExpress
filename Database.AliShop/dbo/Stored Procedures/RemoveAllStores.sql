-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE RemoveAllStores
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