CREATE TABLE [dbo].[Style_UnitPrice](
	[StyleUkey] [bigint] NOT NULL,
	[Ukey] [bigint] NOT NULL,
	[CountryID] [varchar](2) NOT NULL,
	[CurrencyID] [varchar](3) NOT NULL,
	[PoPrice] [numeric](16, 4) NOT NULL,
	[QuotCost] [numeric](16, 4) NOT NULL,
	[CustCDID] [varchar](16) NULL,
	[DestPrice] [numeric](16, 4) NULL,
	[OriginalPrice] [numeric](16, 4) NOT NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[StyleUkey_Old] [varchar](10) NULL,
	[COO] [varchar](2) NULL,
	[FactoryID] [varchar](8) NULL,
 CONSTRAINT [PK_Style_UnitPrice] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
go

ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_StyleUkey]  DEFAULT ((0)) FOR [StyleUkey]
go


ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_CountryID]  DEFAULT ('') FOR [CountryID]
go


ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_CurrencyID]  DEFAULT ('') FOR [CurrencyID]
go


ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_PoPrice]  DEFAULT ((0)) FOR [PoPrice]
go


ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_QuotCost]  DEFAULT ((0)) FOR [QuotCost]
go


ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_CustCDID]  DEFAULT ('') FOR [CustCDID]
go


ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_DestPrice]  DEFAULT ((0)) FOR [DestPrice]
go

ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_OriginalPrice]  DEFAULT ((0)) FOR [OriginalPrice]
go

ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_AddName]  DEFAULT ('') FOR [AddName]
go

ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_EditName]  DEFAULT ('') FOR [EditName]
go

ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_StyleUkey_Old]  DEFAULT ('') FOR [StyleUkey_Old]
go

ALTER TABLE [dbo].[Style_UnitPrice] ADD  DEFAULT ('') FOR [COO]
go

ALTER TABLE [dbo].[Style_UnitPrice] ADD  CONSTRAINT [DF_Style_UnitPrice_FactoryID]  DEFAULT ('') FOR [FactoryID]
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式的唯一值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'StyleUkey'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'唯一值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'Ukey'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國家' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'CountryID'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨幣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'CurrencyID'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'PoPrice'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報價成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'QuotCost'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶資料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'CustCDID'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目的地價格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'DestPrice'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'OriginalPrice'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'AddName'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建立時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'AddDate'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'EditName'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'EditDate'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'舊系統的款式的唯一值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'StyleUkey_Old'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產國家' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'COO'
go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_UnitPrice', @level2type=N'COLUMN',@level2name=N'FactoryID'
go