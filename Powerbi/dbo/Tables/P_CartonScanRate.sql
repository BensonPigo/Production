﻿CREATE TABLE [dbo].[P_CartonScanRate](
	[Date] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[HaulingScanRate] [numeric](5, 2) NOT NULL,
	[PackingAuditScanRate] [numeric](5, 2) NOT NULL,
	[MDScanRate] [numeric](5, 2) NOT NULL,
	[ScanAndPackRate] [numeric](5, 2) NOT NULL,
	[PullOutRate] [numeric](5, 2) NOT NULL,
	[ClogReceivedRate] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_P_CartonScanRate] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_CartonScanRate] ADD  CONSTRAINT [DF_P_CartonScanRate_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_CartonScanRate] ADD  CONSTRAINT [DF_P_CartonScanRate_HaulingScanRate]  DEFAULT ((0)) FOR [HaulingScanRate]
GO

ALTER TABLE [dbo].[P_CartonScanRate] ADD  CONSTRAINT [DF_P_CartonScanRate_PackingAuditScanRate]  DEFAULT ((0)) FOR [PackingAuditScanRate]
GO

ALTER TABLE [dbo].[P_CartonScanRate] ADD  CONSTRAINT [DF_P_CartonScanRate_MDScanRate]  DEFAULT ((0)) FOR [MDScanRate]
GO

ALTER TABLE [dbo].[P_CartonScanRate] ADD  CONSTRAINT [DF_P_CartonScanRate_ScanAndPackRate]  DEFAULT ((0)) FOR [ScanAndPackRate]
GO

ALTER TABLE [dbo].[P_CartonScanRate] ADD  CONSTRAINT [DF_P_CartonScanRate_PullOutRate]  DEFAULT ((0)) FOR [PullOutRate]
GO

ALTER TABLE [dbo].[P_CartonScanRate] ADD  CONSTRAINT [DF_P_CartonScanRate_ClogReceivedRate]  DEFAULT ((0)) FOR [ClogReceivedRate]
GO
