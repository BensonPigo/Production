		CREATE TABLE [dbo].[Style_ThreadColorCombo_History_Version](
			[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
			[StyleUkey] [bigint] NULL,
			[Version] [varchar](5) NULL,
			[UseRatioRule] [varchar](1) NULL,
			[ThickFabricBulk] [bit] NULL,
			[FarmOutQuilting] [bit] NULL,
			[LockHandle] [varchar](10) NULL,
			[LockDate] [datetime] NULL,
			[Category] [varchar](1) NULL,
			[TPDate] [date] NULL,
			[IETMSID_Thread] [varchar](10) NULL,
			[IETMSVersion_Thread] [varchar](3) NULL,
			[AddName] [varchar](10) NULL,
			[AddDate] [datetime] NULL,
            [VersionCOO] VARCHAR (2) CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_VersionCOO] DEFAULT ('') NOT NULL,
		 CONSTRAINT [PK_Style_ThreadColorCombo_History_Version] PRIMARY KEY CLUSTERED 
		(
			[Ukey] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
