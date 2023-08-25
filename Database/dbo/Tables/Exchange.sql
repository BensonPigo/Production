CREATE TABLE [dbo].[Exchange](
	[ExchangeTypeID] [varchar](2) NOT NULL,
	[CurrencyFrom] [varchar](3) NOT NULL,
	[CurrencyTo] [varchar](3) NOT NULL,
	[DateStart] [date] NOT NULL,
	[DateEnd] [date] NOT NULL,
	[Rate] [numeric](9, 4) NOT NULL,
	[Remark] [nvarchar](600) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[Junk] [bit] NOT NULL,
 CONSTRAINT [PK_Exchange] PRIMARY KEY CLUSTERED 
(
	[ExchangeTypeID] ASC,
	[CurrencyFrom] ASC,
	[CurrencyTo] ASC,
	[DateStart] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_ExchangeTypeID]  DEFAULT ('') FOR [ExchangeTypeID]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_CurrencyFrom]  DEFAULT ('') FOR [CurrencyFrom]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_CurrencyTo]  DEFAULT ('') FOR [CurrencyTo]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_Rate]  DEFAULT ((0)) FOR [Rate]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[Exchange] ADD  CONSTRAINT [DF_Exchange_Junk]  DEFAULT ((0)) FOR [Junk]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率別(10:浮動 20:海關三旬)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'ExchangeTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'CurrencyFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'兌換幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'CurrencyTo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率日期(起)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'DateStart'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率日期(迄)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'DateEnd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'匯率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'Rate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幣別匯率明細檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Exchange'
GO


