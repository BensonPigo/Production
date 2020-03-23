
CREATE TABLE [dbo].[GarmentTest_Detail_Apperance](
	[ID] [bigint] NOT NULL,
	[No] [int] NOT NULL,
	[Type] [varchar](200) NOT NULL,
	[Wash1] [varchar](10) NOT NULL CONSTRAINT [DF_GarmentTest_Detail_Apperance_Wash1]  DEFAULT ('N/A'),
	[Wash2] [varchar](10) NOT NULL CONSTRAINT [DF_GarmentTest_Detail_Apperance_Wash2]  DEFAULT ('N/A'),
	[Wash3] [varchar](10) NOT NULL CONSTRAINT [DF_GarmentTest_Detail_Apperance_Wash3]  DEFAULT ('N/A'),
	[Comment] [nvarchar](100) NULL,
	[Seq] [int] NOT NULL,
    [Wash4] VARCHAR(10) NOT NULL DEFAULT ('N/A'), 
    [Wash5] VARCHAR(10) NOT NULL DEFAULT ('N/A'), 
    PRIMARY KEY CLUSTERED ([ID],[No],[Seq])
);
