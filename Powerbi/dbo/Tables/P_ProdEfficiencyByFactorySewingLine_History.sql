	CREATE TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Factory] [varchar](8) Not NULL,
		[FtyZone] [varchar](8) Not NULL,
		[Line] [varchar](5) Not NULL,
		[Year-Month] [date] Not NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_ProdEfficiencyByFactorySewingLine_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine_History] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_History_Factory]  DEFAULT ('') FOR [Factory]
	ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine_History] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_History_FtyZone]  DEFAULT ('') FOR [FtyZone]
	ALTER TABLE [dbo].[P_ProdEfficiencyByFactorySewingLine_History] ADD  CONSTRAINT [DF_P_ProdEfficiencyByFactorySewingLine_History_Line]  DEFAULT ('') FOR [Line]