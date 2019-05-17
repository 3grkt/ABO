

ALTER TABLE [ABO].[dbo].[tblDistributorLetter]
ADD OldDistNumber bigint null
GO
ALTER TABLE [ABO].[dbo].[tblDistributorLetter]
ADD CONSTRAINT FK_tblDistributorLetter_tblDistributor3
FOREIGN KEY (OldDistNumber)
REFERENCES tblDistributor(DistNumber)
GO
-- Update on 06-23-2015

ALTER TABLE [ABO].[dbo].[tblDistributorLetter]
ADD WHId nchar(2) null
GO
ALTER TABLE [ABO].[dbo].[tblDistributorLetter]
ADD CONSTRAINT FK_tblDistributorLetter_tblWarehouse
FOREIGN KEY (WHId)
REFERENCES tblWarehouse(WarehouseId)
GO

ALTER TABLE [ABO].[dbo].[tblDistributorLetter]
ADD UserId int null
GO
ALTER TABLE [ABO].[dbo].[tblDistributorLetter]
ADD CONSTRAINT FK_tblDistributorLetter_tblUser
FOREIGN KEY (USerId)
REFERENCES tblUser(Id)
GO