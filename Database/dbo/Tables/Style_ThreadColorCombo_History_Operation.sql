
	CREATE TABLE [dbo].[Style_ThreadColorCombo_History_Operation](
		[Style_ThreadColorCombo_HistoryUkey] [bigint] NOT NULL,
		[Seq] [varchar](4) NOT NULL,
		[OperationID] [varchar](20) NOT NULL,
		[ComboType] [varchar](1) NULL,
		[Frequency] [numeric](7, 2) NULL,
		[AddName] [varchar](10) NULL,
		[AddDate] [datetime] NULL,
		[EditName] [varchar](10) NULL,
		[EditDate] [datetime] NULL,
		[Ukey] [bigint]  NOT NULL,
	[MachineTypeHem] BIT NULL, 
    [OperationHem] BIT NULL, 
    [Tubular] BIT NULL, 
    [Segment] INT NULL, 
    [SeamLength] NUMERIC(9, 2) NULL, 
    PRIMARY KEY CLUSTERED 
	(
		[Style_ThreadColorCombo_HistoryUkey] ASC,
		[Seq] ASC,
		[OperationID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]