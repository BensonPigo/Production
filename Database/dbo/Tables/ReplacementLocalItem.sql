
CREATE TABLE [dbo].[ReplacementLocalItem](
	[ID] [varchar](13) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_ID]  DEFAULT (''),
	[IssueDate] [date] NOT NULL,
	[FabricType] [varchar](1) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_FabricType]  DEFAULT (''),
	[Type] [varchar](1) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_Type]  DEFAULT (''),
	[Shift] [varchar](1) NOT NULL CONSTRAINT [DF_ReplacementLocalItemShift]  DEFAULT (''),
	[MDivisionID] [varchar](8) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_MDivisionID]  DEFAULT (''),
	[FactoryID] [varchar](8) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_FactoryID]  DEFAULT (''),
	[OrderID] [varchar](13) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_OrderID]  DEFAULT (''),
	[POID] [varchar](13) NULL CONSTRAINT [DF_ReplacementLocalItem_POID]  DEFAULT (''),
	[SewingLineID] [varchar](2) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_SewingLineID]  DEFAULT (''),
	[Remark] [nvarchar](60) NULL CONSTRAINT [DF_ReplacementLocalItem_Remark]  DEFAULT (''),
	[ApplyName] [varchar](10) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_ApplyName]  DEFAULT (''),
	[ApvName] [varchar](10) NULL CONSTRAINT [DF_ReplacementLocalItem_ApvName]  DEFAULT (''),
	[ApvDate] [datetime] NULL,
	[AddName] [varchar](10) NULL CONSTRAINT [DF_ReplacementLocalItem_AddName]  DEFAULT (''),
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL CONSTRAINT [DF_ReplacementLocalItem_EditName]  DEFAULT (''),
	[EditDate] [datetime] NULL,
	[Status] [varchar](15) NULL CONSTRAINT [DF_ReplacementLocalItem_Status]  DEFAULT (''),
	[SubconName] [varchar](8) NULL DEFAULT (''),
	[Dept] [varchar](15) NULL CONSTRAINT [DF_ReplacementLocalItem_Dept]  DEFAULT (''),
 CONSTRAINT [PK_ReplacementLocalItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'IssueDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補料類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'ApplyName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'ApvName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'審核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'ApvDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem', @level2type=N'COLUMN',@level2name=N'Dept'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lacking Replacement' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem'
GO
