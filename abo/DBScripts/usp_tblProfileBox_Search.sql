IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_tblProfileBox_Search]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_tblProfileBox_Search]
GO
CREATE PROCEDURE [dbo].[usp_tblProfileBox_Search] 
(
	@UserId int = null,
	@WarehouseId nchar(2) = null,
	@TypeId int = null,
	@StatusId int = null,
	@PageIndex int = null,
	@PageSize int = null,
	@TotalCount int output
)
AS
BEGIN
DECLARE @sql nvarchar(max)
DECLARE @where nvarchar(max)
DECLARE @from nvarchar(max)
DECLARE @select nvarchar(max)


SET @select = N'Select ROW_NUMBER() OVER(ORDER BY box.Id) AS NUMBER, box.Id,box.Name,box.[CreatedDate],users.UserName,warehouse.WarehouseName,status.Name AS Status,box.ADACount '

SET @where = N' WHERE box.WarehouseId = @wId AND box.CreatedBy = @userId AND box.StatusId = @sttId'
if @TypeId != 0
begin
set @where = @where + ' AND box.TypeId = @tId '
end
SET @from = N'
FROM [tblProfileBox] box
LEFT JOIN [tblProfileType] type ON type.Id = box.TypeId
LEFT JOIN [tblUser] users ON users.Id = box.CreatedBy
LEFT JOIN [tblWarehouse] warehouse ON warehouse.WarehouseId = box.WarehouseId
LEFT JOIN [tblStatus] status ON status.Id = box.StatusId
'  
-- Count Total Items
SET @sql = 'select @Total = COUNT(box.Id)
FROM [tblProfileBox] box
LEFT JOIN [tblProfileType] type ON type.Id = box.TypeId
LEFT JOIN [tblUser] users ON users.Id = box.CreatedBy
LEFT JOIN [tblWarehouse] warehouse ON warehouse.WarehouseId = box.WarehouseId
LEFT JOIN [tblStatus] status ON status.Id = box.StatusId' + @where

EXEC sp_executesql @sql,N'@Total int OUTPUT,@userId AS INT, @wId AS nchar(2), @tId AS INT, @sttId AS INT',
@Total = @TotalCount OUTPUT,@userId = @UserId, @wId = @WarehouseId, @tId = @TypeId, @sttId = @StatusId
--print @TotalItems


---- query Items
SET @sql = @select + @from + @where
--print @sql

SET @sql = N'Select * FROM(' + @sql + N') AS TBL
 WHERE NUMBER BETWEEN ((@PageNo - 1) * @RowspPage + 1) AND (@PageNo * @RowspPage)'
print @sql
EXEC sp_executesql @sql,N'@userId AS INT, @wId AS nchar(2), @tId AS INT, @sttId AS INT,@PageNo  INT, @RowspPage INT',
@userId = @UserId, @wId = @WarehouseId, @tId = @TypeId, @sttId = @StatusId,@PageNo = @PageIndex, @RowspPage = @PageSize




END