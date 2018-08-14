CREATE TABLE [dbo].[SMNotice_Detail] (
	[ID] [varchar](10) NOT NULL CONSTRAINT [DF_SMNotice_Detail_ID]  DEFAULT (''),
	[Type] [varchar](1) NOT NULL CONSTRAINT [DF_SMNotice_Detail_Type]  DEFAULT (''),
	[UseFor] [varchar](1) NOT NULL CONSTRAINT [DF_SMNotice_Detail_UseFor]  DEFAULT (''),
	[PhaseID] [varchar](10) NOT NULL CONSTRAINT [DF_SMNotice_Detail_PhaseID]  DEFAULT (''),
	[RequireDate] [date] NULL,
	[Apv2SampleTime] [datetime] NULL,
	[Apv2SampleHandle] [varchar](10) NULL CONSTRAINT [DF_SMNotice_Detail_Apv2SampleHandle]  DEFAULT (''),
	[ApvName] [varchar](10) NULL CONSTRAINT [DF_SMNotice_Detail_ApvName]  DEFAULT (''),
	[ApvDate] [datetime] NULL,
	[Factory] [varchar](8) NULL CONSTRAINT [DF_SMNotice_Detail_Factory]  DEFAULT (''),
	[IEConfirmMR] [varchar](10) NULL CONSTRAINT [DF_SMNotice_Detail_IEConfirmMR]  DEFAULT (''),
	[PendingStatus] [bit] NULL CONSTRAINT [DF_SMNotice_Detail_PendingStatus]  DEFAULT ((0)),
	[BasicPattern] [nvarchar](100) NULL CONSTRAINT [DF_SMNotice_Detail_BasicPattern]  DEFAULT (''),
	[Remark1] [nvarchar](200) NULL CONSTRAINT [DF_SMNotice_Detail_Remark1]  DEFAULT (''),
	[Remark2] [nvarchar](200) NULL CONSTRAINT [DF_SMNotice_Detail_Remark2]  DEFAULT (''),
	[AddName] [varchar](10) NULL CONSTRAINT [DF_SMNotice_Detail_AddName]  DEFAULT (''),
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL CONSTRAINT [DF_SMNotice_Detail_EditName]  DEFAULT (''),
	[EditDate] [datetime] NULL,
    CONSTRAINT [PK_SMNotice_Detail] PRIMARY KEY CLUSTERED ([ID] ASC,[Type] ASC)
);



GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'申請單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'申請項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'用途(Bulk/Costing)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'UseFor'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'樣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'PhaseID'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'希望完成日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'RequireDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'Approve to Sample Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Apv2SampleTime'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'Appprove to Sample Handler' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Apv2SampleHandle'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'審核人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'ApvName'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'審核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'ApvDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'指定工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Factory'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'允許核准IEGSD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'IEConfirmMR'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'Pending Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'PendingStatus'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'基本版' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'BasicPattern'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'進單備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Remark1'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'進單備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Remark2'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'AddName'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'AddDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'EditName'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'EditDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'樣衣製作申請_明細檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail'
GO