CREATE TABLE [dbo].[P_CartonStatusTrackingList](
	[KPIGroup] [varchar](8) NOT NULL,
	[Fty] [varchar](8) NOT NULL,
	[Line] [varchar](180) NOT NULL,
	[SP] [varchar](13) NOT NULL,
	[SeqNo] [varchar](2) NOT NULL,
	[Category] [varchar](7) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[PONO] [varchar](30) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[Destination] [varchar](2) NOT NULL,
	[SCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[PackingListID] [varchar](13) NOT NULL,
	[CtnNo] [varchar](6) NOT NULL,
	[Size] [varchar](180) NOT NULL,
	[CartonQty] [int] NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[HaulingScanTime] [datetime] NULL,
	[HauledQty] [int] NOT NULL,
	[DryRoomReceiveTime] [datetime] NULL,
	[DryRoomTransferTime] [datetime] NULL,
	[MDScanTime] [datetime] NULL,
	[MDFailQty] [int] NOT NULL,
	[PackingAuditScanTime] [datetime] NULL,
	[PackingAuditFailQty] [int] NOT NULL,
	[M360MDScanTime] [datetime] NULL,
	[M360MDFailQty] [int] NOT NULL,
	[TransferToPackingErrorTime] [datetime] NULL,
	[ConfirmPackingErrorReviseTime] [datetime] NULL,
	[ScanAndPackTime] [datetime] NULL,
	[ScanQty] [int] NOT NULL,
	[FtyTransferToClogTime] [datetime] NULL,
	[ClogReceiveTime] [datetime] NULL,
	[ClogLocation] [nvarchar](50) NOT NULL,
	[ClogReturnTime] [datetime] NULL,
	[ClogTransferToCFATime] [datetime] NULL,
	[CFAReceiveTime] [datetime] NULL,
	[CFAReturnTime] [datetime] NULL,
	[CFAReturnDestination] [varchar](7) NOT NULL,
	[ClogReceiveFromCFATime] [datetime] NULL,
	[DisposeDate] [date] NULL,
	[PulloutComplete] [varchar](1) NOT NULL,
	[PulloutDate] [date] NULL,
    [MDMachineNo] VARCHAR(30) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_P_CartonStatusTrackingList] PRIMARY KEY CLUSTERED 
	[RefNo] [varchar](21) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[HaulingStatus] [varchar](10) NOT NULL,
	[HaulerName] [varchar](50) NOT NULL,
	[PackingAuditStatus] [varchar](10) NOT NULL,
	[PackingAuditName] [varchar](50) NOT NULL,
	[M360MDStatus] [varchar](10) NOT NULL,
	[M360MDName] [varchar](50) NOT NULL,
	[HangerPackScanTime] [datetime] NULL,
	[HangerPackStatus] [varchar](10) NOT NULL,
	[HangerPackName] [varchar](50) NOT NULL,
	[JokerTagScanTime] [datetime] NULL,
	[JokerTagStatus] [varchar](10) NOT NULL,
	[JokerTagName] [varchar](50) NOT NULL,
	[HeatSealScanTime] [datetime] NULL,
	[HeatSealStatus] [varchar](10) NOT NULL,
	[HeatSealName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_P_CartonStatusTrackingList] PRIMARY KEY CLUSTERED 
(
	[SP] ASC,
	[SeqNo] ASC,
	[PackingListID] ASC,
	[CtnNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_KPIGroup]  DEFAULT ('') FOR [KPIGroup]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Fty]  DEFAULT ('') FOR [Fty]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_SP]  DEFAULT ('') FOR [SP]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_SeqNo]  DEFAULT ('') FOR [SeqNo]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_PONO]  DEFAULT ('') FOR [PONO]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Destination]  DEFAULT ('') FOR [Destination]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_PackingListID]  DEFAULT ('') FOR [PackingListID]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_CtnNo]  DEFAULT ('') FOR [CtnNo]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_Size]  DEFAULT ('') FOR [Size]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_CartonQty]  DEFAULT ((0)) FOR [CartonQty]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_MDMachineNo]  DEFAULT ((0)) FOR [MDMachineNo]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_HauledQty]  DEFAULT ((0)) FOR [HauledQty]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_MDFailQty]  DEFAULT ((0)) FOR [MDFailQty]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_PackingAuditFailQty]  DEFAULT ((0)) FOR [PackingAuditFailQty]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_M360MDFailQty]  DEFAULT ((0)) FOR [M360MDFailQty]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_ScanQty]  DEFAULT ((0)) FOR [ScanQty]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_ClogLocation]  DEFAULT ('') FOR [ClogLocation]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_CFAReturnDestination]  DEFAULT ('') FOR [CFAReturnDestination]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  CONSTRAINT [DF_P_CartonStatusTrackingList_PulloutComplete]  DEFAULT ('') FOR [PulloutComplete]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [RefNo]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [HaulingStatus]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [HaulerName]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [PackingAuditStatus]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [PackingAuditName]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [M360MDStatus]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [M360MDName]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [HangerPackStatus]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [HangerPackName]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [JokerTagStatus]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [JokerTagName]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [HeatSealStatus]
GO

ALTER TABLE [dbo].[P_CartonStatusTrackingList] ADD  DEFAULT ('') FOR [HeatSealName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SDP KPI Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'KPIGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Fty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD機器代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'MDMachineNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipment Seq' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'SeqNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Category' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Brand' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cust PONo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PONO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Season' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Destination' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Destination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'SCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Buyer Delivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing List #' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PackingListID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Carton#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'CtnNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ctn Size' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Size'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Qty per Carton' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'CartonQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Carton Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hauling Scan Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HaulingScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hauled Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HauledQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Sewing/P06' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'DryRoomReceiveTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Sewing/P12' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'DryRoomTransferTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Sewing/P08' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'MDScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD Scan discrepancy qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'MDFailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Packing/P29' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PackingAuditScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the discrepancy qty in Packing/P29 with the last audit time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PackingAuditFailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the scan time in Packing/P09 with data remark = ''Create from M360'' and the last scan time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'M360MDScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the discrepancy qty  in Packing/P09 with data remark = ''Create from M360'' and the last scan time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'M360MDFailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Packing/P19' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'TransferToPackingErrorTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Packing/P20' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ConfirmPackingErrorReviseTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Packing/P18' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ScanAndPackTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Complete Scan & Pack qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ScanQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Packing/P10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'FtyTransferToClogTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Clog/P02' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ClogReceiveTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The current location in Clog' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ClogLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Clog/P03' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ClogReturnTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Clog/P07' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ClogTransferToCFATime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in QA/P23' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'CFAReceiveTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in QA/P24' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'CFAReturnTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA can return to Clog or Fty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'CFAReturnDestination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Show the last scan time in Clog/P08' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'ClogReceiveFromCFATime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This ctn dispose date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'DisposeDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Y: Pullout; N: Not yet pullout' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PulloutComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This packing list pullout date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PulloutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RefNo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'RefNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hauling Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HaulingStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hauling Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HaulerName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing Audit Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PackingAuditStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Packing Audit Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'PackingAuditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360MDStatus' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'M360MDStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M360 MD Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'M360MDName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hanger Pack Scan Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HangerPackScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hanger Pack Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HangerPackStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Hanger Pack Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HangerPackName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Joker Tag Scan Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'JokerTagScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Joker Tag Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'JokerTagStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Joker Tag Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'JokerTagName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Heat Seal Scan Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HeatSealScanTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Heat Seal Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HeatSealStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Heat Seal Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'HeatSealName'
GO


