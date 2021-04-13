
	CREATE TABLE [dbo].[Style_ThreadColorCombo_History_Detail](
		[Style_ThreadColorCombo_HistoryUkey] [bigint] NOT NULL,
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
		[Ukey] [bigint]  NOT NULL,
		[UseRatio] [numeric](5, 2) NULL,
		[Allowance] [numeric](4, 2) NULL,
		[AllowanceTubular] [numeric](4, 2) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Style_ThreadColorCombo_HistoryUkey] ASC,
		[Seq] ASC,
		[Article] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
