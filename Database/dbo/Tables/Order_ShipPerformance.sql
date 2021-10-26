CREATE TABLE [dbo].[Order_ShipPerformance](
	[ID] [varchar](13) NOT NULL,
	[Seq] [varchar](2) NOT NULL,
	[BookDate] [date] NULL,
	[PKManifestCreateDate] [date] NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Order_ShipPerformance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

