CREATE TABLE [dbo].[DailyDataExchangeResult](
	[ExchangeDate] [date] NOT NULL,
	[Result] [bit] NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_DailyDataExchangeResult] PRIMARY KEY CLUSTERED 
(
	[ExchangeDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DailyDataExchangeResult] ADD  CONSTRAINT [DF_DailyDataExchangeResult_Result]  DEFAULT ((0)) FOR [Result]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每日資料交換日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DailyDataExchangeResult', @level2type=N'COLUMN',@level2name=N'ExchangeDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每日資料交換結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DailyDataExchangeResult', @level2type=N'COLUMN',@level2name=N'Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DailyDataExchangeResult', @level2type=N'COLUMN',@level2name=N'EditDate'
GO
