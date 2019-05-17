-------------------------------------------------------
-- 20150326: Add FileName
-------------------------------------------------------
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
PRINT N'Dropping [dbo].[FK_tblProfile_tblDistributor]...';


GO
ALTER TABLE [dbo].[tblProfile] DROP CONSTRAINT [FK_tblProfile_tblDistributor];


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
PRINT N'Dropping [dbo].[FK_tblProfile_tblProfileBox]...';


GO
ALTER TABLE [dbo].[tblProfile] DROP CONSTRAINT [FK_tblProfile_tblProfileBox];


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
PRINT N'Dropping [dbo].[FK_tblProfile_tblProfileType]...';


GO
ALTER TABLE [dbo].[tblProfile] DROP CONSTRAINT [FK_tblProfile_tblProfileType];


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
PRINT N'Dropping [dbo].[FK_tblProfile_tblUser]...';


GO
ALTER TABLE [dbo].[tblProfile] DROP CONSTRAINT [FK_tblProfile_tblUser];


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
PRINT N'Dropping [dbo].[FK_tblProfile_tblStatus]...';


GO
ALTER TABLE [dbo].[tblProfile] DROP CONSTRAINT [FK_tblProfile_tblStatus];


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
PRINT N'Dropping [dbo].[FK_tblProfile_tblWarehouse]...';


GO
ALTER TABLE [dbo].[tblProfile] DROP CONSTRAINT [FK_tblProfile_tblWarehouse];


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
PRINT N'Starting rebuilding table [dbo].[tblProfile]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblProfile] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [WarehouseId] NCHAR (2)       NULL,
    [TypeId]      INT             NOT NULL,
    [BoxId]       INT             NULL,
    [UserId]      INT             NULL,
    [DistNumber]  BIGINT          NULL,
    [CreatedDate] DATETIME        NOT NULL,
    [ScannedDate] DATETIME        NULL,
    [FileName]    NVARCHAR (100)  NULL,
    [Description] NVARCHAR (1000) NULL,
    [StatusId]    SMALLINT        NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_tblProfile] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblProfile])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblProfile] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblProfile] ([Id], [WarehouseId], [TypeId], [BoxId], [UserId], [DistNumber], [CreatedDate], [ScannedDate], [Description], [StatusId])
        SELECT   [Id],
                 [WarehouseId],
                 [TypeId],
                 [BoxId],
                 [UserId],
                 [DistNumber],
                 [CreatedDate],
                 [ScannedDate],
                 [Description],
                 [StatusId]
        FROM     [dbo].[tblProfile]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblProfile] OFF;
    END

DROP TABLE [dbo].[tblProfile];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblProfile]', N'tblProfile';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_tblProfile]', N'PK_tblProfile', N'OBJECT';

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
PRINT N'Creating [dbo].[FK_tblProfile_tblDistributor]...';


GO
ALTER TABLE [dbo].[tblProfile] WITH NOCHECK
    ADD CONSTRAINT [FK_tblProfile_tblDistributor] FOREIGN KEY ([DistNumber]) REFERENCES [dbo].[tblDistributor] ([DistNumber]);


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
PRINT N'Creating [dbo].[FK_tblProfile_tblProfileBox]...';


GO
ALTER TABLE [dbo].[tblProfile] WITH NOCHECK
    ADD CONSTRAINT [FK_tblProfile_tblProfileBox] FOREIGN KEY ([BoxId]) REFERENCES [dbo].[tblProfileBox] ([Id]);


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
PRINT N'Creating [dbo].[FK_tblProfile_tblProfileType]...';


GO
ALTER TABLE [dbo].[tblProfile] WITH NOCHECK
    ADD CONSTRAINT [FK_tblProfile_tblProfileType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[tblProfileType] ([Id]);


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
PRINT N'Creating [dbo].[FK_tblProfile_tblUser]...';


GO
ALTER TABLE [dbo].[tblProfile] WITH NOCHECK
    ADD CONSTRAINT [FK_tblProfile_tblUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[tblUser] ([Id]);


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
PRINT N'Creating [dbo].[FK_tblProfile_tblStatus]...';


GO
ALTER TABLE [dbo].[tblProfile] WITH NOCHECK
    ADD CONSTRAINT [FK_tblProfile_tblStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblStatus] ([Id]);


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
PRINT N'Creating [dbo].[FK_tblProfile_tblWarehouse]...';


GO
ALTER TABLE [dbo].[tblProfile] WITH NOCHECK
    ADD CONSTRAINT [FK_tblProfile_tblWarehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[tblWarehouse] ([WarehouseId]);


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
ALTER TABLE [dbo].[tblProfile] WITH CHECK CHECK CONSTRAINT [FK_tblProfile_tblDistributor];

ALTER TABLE [dbo].[tblProfile] WITH CHECK CHECK CONSTRAINT [FK_tblProfile_tblProfileBox];

ALTER TABLE [dbo].[tblProfile] WITH CHECK CHECK CONSTRAINT [FK_tblProfile_tblProfileType];

ALTER TABLE [dbo].[tblProfile] WITH CHECK CHECK CONSTRAINT [FK_tblProfile_tblUser];

ALTER TABLE [dbo].[tblProfile] WITH CHECK CHECK CONSTRAINT [FK_tblProfile_tblStatus];

ALTER TABLE [dbo].[tblProfile] WITH CHECK CHECK CONSTRAINT [FK_tblProfile_tblWarehouse];


GO
PRINT N'Update complete.';


GO
