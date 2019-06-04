CREATE TABLE [dbo].[MachineType_ThreadRatio_Regular](
	[ID] [varchar](10) NOT NULL,
	[Seq] [varchar](2) NOT NULL,
	[UseRatioRule] [varchar](1) NOT NULL,
	[UseRatio] [numeric](5, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Seq] ASC,
	[UseRatioRule] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]