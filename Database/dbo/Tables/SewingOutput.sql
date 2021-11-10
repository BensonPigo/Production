CREATE TABLE [dbo].[SewingOutput] (
    [ID]                      VARCHAR (13)   CONSTRAINT [DF_SewingOutput_ID] DEFAULT ('') NOT NULL,
    [OutputDate]              DATE           NOT NULL,
    [SewingLineID]            VARCHAR (5)    CONSTRAINT [DF_SewingOutput_SewingLineID] DEFAULT ('') NULL,
    [QAQty]                   INT            CONSTRAINT [DF_SewingOutput_QAQty] DEFAULT ((0)) NULL,
    [DefectQty]               INT            CONSTRAINT [DF_SewingOutput_DefectQty] DEFAULT ((0)) NULL,
    [InlineQty]               INT            CONSTRAINT [DF_SewingOutput_InlineQty] DEFAULT ((0)) NULL,
    [TMS]                     INT            CONSTRAINT [DF_SewingOutput_TMS] DEFAULT ((0)) NULL,
    [Manpower]                NUMERIC (4, 1) CONSTRAINT [DF_SewingOutput_Manpower] DEFAULT ((0)) NULL,
    [ManHour]                 NUMERIC (9, 3) CONSTRAINT [DF_SewingOutput_ManHour] DEFAULT ((0)) NULL,
    [Efficiency]              NUMERIC (6, 1) CONSTRAINT [DF_SewingOutput_Efficiency] DEFAULT ((0)) NULL,
    [Shift]                   VARCHAR (1)    CONSTRAINT [DF_SewingOutput_Shift] DEFAULT ('') NULL,
    [Team]                    VARCHAR (1)    CONSTRAINT [DF_SewingOutput_Team] DEFAULT ('') NULL,
    [Status]                  VARCHAR (15)   CONSTRAINT [DF_SewingOutput_Status] DEFAULT ('') NULL,
    [LockDate]                DATE           NULL,
    [WorkHour]                NUMERIC (6, 2) CONSTRAINT [DF_SewingOutput_WorkHour] DEFAULT ((0)) NULL,
    [FactoryID]               VARCHAR (8)    CONSTRAINT [DF_SewingOutput_FactoryID] DEFAULT ('') NULL,
    [MDivisionID]             VARCHAR (8)    CONSTRAINT [DF_SewingOutput_MDivisionID] DEFAULT ('') NULL,
    [Category]                VARCHAR (1)    CONSTRAINT [DF_SewingOutput_Category] DEFAULT ('') NULL,
    [SFCData]                 BIT            CONSTRAINT [DF_SewingOutput_SFCData] DEFAULT ((0)) NULL,
    [AddName]                 VARCHAR (10)   CONSTRAINT [DF_SewingOutput_AddName] DEFAULT ('') NULL,
    [AddDate]                 DATETIME       NULL,
    [EditName]                VARCHAR (10)   CONSTRAINT [DF_SewingOutput_EditName] DEFAULT ('') NULL,
    [EditDate]                DATETIME       NULL,
    [SubconOutFty]            VARCHAR (8)    DEFAULT ('') NULL,
    [SubConOutContractNumber] VARCHAR (50)   CONSTRAINT [DF_SewingOutput_SubConOutContractNumber] DEFAULT ('') NULL,
    [ReDailyTransferDate]     DATE           NULL,
    CONSTRAINT [PK_SewingOutput] PRIMARY KEY CLUSTERED ([ID] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Dailiy output(車縫日報主檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'OutputDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總生產數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'QAQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總瑕疵數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'DefectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總上線數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'InlineQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Costing TMS (sec)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'TMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'投入人數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'Manpower';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'投入時數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'ManHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'效率(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'Efficiency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'Shift';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'Team';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Record Lock date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'LockDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總時數/天', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'WorkHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order or Mockup order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Batch import from SFC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'SFCData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
CREATE NONCLUSTERED INDEX [FACTORY]
    ON [dbo].[SewingOutput]([FactoryID] ASC);


GO
CREATE NONCLUSTERED INDEX [OpD]
    ON [dbo].[SewingOutput]([OutputDate] ASC);


GO
CREATE NONCLUSTERED INDEX [line]
    ON [dbo].[SewingOutput]([SewingLineID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_SewingOutput_OutputDate]
	ON [dbo].[SewingOutput] ([OutputDate])
	INCLUDE ([ID],[Shift],[FactoryID],[MDivisionID],[Category],[SubconOutFty],[SubConOutContractNumber])
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ݭ��s��ƥ洫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput', @level2type = N'COLUMN', @level2name = N'ReDailyTransferDate';

