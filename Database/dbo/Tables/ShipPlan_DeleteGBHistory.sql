CREATE TABLE [dbo].[ShipPlan_DeleteGBHistory](
	[ID] [varchar](13) NOT NULL,
	[GMTBookingID] [varchar](25) NOT NULL,
	[ReasonID] [varchar](5) NOT NULL,
	[BackDate] [date] NULL,
	[NewShipModeID] [varchar](10) NOT NULL,
	[NewPulloutDate] [date] NULL,
	[NewDestination] [nvarchar](80) NOT NULL,
	[Remark] [nvarchar](80) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
 CONSTRAINT [PK_ShipPlan_DeleteGBHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[GMTBookingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShipPlan_DeleteGBHistory] ADD  CONSTRAINT [DF_ShipPlan_DeleteGBHistory_ReasonID]  DEFAULT ('') FOR [ReasonID]
GO

ALTER TABLE [dbo].[ShipPlan_DeleteGBHistory] ADD  CONSTRAINT [DF_ShipPlan_DeleteGBHistory_NewShipModeID]  DEFAULT ('') FOR [NewShipModeID]
GO

ALTER TABLE [dbo].[ShipPlan_DeleteGBHistory] ADD  CONSTRAINT [DF_ShipPlan_DeleteGBHistory_NewDestination]  DEFAULT ('') FOR [NewDestination]
GO

ALTER TABLE [dbo].[ShipPlan_DeleteGBHistory] ADD  CONSTRAINT [DF_ShipPlan_DeleteGBHistory_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[ShipPlan_DeleteGBHistory] ADD  CONSTRAINT [DF_ShipPlan_DeleteGBHistory_AddName]  DEFAULT ('') FOR [AddName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ShipPlan ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment Booking  ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'GMTBookingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原因ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'ReasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'退回日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'BackDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新的出貨方式主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'NewShipModeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新的出貨日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'NewPulloutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新的明細說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'NewDestination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'刪除人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'刪除時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShipPlan_DeleteGBHistory', @level2type=N'COLUMN',@level2name=N'AddDate'
GO