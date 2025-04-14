CREATE TABLE [dbo].[P_SDP](
	[Country] [varchar](2) NOT NULL,
	[KPIGroup] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SPNo] [varchar](13) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[Seq] [varchar](2) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[BuyerDelivery] [date] NULL,
	[FactoryKPI] [date] NULL,
	[Extension] [date] NULL,
	[DeliveryByShipmode] [varchar](10) NOT NULL,
	[OrderQty] [int] NOT NULL,
	[OnTimeQty] [int] NOT NULL,
	[FailQty] [int] NOT NULL,
	[ClogRec_OnTimeQty] [int] NOT NULL,
	[ClogRec_FailQty] [int] NOT NULL,
	[PullOutDate] [char](10) NULL,
	[ShipMode] [varchar](10) NOT NULL,
	[Pullouttimes] [int] NOT NULL,
	[GarmentComplete] [varchar](1) NOT NULL,
	[ReasonID] [varchar](5) NOT NULL,
	[OrderReason] [nvarchar](500) NOT NULL,
	[Handle] [varchar](45) NOT NULL,
	[SMR] [varchar](45) NOT NULL,
	[POHandle] [varchar](45) NOT NULL,
	[POSMR] [varchar](45) NOT NULL,
	[OrderType] [varchar](20) NOT NULL,
	[DevSample] [varchar](1) NOT NULL,
	[SewingQty] [int] NOT NULL,
	[FOCQty] [int] NOT NULL,
	[LastSewingOutputDate] [varchar](10) NULL,
	[LastCartonReceivedDate] [datetime] NULL,
	[IDDReason] [nvarchar](60) NOT NULL,
	[PartialShipment] [varchar](1) NOT NULL,
	[Alias] [varchar](30) NOT NULL,
	[CFAInspectionDate] [datetime] NULL,
	[CFAFinalInspectionResult] [varchar](16) NOT NULL,
	[CFA3rdInspectDate] [datetime] NULL,
	[CFA3rdInspectResult] [varchar](16) NOT NULL,
	[Destination] [varchar](50) NOT NULL,
	[PONO] [varchar](30) NOT NULL,
	[OutstandingReason] [nvarchar](500) NOT NULL,
	[ReasonRemark] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_P_SDP] PRIMARY KEY CLUSTERED 
(
	[FactoryID] asc,
	[SPNo] ASC,
	[Style] ASC,
	[Seq] ASC,
	[Pullouttimes] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Country]  DEFAULT ('') FOR [Country]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_KPIGroup]  DEFAULT ('') FOR [KPIGroup]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Factory]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_SPNo]  DEFAULT ('') FOR [SPNo]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_DeliveryByShipmode]  DEFAULT ('') FOR [DeliveryByShipmode]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_OnTimeQty]  DEFAULT ((0)) FOR [OnTimeQty]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_FailQty]  DEFAULT ((0)) FOR [FailQty]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_ClogRec_OnTimeQty]  DEFAULT ((0)) FOR [ClogRec_OnTimeQty]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_ClogRec_FailQty]  DEFAULT ((0)) FOR [ClogRec_FailQty]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_PullOutDate]  DEFAULT ('') FOR [PullOutDate]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_ShipMode]  DEFAULT ('') FOR [ShipMode]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Pullouttimes]  DEFAULT ((0)) FOR [Pullouttimes]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_GarmentComplete]  DEFAULT ('') FOR [GarmentComplete]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_ReasonID]  DEFAULT ('') FOR [ReasonID]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_OrderReason]  DEFAULT ('') FOR [OrderReason]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Handle]  DEFAULT ('') FOR [Handle]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_SMR]  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_POHandle]  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_POSMR]  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_OrderType]  DEFAULT ('') FOR [OrderType]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_DevSample]  DEFAULT ('') FOR [DevSample]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_SewingQty]  DEFAULT ((0)) FOR [SewingQty]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_FOCQty]  DEFAULT ((0)) FOR [FOCQty]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_LastSewingOutputDate]  DEFAULT ('') FOR [LastSewingOutputDate]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_IDDReason]  DEFAULT ('') FOR [IDDReason]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_PartialShipment]  DEFAULT ('') FOR [PartialShipment]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Alias]  DEFAULT ('') FOR [Alias]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_CFAFinalInspectionResult]  DEFAULT ('') FOR [CFAFinalInspectionResult]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_CFA3rdInspectResult]  DEFAULT ('') FOR [CFA3rdInspectResult]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_Destination]  DEFAULT ('') FOR [Destination]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_PONO]  DEFAULT ('') FOR [PONO]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_OutstandingReason]  DEFAULT ('') FOR [OutstandingReason]
GO

ALTER TABLE [dbo].[P_SDP] ADD  CONSTRAINT [DF_P_SDP_ReasonRemark]  DEFAULT ('') FOR [ReasonRemark]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Country'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠kpi統計群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'KPIGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠KPI' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FactoryKPI'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'展延日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Extension'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運輸方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'DeliveryByShipmode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'On Time Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OnTimeQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fail Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'On Time Qty(Clog rec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ClogRec_OnTimeQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fail Qty(Clog rec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ClogRec_FailQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'PullOutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運輸方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ShipMode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Pullouttimes'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Garment Complete ( From Trade)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'GarmentComplete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ReasonID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ReasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reason敘述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OrderReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'POHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po 組長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'POSMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'is Dev Sample' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'DevSample'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'SewingQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'免費數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'FOCQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後產出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'LastSewingOutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後收料日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'LastCartonReceivedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IDD(Intended Delivery Date) Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'IDDReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否分批運送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'PartialShipment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'別名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Alias'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFAInspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFAFinalInspectionResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFA3rdInspectDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'CFA3rdInspectResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國別+別名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'Destination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'PONO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Outstanding Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'OutstandingReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原因備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SDP', @level2type=N'COLUMN',@level2name=N'ReasonRemark'
GO