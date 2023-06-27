CREATE TABLE [dbo].[P_SDP]
(
	Country						varchar(2)		CONSTRAINT [DF_P_SDP_Country]						DEFAULT ((''))	NOT NULL,
	KPIGroup					varchar(8)		CONSTRAINT [DF_P_SDP_KPIGroup]						DEFAULT ((''))	NOT NULL,
	Factory						varchar(8)		CONSTRAINT [DF_P_SDP_FactoryID]						DEFAULT ((''))	NOT NULL,
	SPNo						varchar(13)		CONSTRAINT [DF_P_SDP_SPNo]							DEFAULT ((''))	NOT NULL,
	Style						varchar(15)		CONSTRAINT [DF_P_SDP_Style]							DEFAULT ((''))	NOT NULL,
	Seq							varchar(2)		CONSTRAINT [DF_P_SDP_Seq]							DEFAULT ((''))	NOT NULL,
	Brand						varchar(8)		CONSTRAINT [DF_P_SDP_Brand]							DEFAULT ((''))	NOT NULL,
	BuyerDelivery				varchar(10)		CONSTRAINT [DF_P_SDP_BuyerDelivery]					DEFAULT ((''))	NOT NULL,
	FactoryKPI					varchar(10)		CONSTRAINT [DF_P_SDP_FactoryKPI]					DEFAULT ((''))	NOT NULL,
	Extension					varchar(10)		CONSTRAINT [DF_P_SDP_Extension]						DEFAULT ((''))	NOT NULL,
	DeliveryByShipmode			varchar(10)		CONSTRAINT [DF_P_SDP_DeliveryByShipmode]			DEFAULT ((''))	NOT NULL,
	OrderQty					int				CONSTRAINT [DF_P_SDP_OrderQty]						DEFAULT ((0))	NOT NULL,
	OnTimeQty					int				CONSTRAINT [DF_P_SDP_OnTimeQty]						DEFAULT ((0))	NOT NULL,
	FailQty						int				CONSTRAINT [DF_P_SDP_FailQty]						DEFAULT ((0))	NOT NULL,
	ClogRec_OnTimeQty			int				CONSTRAINT [DF_P_SDP_ClogRec_OnTimeQty]				DEFAULT ((0))	NOT NULL,
	ClogRec_FailQty				int				CONSTRAINT [DF_P_SDP_ClogRec_FailQty]				DEFAULT ((0))	NOT NULL,
	PullOutDate					DATE		CONSTRAINT [DF_P_SDP_PullOutDate]					DEFAULT ((''))	NOT NULL,
	ShipMode					varchar(10)		CONSTRAINT [DF_P_SDP_ShipMode]						DEFAULT ((''))	NOT NULL,
	Pullouttimes				int				CONSTRAINT [DF_P_SDP_Pullouttimes]					DEFAULT ((0))	NOT NULL,
	GarmentComplete				varchar(1)		CONSTRAINT [DF_P_SDP_GarmentComplete]				DEFAULT ((''))	NOT NULL,	
	ReasonID					varchar(5)		CONSTRAINT [DF_P_SDP_ReasonID]						DEFAULT ((''))	NOT NULL,
	OrderReason					nvarchar(500)	CONSTRAINT [DF_P_SDP_OrderReason]					DEFAULT ((''))	NOT NULL,
	Handle						varchar(45)		CONSTRAINT [DF_P_SDP_Handle]						DEFAULT ((''))	NOT NULL,
	SMR							varchar(45)		CONSTRAINT [DF_P_SDP_SMR]							DEFAULT ((''))	NOT NULL,
	POHandle					varchar(45)		CONSTRAINT [DF_P_SDP_POHandle]						DEFAULT ((''))	NOT NULL,
	POSMR						varchar(45)		CONSTRAINT [DF_P_SDP_POSMR]							DEFAULT ((''))	NOT NULL,
	OrderType					varchar(20)		CONSTRAINT [DF_P_SDP_OrderType]						DEFAULT ((''))	NOT NULL,
	DevSample					varchar(1)		CONSTRAINT [DF_P_SDP_DevSample]						DEFAULT ((''))	NOT NULL,
	SewingQty					int				CONSTRAINT [DF_P_SDP_SewingQty]						DEFAULT ((0))	NOT NULL,
	FOCQty						int				CONSTRAINT [DF_P_SDP_FOCQty]						DEFAULT ((0))	NOT NULL,
	LastSewingOutputDate		DATE		CONSTRAINT [DF_P_SDP_LastSewingOutputDate]			DEFAULT ((''))	NOT NULL,
	LastCartonReceivedDate		datetime																			NULL,
	IDDReason					nvarchar(60)	CONSTRAINT [DF_P_SDP_IDDReason]						DEFAULT ((''))	NOT NULL,
	PartialShipment				varchar(1)		CONSTRAINT [DF_P_SDP_PartialShipment]				DEFAULT ((''))	NOT NULL,
	Alias						varchar(30)		CONSTRAINT [DF_P_SDP_Alias]							DEFAULT ((''))	NOT NULL,
	CFAInspectionDate			datetime																			NULL,
	CFAFinalInspectionResult	varchar(16)		CONSTRAINT [DF_P_SDP_CFAFinalInspectionResult]		DEFAULT ((''))	NOT NULL,
	CFA3rdInspectDate		    datetime																			NULL,
	CFA3rdInspectResult			varchar(16)		CONSTRAINT [DF_P_SDP_CFA3rdInspectResult]			DEFAULT ((''))	NOT NULL,
	Destination					varchar(2)		CONSTRAINT [DF_P_SDP_Destination]					DEFAULT ((''))	NOT NULL,
	PONO						varchar(30)		CONSTRAINT [DF_P_SDP_PONO]							DEFAULT ((''))	NOT NULL,
	OutstandingReason			nvarchar(500)	CONSTRAINT [DF_P_SDP_OutstandingReason]				DEFAULT ((''))	NOT NULL,
	ReasonRemark				nvarchar(Max)	CONSTRAINT [DF_P_SDP_ReasonRemark]					DEFAULT ((''))	NOT NULL, 
    CONSTRAINT [PK_P_SDP] PRIMARY KEY ([Factory],[SPNO],[Style],[Seq])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'原因備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'ReasonRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Outstanding Reason',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'OutstandingReason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'國別+別名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Destination'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'客戶訂單單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'PONO'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'國別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Country'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠kpi統計群組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'KPIGroup'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Factory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'SPNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'款式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Style'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'序號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Seq'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Brand'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'客戶交期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'BuyerDelivery'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠KPI',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'FactoryKPI'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'展延日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Extension'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'運輸方式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'DeliveryByShipmode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單數量Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'OrderQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'On Time Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'OnTimeQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fail Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'FailQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'On Time Qty(Clog rec)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'ClogRec_OnTimeQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Fail Qty(Clog rec)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'ClogRec_FailQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出貨日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'PullOutDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'運輸方式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'ShipMode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出貨次數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Pullouttimes'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Garment Complete ( From Trade)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'GarmentComplete'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ReasonID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'ReasonID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Reason敘述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'OrderReason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單Handle',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Handle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'組長',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'SMR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'po Handle',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'POHandle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'po 組長',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'POSMR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'分類細項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'OrderType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'is Dev Sample',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'DevSample'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車縫數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'SewingQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'免費數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'FOCQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後產出日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'LastSewingOutputDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後收料日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'LastCartonReceivedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IDD(Intended Delivery Date) Reason',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'IDDReason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否分批運送',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'PartialShipment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'別名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'Alias'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'CFAInspectionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'CFAFinalInspectionResult'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'第三方檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'CFA3rdInspectDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'第三方檢驗結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SDP',
    @level2type = N'COLUMN',
    @level2name = N'CFA3rdInspectResult'