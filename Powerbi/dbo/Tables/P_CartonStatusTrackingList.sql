CREATE TABLE [dbo].[P_CartonStatusTrackingList] (
    [KPIGroup]                      VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_KPIGroup_New] DEFAULT ('') NOT NULL,
    [FactoryID]                     VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Fty_New] DEFAULT ('') NOT NULL,
    [Line]                          VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Line_New] DEFAULT ('') NOT NULL,
    [SP]                            VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_SP_New] DEFAULT ('') NOT NULL,
    [SeqNo]                         VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_SeqNo_New] DEFAULT ('') NOT NULL,
    [Category]                      VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Category_New] DEFAULT ('') NOT NULL,
    [Brand]                         VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Brand_New] DEFAULT ('') NOT NULL,
    [Style]                         VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Style_New] DEFAULT ('') NOT NULL,
    [PONO]                          VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_PONO_New] DEFAULT ('') NOT NULL,
    [Season]                        VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Season_New] DEFAULT ('') NOT NULL,
    [Destination]                   VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Destination_New] DEFAULT ('') NOT NULL,
    [SCIDelivery]                   DATE            NULL,
    [BuyerDelivery]                 DATE            NULL,
    [PackingListID]                 VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_PackingListID_New] DEFAULT ('') NOT NULL,
    [CtnNo]                         VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_CtnNo_New] DEFAULT ('') NOT NULL,
    [Size]                          VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_Size_New] DEFAULT ('') NOT NULL,
    [CartonQty]                     INT             CONSTRAINT [DF_P_CartonStatusTrackingList_CartonQty_New] DEFAULT ((0)) NOT NULL,
    [Status]                        VARCHAR (8000)  NOT NULL,
    [HaulingScanTime]               DATETIME        NULL,
    [HauledQty]                     INT             CONSTRAINT [DF_P_CartonStatusTrackingList_HauledQty_New] DEFAULT ((0)) NOT NULL,
    [DryRoomReceiveTime]            DATETIME        NULL,
    [DryRoomTransferTime]           DATETIME        NULL,
    [MDScanTime]                    DATETIME        NULL,
    [MDFailQty]                     INT             CONSTRAINT [DF_P_CartonStatusTrackingList_MDFailQty_New] DEFAULT ((0)) NOT NULL,
    [PackingAuditScanTime]          DATETIME        NULL,
    [PackingAuditFailQty]           INT             CONSTRAINT [DF_P_CartonStatusTrackingList_PackingAuditFailQty_New] DEFAULT ((0)) NOT NULL,
    [M360MDScanTime]                DATETIME        NULL,
    [M360MDFailQty]                 INT             CONSTRAINT [DF_P_CartonStatusTrackingList_M360MDFailQty_New] DEFAULT ((0)) NOT NULL,
    [TransferToPackingErrorTime]    DATETIME        NULL,
    [ConfirmPackingErrorReviseTime] DATETIME        NULL,
    [ScanAndPackTime]               DATETIME        NULL,
    [ScanQty]                       INT             CONSTRAINT [DF_P_CartonStatusTrackingList_ScanQty_New] DEFAULT ((0)) NOT NULL,
    [FtyTransferToClogTime]         DATETIME        NULL,
    [ClogReceiveTime]               DATETIME        NULL,
    [ClogLocation]                  NVARCHAR (1000) CONSTRAINT [DF_P_CartonStatusTrackingList_ClogLocation_New] DEFAULT ('') NOT NULL,
    [ClogReturnTime]                DATETIME        NULL,
    [ClogTransferToCFATime]         DATETIME        NULL,
    [CFAReceiveTime]                DATETIME        NULL,
    [CFAReturnTime]                 DATETIME        NULL,
    [CFAReturnDestination]          VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_CFAReturnDestination_New] DEFAULT ('') NOT NULL,
    [ClogReceiveFromCFATime]        DATETIME        NULL,
    [DisposeDate]                   DATE            NULL,
    [PulloutComplete]               VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_PulloutComplete_New] DEFAULT ('') NOT NULL,
    [PulloutDate]                   DATE            NULL,
    [RefNo]                         VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__RefNo__4C0CFD33_New] DEFAULT ('') NOT NULL,
    [Description]                   NVARCHAR (MAX)  CONSTRAINT [DF__P_CartonS__Descr__4D01216C_New] DEFAULT ('') NOT NULL,
    [HaulingStatus]                 VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Hauli__4DF545A5_New] DEFAULT ('') NOT NULL,
    [HaulerName]                    VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Haule__4EE969DE_New] DEFAULT ('') NOT NULL,
    [PackingAuditStatus]            VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Packi__4FDD8E17_New] DEFAULT ('') NOT NULL,
    [PackingAuditName]              VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Packi__50D1B250_New] DEFAULT ('') NOT NULL,
    [M360MDStatus]                  VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__M360M__51C5D689_New] DEFAULT ('') NOT NULL,
    [M360MDName]                    VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__M360M__52B9FAC2_New] DEFAULT ('') NOT NULL,
    [HangerPackScanTime]            DATETIME        NULL,
    [HangerPackStatus]              VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Hange__53AE1EFB_New] DEFAULT ('') NOT NULL,
    [HangerPackName]                VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Hange__54A24334_New] DEFAULT ('') NOT NULL,
    [JokerTagScanTime]              DATETIME        NULL,
    [JokerTagStatus]                VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Joker__5596676D_New] DEFAULT ('') NOT NULL,
    [JokerTagName]                  VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__Joker__568A8BA6_New] DEFAULT ('') NOT NULL,
    [HeatSealScanTime]              DATETIME        NULL,
    [HeatSealStatus]                VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__HeatS__577EAFDF_New] DEFAULT ('') NOT NULL,
    [HeatSealName]                  VARCHAR (8000)  CONSTRAINT [DF__P_CartonS__HeatS__5872D418_New] DEFAULT ('') NOT NULL,
    [MDMachineNo]                   VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_MDMachineNo_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]                   VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                  DATETIME        NULL,
    [BIStatus]                      VARCHAR (8000)  CONSTRAINT [DF_P_CartonStatusTrackingList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CartonStatusTrackingList] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [SP] ASC, [SeqNo] ASC, [PackingListID] ASC, [CtnNo] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SDP KPI Code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'KPIGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'SP'
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD機器代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'MDMachineNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CartonStatusTrackingList', @level2type = N'COLUMN', @level2name = N'BIStatus';

