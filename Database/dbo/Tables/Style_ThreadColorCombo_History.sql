
	CREATE TABLE [dbo].[Style_ThreadColorCombo_History](
		[StyleUkey] [bigint] NOT NULL,
		[Thread_ComboID] [varchar](10) NOT NULL,
		[MachineTypeID] [varchar](10) NOT NULL,
		[SeamLength] [numeric](8, 2) NULL,
		[ConsPC] [numeric](8, 2) NULL,
		[AddName] [varchar](10) NULL,
		[AddDate] [datetime] NULL,
		[EditName] [varchar](10) NULL,
		[EditDate] [datetime] NULL,
		[Ukey] [bigint] NOT NULL,
		[LockDate] [datetime] NOT NULL,
		[Category] [varchar](1) NULL,
		[TPDate] [date] NULL,
		[IETMSID_Thread] [varchar](10) NULL,
		[IETMSVersion_Thread] [varchar](3) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[StyleUkey] ASC,
		[LockDate] ASC,
		[Thread_ComboID] ASC,
		[MachineTypeID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]