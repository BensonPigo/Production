
CREATE TABLE [dbo].[Style_QTThreadColorCombo_History_Detail](
	[Style_QTThreadColorCombo_HistoryUkey] [bigint] NOT NULL,
	[Seq] [varchar](2) NOT NULL,
	[SCIRefNo] [varchar](30) NULL,
	[SuppId] [varchar](6) NULL,
	[Article] [varchar](8) NOT NULL,
	[ColorID] [varchar](6) NULL,
	[SuppColor] [varchar](30) NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Ratio] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_Style_QTThreadColorCombo_History_Detail] PRIMARY KEY CLUSTERED 
(
	[Style_QTThreadColorCombo_HistoryUkey] ASC,
	[Seq] ASC,
	[Article] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


