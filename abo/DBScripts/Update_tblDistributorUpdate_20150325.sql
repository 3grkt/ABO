----------------------------------------------
-- 20150325: Add StatusId
----------------------------------------------
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
PRINT N'Altering [dbo].[tblDistributorUpdate]...';


GO
ALTER TABLE [dbo].[tblDistributorUpdate]
    ADD [StatusId] SMALLINT DEFAULT (401) NOT NULL;


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
PRINT N'Creating [dbo].[FK_tblDistributorUpdate_tblStatus]...';


GO
ALTER TABLE [dbo].[tblDistributorUpdate] WITH NOCHECK
    ADD CONSTRAINT [FK_tblDistributorUpdate_tblStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[tblStatus] ([Id]);


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
ALTER TABLE [dbo].[tblDistributorUpdate] WITH CHECK CHECK CONSTRAINT [FK_tblDistributorUpdate_tblStatus];


GO
PRINT N'Update complete.';


GO
