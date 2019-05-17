ALTER TABLE [ABO].[dbo].[tblLocation]
ADD WHcode nchar(2) null
GO

ALTER TABLE [ABO].[dbo].[tblLocation]
ADD CONSTRAINT FK_tblLocation_tblWarehouse
FOREIGN KEY (WHcode)
REFERENCES tblWarehouse(WarehouseId)
GO

