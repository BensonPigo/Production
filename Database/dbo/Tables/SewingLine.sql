CREATE TABLE [dbo].[SewingLine] (
    [ID]                 VARCHAR (5)    CONSTRAINT [DF_SewingLine_ID] DEFAULT ('') NOT NULL,
    [Description]        NVARCHAR (500) CONSTRAINT [DF_SewingLine_Description] DEFAULT ('') NOT NULL,
    [FactoryID]          VARCHAR (8)    CONSTRAINT [DF_SewingLine_FactoryID] DEFAULT ('') NOT NULL,
    [SewingCell]         VARCHAR (2)    CONSTRAINT [DF_SewingLine_SewingCell] DEFAULT ('') NULL,
    [Sewer]              INT            CONSTRAINT [DF_SewingLine_Sewer] DEFAULT ((0)) NULL,
    [Junk]               BIT            CONSTRAINT [DF_SewingLine_Junk] DEFAULT ((0)) NULL,
    [AddName]            VARCHAR (10)   CONSTRAINT [DF_SewingLine_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME       NULL,
    [EditName]           VARCHAR (10)   CONSTRAINT [DF_SewingLine_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME       NULL,
    [LastInpsectionTime] DATETIME       NULL,
    [IdleTime]           INT            CONSTRAINT [DF_SewingLine_IdleTime] DEFAULT ((0)) NOT NULL,
    [LineGroup]          NVARCHAR (50)  CONSTRAINT [DF_SewingLine_LineGroup] DEFAULT ('') NULL,
    [DQSQtyPCT] SMALLINT NOT NULL, 
    [DQSQtyPCTEditName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [DQSQtyPCTEditDate] DATETIME NULL, 
    [DQSWFTPCT] numeric(3, 1) CONSTRAINT [DF_SewingLine_DQSWFTPCT] DEFAULT ((0)) NOT NULL,
    [LineNmforReport] VARCHAR(5) CONSTRAINT [DF_SewingLine_LineNmforReport] NOT NULL DEFAULT (('')), 
    [DQSTargetQty] INT NOT NULL DEFAULT ((0)), 
    [Outsourcing] BIT NOT NULL DEFAULT ((0)), 
	DashboardSource varchar(8) NOT NULL CONSTRAINT [DF_SewingLine_DashboardSource] DEFAULT 'DQS',
    CONSTRAINT [PK_SewingLine] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Line Index(車線產線基本檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'SewingCell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預設車縫人數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'Sewer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'EditDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不同區顯示只顯示此Group下的Line', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingLine', @level2type = N'COLUMN', @level2name = N'LineGroup';

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後檢驗時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingLine',
    @level2type = N'COLUMN',
    @level2name = N'LastInpsectionTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'閒置時間(分鐘)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingLine',
    @level2type = N'COLUMN',
    @level2name = N'IdleTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整WFT%',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingLine',
    @level2type = N'COLUMN',
    @level2name = N'DQSWFTPCT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'報表顯示用的LineNM',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingLine',
    @level2type = N'COLUMN',
    @level2name = N'LineNmforReport'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'DQSTargetQty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingLine',
    @level2type = N'COLUMN',
    @level2name = N'DQSTargetQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'外發廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingLine',
    @level2type = N'COLUMN',
    @level2name = N'Outsourcing'

	
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯入Dashboard的資料來源' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SewingLine', @level2type=N'COLUMN',@level2name=N'DashboardSource'
GO
