CREATE TABLE [dbo].[Brand_ThreadCalculateRules](
	[ID] [varchar](8) NOT NULL,
	[FabricType] [varchar](5) NOT NULL,
	[UseRatioRule] [varchar](1) NULL,
	[UseRatioRule_Thick] [varchar](1) NULL,
	[ProgramID] [varchar](12) NOT NULL CONSTRAINT [DF_Brand_ThreadCalculateRules_ProgramID]  DEFAULT (''),
PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[FabricType] ASC,
	[ProgramID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
