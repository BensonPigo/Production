CREATE TABLE [dbo].[BITaskInfo] (
    [Name]              VARCHAR (50)   NOT NULL,
    [ProcedureName]     VARCHAR (200)  CONSTRAINT [DF_BITaskInfo_ProcedureName] DEFAULT ('') NOT NULL,
    [DBName]            VARCHAR (10)   CONSTRAINT [DF_BITaskInfo_DBName] DEFAULT ('') NOT NULL,
    [HasStartDate]      BIT            CONSTRAINT [DF_BITaskInfo_HasStartDate] DEFAULT ((0)) NOT NULL,
    [HasEndDate]        BIT            CONSTRAINT [DF_BITaskInfo_HasEndDate] DEFAULT ((0)) NOT NULL,
    [HasStartDate2]     BIT            CONSTRAINT [DF_BITaskInfo_HasStartDate2] DEFAULT ((0)) NOT NULL,
    [HasEndDate2]       BIT            CONSTRAINT [DF_BITaskInfo_HasEndDate2] DEFAULT ((0)) NOT NULL,
    [StartDateDefault]  NVARCHAR (500) CONSTRAINT [DF_BITaskInfo_StartDateDefault] DEFAULT ('') NOT NULL,
    [EndDateDefault]    NVARCHAR (500) CONSTRAINT [DF_BITaskInfo_EndDateDefault] DEFAULT ('') NOT NULL,
    [StartDateDefault2] NVARCHAR (500) CONSTRAINT [DF_BITaskInfo_StartDateDefault2] DEFAULT ('') NOT NULL,
    [EndDateDefault2]   NVARCHAR (500) CONSTRAINT [DF_BITaskInfo_EndDateDefault2] DEFAULT ('') NOT NULL,
    [RunOnSunday]       BIT            CONSTRAINT [DF_BITaskInfo_RunOnSunday] DEFAULT ((0)) NOT NULL,
    [Source]            VARCHAR (50)   CONSTRAINT [DF_BITaskInfo_Source] DEFAULT ('') NOT NULL,
    [Junk]              BIT            CONSTRAINT [DF_BITaskInfo_Junk] DEFAULT ((0)) NOT NULL,
    [RunOnPM]           BIT            CONSTRAINT [DF_BITaskInfo_RunOnPM] DEFAULT ((0)) NOT NULL,
    [IsTrans]           BIT            CONSTRAINT [DF_BITaskInfo_IsTrans] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BITaskInfo] PRIMARY KEY CLUSTERED ([Name] ASC)
);


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BI Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ProcedureName Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'ProcedureName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DB Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'DBName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 StartDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasStartDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 EndDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasEndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 第二 StartDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasStartDate2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有條件 第二 EndDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'HasEndDate2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StartDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'StartDateDefault'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EndDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'EndDateDefault'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第二 StartDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'StartDateDefault2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第二 EndDate 預設值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'EndDateDefault2'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'只在周日執行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'RunOnSunday'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'BITaskInfo', @level2type=N'COLUMN',@level2name=N'Source'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'下午執行', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BITaskInfo', @level2type = N'COLUMN', @level2name = N'RunOnPM';

