CREATE TABLE [dbo].[LineMapping] (
    [StyleID]          VARCHAR (15)   CONSTRAINT [DF_LineMapping_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]         VARCHAR (10)   CONSTRAINT [DF_LineMapping_SeasonID] DEFAULT ('') NOT NULL,
    [BrandID]          VARCHAR (8)    CONSTRAINT [DF_LineMapping_BrandID] DEFAULT ('') NOT NULL,
    [StyleUKey]        BIGINT         CONSTRAINT [DF_LineMapping_StyleUKey] DEFAULT ((0)) NOT NULL,
    [Version]          TINYINT        CONSTRAINT [DF_LineMapping_Version] DEFAULT ((0)) NOT NULL,
    [FactoryID]        VARCHAR (8)    CONSTRAINT [DF_LineMapping_FactoryID] DEFAULT ('') NULL,
    [ComboType]        VARCHAR (1)    CONSTRAINT [DF_LineMapping_ComboType] DEFAULT ('') NULL,
    [SewingLineID]     VARCHAR (2)    CONSTRAINT [DF_LineMapping_SewingLineID] DEFAULT ('') NULL,
    [Team]             VARCHAR (1)    CONSTRAINT [DF_LineMapping_Team] DEFAULT ('') NULL,
    [IdealOperators]   TINYINT        CONSTRAINT [DF_LineMapping_IdealOperators] DEFAULT ((0)) NULL,
    [CurrentOperators] SMALLINT        CONSTRAINT [DF_LineMapping_CurrentOperators] DEFAULT ((0)) NULL,
    [StandardOutput]   INT            CONSTRAINT [DF_LineMapping_StandardOutput] DEFAULT ((0)) NULL,
    [DailyDemand]      INT            CONSTRAINT [DF_LineMapping_DailyDemand] DEFAULT ((0)) NULL,
    [Workhour]         NUMERIC (4, 1) CONSTRAINT [DF_LineMapping_Workhour] DEFAULT ((0)) NULL,
    [NetTime]          INT            CONSTRAINT [DF_LineMapping_NetTime] DEFAULT ((0)) NULL,
    [TaktTime]         INT            CONSTRAINT [DF_LineMapping_TaktTime] DEFAULT ((0)) NULL,
    [TotalGSD]         INT            CONSTRAINT [DF_LineMapping_TotalGSD] DEFAULT ((0)) NULL,
    [TotalCycle]       INT            CONSTRAINT [DF_LineMapping_TotalCycle] DEFAULT ((0)) NULL,
    [HighestGSD]       NUMERIC (7, 2) CONSTRAINT [DF_LineMapping_HighestGSD] DEFAULT ((0)) NULL,
    [HighestCycle]     NUMERIC (7, 2) CONSTRAINT [DF_LineMapping_HighestCycle] DEFAULT ((0)) NULL,
    [ID]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Status]           VARCHAR (15)   CONSTRAINT [DF_LineMapping_Status] DEFAULT ('') NULL,
    [IEReasonID]       VARCHAR (5)    CONSTRAINT [DF_LineMapping_IEReasonID] DEFAULT ('') NULL,
    [AddName]          VARCHAR (10)   CONSTRAINT [DF_LineMapping_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME       NULL,
    [EditName]         VARCHAR (10)   CONSTRAINT [DF_LineMapping_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME       NULL,
    [TimeStudyPhase]   VARCHAR(10)    CONSTRAINT [DF_LineMapping_TimeStudyPhase] DEFAULT ('') NOT NULL, 
    [TimeStudyVersion] VARCHAR(2)     CONSTRAINT [DF_LineMapping_TimeStudyVersion] DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_LineMapping] PRIMARY KEY CLUSTERED ([StyleID] ASC, [SeasonID] ASC, [BrandID] ASC, [StyleUKey] ASC, [Version] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Line Mapping', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'StyleUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'StyleUKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'套裝部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'Team';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計投入人數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'IdealOperators';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'現行投入人數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'CurrentOperators';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標準產出', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'StandardOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'一天產出件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'DailyDemand';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每日工作時數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'Workhour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Net available Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'NetTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每產出一件所需秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'TaktTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSD上的總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'TotalGSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全部Cycle的總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'TotalCycle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各站中最高的GSD秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'HighestGSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各站中最高的Cycle秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'HighestCycle';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Not hit target reason', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'IEReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD的 Phase',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyPhase'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fty GSD的 version',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'TimeStudyVersion'