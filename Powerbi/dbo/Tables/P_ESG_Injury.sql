CREATE TABLE [dbo].[P_ESG_Injury]
(
	[ID]					varchar(13)       CONSTRAINT [DF_P_ESG_Injurye_ID]              DEFAULT ((''))      NOT NULL,
	[FactoryID]				varchar(4)        CONSTRAINT [DF_P_ESG_Injurye_FactoryID]       DEFAULT ((''))      NOT NULL,
	[InjuryType]					varchar(15)       CONSTRAINT [DF_P_ESG_Injurye_InjuryType]            DEFAULT ((''))      NOT NULL,
	[CDate]					date                                                                                NULL,
	[LossHours]				numeric(4, 1)     CONSTRAINT [DF_P_ESG_Injurye_LossHours]       DEFAULT ((0))       NOT NULL,
	[IncidentType]			varchar(50)       CONSTRAINT [DF_P_ESG_Injurye_IncidentType]    DEFAULT ((''))      NOT NULL,
	[IncidentRemark]		nvarchar(2000)    CONSTRAINT [DF_P_ESG_Injurye_IncidentRemark]  DEFAULT ((''))      NOT NULL,
	[SevereLevel]			varchar(50)       CONSTRAINT [DF_P_ESG_Injurye_SevereLevel]     DEFAULT ((''))      NOT NULL,
	[SevereRemark]			nvarchar(2000)    CONSTRAINT [DF_P_ESG_Injurye_SevereRemark]    DEFAULT ((''))      NOT NULL,
	[CAP]					nvarchar(2000)    CONSTRAINT [DF_P_ESG_Injurye_CAP]             DEFAULT ((''))      NOT NULL,
	[Incharge]				nvarchar(100)     CONSTRAINT [DF_P_ESG_Injurye_Incharge]        DEFAULT ((''))      NOT NULL,
	[InchargeCheckDate]		date                                                                                NULL,
	[Approver]				nvarchar(100)     CONSTRAINT [DF_P_ESG_Injurye_Approver]        DEFAULT ((''))      NOT NULL,
	[ApproveDate]			date                                                                                NULL,
	[ProcessDate]			date                                                                                NULL,
	[ProcessTime]			time(0)                                                                             NULL,
	[ProcessUpdate]			nvarchar(2000)    CONSTRAINT [DF_P_ESG_Injurye_ProcessUpdate]   DEFAULT ((''))      NOT NULL,
	[Status]				varchar(15)       CONSTRAINT [DF_P_ESG_Injurye_Status]          DEFAULT ((''))      NOT NULL, 
    CONSTRAINT [PK_P_ESG_Injury] PRIMARY KEY ([ID] ASC, [FactoryID] ASC),
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單據狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'處理狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'ProcessUpdate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'處理時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'ProcessTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'處理日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'ProcessDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'審核者',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'Approver'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'審核日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'ApproveDate'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'負責人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'Incharge'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Action plan執行方案',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'CAP'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'嚴重程度描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'SevereRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'嚴重程度',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'SevereLevel'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'事件描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'IncidentRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'事件種類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'IncidentType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'損失時數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'LossHours'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工傷;勞安',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'InjuryType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ESG_Injury',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'