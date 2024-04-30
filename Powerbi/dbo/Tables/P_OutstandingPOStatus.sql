CREATE TABLE [dbo].[P_OutstandingPOStatus](
	[Buyerdelivery] [date] NOT NULL,
	[FTYGroup] [varchar](3) NOT NULL,
	[TotalCMPQty] [int] NOT NULL,
	[TotalClogCtn] [int] NOT NULL,
	[NotYet3rdSPCount] [int] NOT NULL,
 CONSTRAINT [PK_P_OutstandingPOStatus] PRIMARY KEY CLUSTERED 
(
	[Buyerdelivery] ASC,
	[FTYGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_OutstandingPOStatus] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_TotalCMPQty]  DEFAULT ((0)) FOR [TotalCMPQty]
GO

ALTER TABLE [dbo].[P_OutstandingPOStatus] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_TotalClogCtn]  DEFAULT ((0)) FOR [TotalClogCtn]
GO

ALTER TABLE [dbo].[P_OutstandingPOStatus] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_NotYet3rdSPCount]  DEFAULT ((0)) FOR [NotYet3rdSPCount]
GO