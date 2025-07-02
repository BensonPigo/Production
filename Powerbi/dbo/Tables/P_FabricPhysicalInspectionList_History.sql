CREATE TABLE [dbo].[P_FabricPhysicalInspectionList_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[SEQ] [varchar](6) NOT NULL,
	[ReceivingID] [varchar](13) NOT NULL,
	[Roll] [varchar](8) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_FabricPhysicalInspectionList_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO