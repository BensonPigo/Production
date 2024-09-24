CREATE TABLE [dbo].[P_MachineMasterList](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Month] [varchar](6) NULL,
	[MachineID] [varchar](16) NULL,
	[M] [varchar](8) NULL,
	[FTY] [varchar](8) NULL,
	[MachineLocationID] [varchar](10) NULL,
	[MachineGroup] [nvarchar](200) NULL,
	[BrandID] [varchar](10) NULL,
	[BrandName] [varchar](50) NULL,
	[Model] [varchar](50) NULL,
	[SerialNo] [varchar](20) NULL,
	[Condition] [varchar](10) NULL,
	[PendingCountryMangerApvDate] [date] NULL,
	[RepairStartDate] [date] NULL,
	[EstFinishRepairDate] [date] NULL,
	[MachineArrivalDate] [date] NULL,
	[Obtained Date] [date] NULL,
	[TransferDate] [date] NULL,
	[LendTo] [varchar](25) NULL,
	[LendDate] [date] NULL,
	[LastEstReturnDate] [date] NULL,
	[Remark] [nvarchar](100) NULL,
	[FAID] [varchar](21) NULL,
	[Junk] [varchar](1) NULL,
	[POID] [varchar](13) NULL,
	[RefNo] [varchar](23) NULL,
 CONSTRAINT [PK_P_MachineMasterList] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_Month]  DEFAULT ('') FOR [Month]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_MachineID]  DEFAULT ('') FOR [MachineID]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_M]  DEFAULT ('') FOR [M]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_FTY]  DEFAULT ('') FOR [FTY]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_MachineLocationID]  DEFAULT ('') FOR [MachineLocationID]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_MachineGroup]  DEFAULT ('') FOR [MachineGroup]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_BrandName]  DEFAULT ('') FOR [BrandName]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_Model]  DEFAULT ('') FOR [Model]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_SerialNo]  DEFAULT ('') FOR [SerialNo]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_Condition]  DEFAULT ('') FOR [Condition]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_LendTo]  DEFAULT ('') FOR [LendTo]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_FAID]  DEFAULT ('') FOR [FAID]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_Junk]  DEFAULT ('') FOR [Junk]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_MachineMasterList] ADD  CONSTRAINT [DF_P_MachineMasterList_RefNo]  DEFAULT ('') FOR [RefNo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報表月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'Month'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機器編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'MachineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目前位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'MachineLocationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'待Country Mgr審核日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'PendingCountryMangerApvDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機器借給誰' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'LendTo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機器借出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'LendDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計歸還日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'LastEstReturnDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'固資編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterList', @level2type=N'COLUMN',@level2name=N'FAID'
GO
