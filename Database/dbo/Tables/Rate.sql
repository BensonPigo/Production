CREATE TABLE [dbo].[Rate](
	[RateTypeID] [varchar](2) NOT NULL,
	[BeginDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[OriginalCurrency] [varchar](3) NOT NULL,
	[ExchangeCurrency] [varchar](3) NOT NULL,
	[Rate] [decimal](18, 8) NOT NULL,
	[Remark] [nvarchar](1000) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Rate] PRIMARY KEY CLUSTERED 
(
	[RateTypeID] ASC,
	[BeginDate] ASC,
	[OriginalCurrency] ASC,
	[ExchangeCurrency] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Rate] ADD  CONSTRAINT [DF_Rate_RateTypeID]  DEFAULT ('') FOR [RateTypeID]
GO

ALTER TABLE [dbo].[Rate] ADD  CONSTRAINT [DF_Rate_OriginalCurrency]  DEFAULT ('') FOR [OriginalCurrency]
GO

ALTER TABLE [dbo].[Rate] ADD  CONSTRAINT [DF_Rate_ExchangeCurrency]  DEFAULT ('') FOR [ExchangeCurrency]
GO

ALTER TABLE [dbo].[Rate] ADD  CONSTRAINT [DF_Rate_Rate]  DEFAULT ((0)) FOR [Rate]
GO

ALTER TABLE [dbo].[Rate] ADD  CONSTRAINT [DF_Rate_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[Rate] ADD  CONSTRAINT [DF_Rate_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Rate] ADD  CONSTRAINT [DF_Rate_EditName]  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'RateTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率起始日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率結束日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'EndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉換前幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'OriginalCurrency'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉換後幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'ExchangeCurrency'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'Rate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Rate', @level2type=N'COLUMN',@level2name=N'EditDate'
GO


