---------------------------------------------
-- 20150324: Set identity for PK
---------------------------------------------
IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping [dbo].[FK_tblDistributorLog_tblStatus]...';


GO
ALTER TABLE [dbo].[tblDistributorLog] DROP CONSTRAINT [FK_tblDistributorLog_tblStatus];


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Dropping [dbo].[FK_tblDistributorLog_tblDistributor]...';


GO
ALTER TABLE [dbo].[tblDistributorLog] DROP CONSTRAINT [FK_tblDistributorLog_tblDistributor];


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Starting rebuilding table [dbo].[tblDistributorLog]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblDistributorLog] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [DistNumber] BIGINT   NOT NULL,
    [UpdateType] CHAR (1) NOT NULL,
    [UpdateDate] DATETIME NOT NULL,
    [StatusId]   SMALLINT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblDistributorLog])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDistributorLog] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblDistributorLog] ([Id], [DistNumber], [UpdateType], [UpdateDate], [StatusId])
        SELECT   [Id],
                 [DistNumber],
                 [UpdateType],
                 [UpdateDate],
                 [StatusId]
        FROM     [dbo].[tblDistributorLog]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDistributorLog] OFF;
    END

DROP TABLE [dbo].[tblDistributorLog];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblDistributorLog]', N'tblDistributorLog';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[FK_tblDistributorLog_tblStatus]...';


GO
ALTER TABLE [dbo].[tblDistributorLog] WITH NOCHECK
    ADD CONSTRAINT [FK_tblDistributorLog_tblStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblStatus] ([Id]);


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[FK_tblDistributorLog_tblDistributor]...';


GO
ALTER TABLE [dbo].[tblDistributorLog] WITH NOCHECK
    ADD CONSTRAINT [FK_tblDistributorLog_tblDistributor] FOREIGN KEY ([DistNumber]) REFERENCES [dbo].[tblDistributor] ([DistNumber]);


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO

IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT N'The transacted portion of the database update succeeded.'
COMMIT TRANSACTION
END
ELSE PRINT N'The transacted portion of the database update failed.'
GO
DROP TABLE #tmpErrors
GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[tblDistributorLog] WITH CHECK CHECK CONSTRAINT [FK_tblDistributorLog_tblStatus];

ALTER TABLE [dbo].[tblDistributorLog] WITH CHECK CHECK CONSTRAINT [FK_tblDistributorLog_tblDistributor];


GO
PRINT N'Update complete.';


GO


---------------------------------------------
-- 20150324: Drop FK to tblDistributor
---------------------------------------------
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tblDistributorLog_tblDistributor]') AND parent_object_id = OBJECT_ID(N'[dbo].[tblDistributorLog]'))
ALTER TABLE [dbo].[tblDistributorLog] DROP CONSTRAINT [FK_tblDistributorLog_tblDistributor]
GO



---------------------------------------------
-- 20150325: Add WarehouseId
---------------------------------------------
IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping [dbo].[FK_tblDistributorLog_tblStatus]...';


GO
ALTER TABLE [dbo].[tblDistributorLog] DROP CONSTRAINT [FK_tblDistributorLog_tblStatus];


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Starting rebuilding table [dbo].[tblDistributorLog]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblDistributorLog] (
    [Id]          INT       IDENTITY (1, 1) NOT NULL,
    [WarehouseId] NCHAR (2) NULL,
    [DistNumber]  BIGINT    NOT NULL,
    [UpdateType]  CHAR (1)  NOT NULL,
    [UpdateDate]  DATETIME  NOT NULL,
    [StatusId]    SMALLINT  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblDistributorLog])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDistributorLog] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblDistributorLog] ([Id], [DistNumber], [UpdateType], [UpdateDate], [StatusId])
        SELECT   [Id],
                 [DistNumber],
                 [UpdateType],
                 [UpdateDate],
                 [StatusId]
        FROM     [dbo].[tblDistributorLog]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDistributorLog] OFF;
    END

DROP TABLE [dbo].[tblDistributorLog];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblDistributorLog]', N'tblDistributorLog';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[FK_tblDistributorLog_tblStatus]...';


GO
ALTER TABLE [dbo].[tblDistributorLog] WITH NOCHECK
    ADD CONSTRAINT [FK_tblDistributorLog_tblStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblStatus] ([Id]);


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO
PRINT N'Creating [dbo].[FK_tblDistributorLog_tblWarehouse]...';


GO
ALTER TABLE [dbo].[tblDistributorLog] WITH NOCHECK
    ADD CONSTRAINT [FK_tblDistributorLog_tblWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[tblWarehouse] ([WarehouseId]);


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO

IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT N'The transacted portion of the database update succeeded.'
COMMIT TRANSACTION
END
ELSE PRINT N'The transacted portion of the database update failed.'
GO
DROP TABLE #tmpErrors
GO
PRINT N'Checking existing data against newly created constraints';


GO
ALTER TABLE [dbo].[tblDistributorLog] WITH CHECK CHECK CONSTRAINT [FK_tblDistributorLog_tblStatus];

ALTER TABLE [dbo].[tblDistributorLog] WITH CHECK CHECK CONSTRAINT [FK_tblDistributorLog_tblWarehouse];


GO
PRINT N'Update complete.';


GO
