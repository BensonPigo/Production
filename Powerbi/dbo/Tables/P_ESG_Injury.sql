CREATE TABLE [dbo].[P_ESG_Injury](
	[ID] [varchar](13) NOT NULL,
	[FactoryID] [varchar](4) NOT NULL,
	[InjuryType] [varchar](15) NOT NULL,
	[CDate] [date] NULL,
	[LossHours] [numeric](4, 1) NOT NULL,
	[IncidentType] [varchar](50) NOT NULL,
	[IncidentRemark] [nvarchar](2000) NOT NULL,
	[SevereLevel] [varchar](50) NOT NULL,
	[SevereRemark] [nvarchar](2000) NOT NULL,
	[CAP] [nvarchar](2000) NOT NULL,
	[Incharge] [nvarchar](100) NOT NULL,
	[InchargeCheckDate] [date] NULL,
	[Approver] [nvarchar](100) NOT NULL,
	[ApproveDate] [date] NULL,
	[ProcessDate] [date] NULL,
	[ProcessTime] [time](0) NULL,
	[ProcessUpdate] [nvarchar](2000) NOT NULL,
	[Status] [varchar](15) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_ESG_Injury] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_InjuryType]  DEFAULT ('') FOR [InjuryType]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_LossHours]  DEFAULT ((0)) FOR [LossHours]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_IncidentType]  DEFAULT ('') FOR [IncidentType]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_IncidentRemark]  DEFAULT ('') FOR [IncidentRemark]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_SevereLevel]  DEFAULT ('') FOR [SevereLevel]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_SevereRemark]  DEFAULT ('') FOR [SevereRemark]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_CAP]  DEFAULT ('') FOR [CAP]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_Incharge]  DEFAULT ('') FOR [Incharge]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_Approver]  DEFAULT ('') FOR [Approver]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_ProcessUpdate]  DEFAULT ('') FOR [ProcessUpdate]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injurye_Status]  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[P_ESG_Injury] ADD  CONSTRAINT [DF_P_ESG_Injury_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工傷;勞安' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'InjuryType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'損失時數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'LossHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'IncidentType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事件描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'IncidentRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'嚴重程度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'SevereLevel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'嚴重程度描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'SevereRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Action plan執行方案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'CAP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'負責人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'Incharge'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'Approver'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ApproveDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'處理日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ProcessDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'處理時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ProcessTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'處理狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'ProcessUpdate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單據狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ESG_Injury', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO