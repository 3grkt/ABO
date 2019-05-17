IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblDistributorLog_tblStatus]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblDistributorLog]'))
ALTER TABLE [dbo].[tblDistributorLog] DROP CONSTRAINT [FK_tblDistributorLog_tblStatus]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblDistributorLog_tblWarehouse]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblDistributorLog]'))
ALTER TABLE [dbo].[tblDistributorLog] DROP CONSTRAINT [FK_tblDistributorLog_tblWarehouse]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblDistributorLog]') AND type in (N'U'))
DROP TABLE [dbo].[tblDistributorLog]
GO

CREATE TABLE [dbo].[tblDistributorLog](
	[DISTNO] [int] NULL,
	[UPDATETYPE] [varchar](3) NULL,
	[UPDATEDTE] [int] NULL,
	[WAREHOUSE] [varchar](3) NULL
) ON [PRIMARY]

GO