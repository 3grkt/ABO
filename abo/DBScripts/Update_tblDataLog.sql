TRUNCATE TABLE tblDataLog
GO

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
/*
The column [dbo].[tblDataLog].[PK] on table [dbo].[tblDataLog] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [dbo].[tblDataLog]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_tblDataLog] (
    [Id]        BIGINT             IDENTITY (1, 1) NOT NULL,
    [TableName] NVARCHAR (100)  NOT NULL,
    [FieldName] NVARCHAR (200)  NOT NULL,
    [PK]        NVARCHAR (30)   NOT NULL,
    [OldValue]  NVARCHAR (1000) NULL,
    [NewValue]  NVARCHAR (1000) NULL,
    [LogDate]   DATETIME        NOT NULL,
    [LogUser]   NVARCHAR (100)  NULL,
    [LogInfo]   NVARCHAR (200)  NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_tblDataLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[tblDataLog])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDataLog] ON;
        INSERT INTO [dbo].[tmp_ms_xx_tblDataLog] ([Id], [TableName], [FieldName], [OldValue], [NewValue], [LogDate], [LogUser])
        SELECT   [Id],
                 [TableName],
                 [FieldName],
                 [OldValue],
                 [NewValue],
                 [LogDate],
                 [LogUser]
        FROM     [dbo].[tblDataLog]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_tblDataLog] OFF;
    END

DROP TABLE [dbo].[tblDataLog];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_tblDataLog]', N'tblDataLog';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_tblDataLog]', N'PK_tblDataLog', N'OBJECT';

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
PRINT N'Update complete.';


GO
