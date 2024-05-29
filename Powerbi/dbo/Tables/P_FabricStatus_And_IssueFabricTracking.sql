﻿CREATE TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking](
	[SewingCell] [varchar](2) NOT NULL,
	[LineID] [varchar](5) NOT NULL,
	[Department] [varchar](15) not null DEFAULT (''),
	[ReplacementID] [varchar](13) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[Seq] [varchar](6) NOT NULL,
	[FabricType] [varchar](1) NOT NULL,
	[Color] [nvarchar](150) NOT NULL,
	[RefNo] [varchar](36) NOT NULL,
	[ApvDate] [datetime] NULL,
	[NoOfPcsRejected] [int] NOT NULL,
	[RequestQtyYrds] [numeric](10, 2) NOT NULL,
	[IssueQtyYrds] [numeric](10, 2) NOT NULL,
	[ReplacementFinishedDate] [datetime] NULL,
	[Type] [varchar](11) NOT NULL,
	[Process] [varchar](30) NOT NULL,
	[Description] [nvarchar](60) NOT NULL,
	[OnTime] [varchar](1) NOT NULL,
	[Remark] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_P_FabricStatus_And_IssueFabricTracking] PRIMARY KEY CLUSTERED 
(
	[ReplacementID] ASC,
	[SP] ASC,
	[Seq] ASC,
	[RefNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SewingCell]  DEFAULT ('') FOR [SewingCell]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_LineID]  DEFAULT ('') FOR [LineID]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_ReplacementID]  DEFAULT ('') FOR [ReplacementID]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SP]  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_FabricType]  DEFAULT ('') FOR [FabricType]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_RefNo]  DEFAULT ('') FOR [RefNo]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_NoOfPcsRejected]  DEFAULT ((0)) FOR [NoOfPcsRejected]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_RequestQtyYrds]  DEFAULT ((0)) FOR [RequestQtyYrds]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_IssueQtyYrds]  DEFAULT ((0)) FOR [IssueQtyYrds]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Process]  DEFAULT ('') FOR [Process]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_OnTime]  DEFAULT ('') FOR [OnTime]
GO

ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Remark]  DEFAULT ('') FOR [Remark]
GO
