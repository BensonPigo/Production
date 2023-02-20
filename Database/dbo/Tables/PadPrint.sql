CREATE TABLE [dbo].[PadPrint](
	[Ukey] [bigint] NOT NULL,
	[Refno] [varchar](23)　		CONSTRAINT [DF_PadPrint_Refno]  DEFAULT ('') NOT NULL,
	[BrandID] [varchar](8)　	CONSTRAINT [DF_PadPrint_BrandID]  DEFAULT ('') NOT NULL,
	[Category] [varchar](1)　	CONSTRAINT [DF_PadPrint_Category]  DEFAULT ('') NOT NULL,
	[SuppID] [varchar](6)		CONSTRAINT [DF_PadPrint_SuppID]  DEFAULT ('')　NOT NULL,
	[CurrencyID] [varchar](3)　	CONSTRAINT [DF_PadPrint_CurrencyID]  DEFAULT ('') NOT NULL,
	[Junk] [bit]　				CONSTRAINT [DF_PadPrint_Junk]  DEFAULT ((0)) NOT NULL,
	[Remark] [nvarchar](1000)　	CONSTRAINT [DF_PadPrint_Remark]  DEFAULT ('') NOT NULL,
	[AddName] [varchar](10)　	CONSTRAINT [DF_PadPrint_AddName]  DEFAULT ('') NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10)　	CONSTRAINT [DF_PadPrint_EditName]  DEFAULT ('') NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PadPrint_Ukey] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

