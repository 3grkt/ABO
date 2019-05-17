ALTER TABLE [ABO].[dbo].[tblProfileBox]
	ADD  ProfileCount int null
GO
-- Update 06/18
ALTER TABLE [ABO].[dbo].[tblProfileBox]
DROP COLUMN OfficeId
GO
ALTER TABLE [ABO].[dbo].[tblProfileBox]
ADD OfficeId NCHAR(2)
GO

ALTER TABLE [ABO].[dbo].[tblProfileBox]
ADD CONSTRAINT FK_tblProfileBox_tblWarehouse2
FOREIGN KEY (OfficeId)
REFERENCES tblWarehouse(WarehouseId)
GO