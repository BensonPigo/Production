CREATE TABLE [dbo].[FirstSaleCostSetting](
	[CountryID] [varchar](2) NOT NULL CONSTRAINT [DF_FirstSaleCostSetting_CountryID]  DEFAULT (''),
	[ArtWorkID] [varchar](20) NOT NULL CONSTRAINT [DF_FirstSaleCostSetting_ArtWorkID]  DEFAULT (''),
	[CostTypeID] [varchar](11) NOT NULL CONSTRAINT [DF_FirstSaleCostSetting_CostTypeID]  DEFAULT (''),
	[BeginDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IsJunk] [bit] NULL CONSTRAINT [DF_FirstSaleCostSetting_IsJunk]  DEFAULT ((0)),
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10) NULL CONSTRAINT [DF_FirstSaleCostSetting_AddName]  DEFAULT (''),
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10) NULL CONSTRAINT [DF_FirstSaleCostSetting_EditName]  DEFAULT (''),
 CONSTRAINT [PK_FirstSaleCostSetting] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC,
	[ArtWorkID] ASC,
	[CostTypeID] ASC,
	[BeginDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國別代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'CountryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'ArtWorkID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'CostTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起始日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'迄止日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'EndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'IsJunk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First sale Cost Setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting'
GO
