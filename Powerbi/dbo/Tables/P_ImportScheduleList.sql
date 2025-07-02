CREATE TABLE [dbo].[P_ImportScheduleList](
	[WK] [varchar](13) NOT NULL,
	[ExportDetailUkey] [bigint] NOT NULL,
	[ETA] [date] NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Consignee] [varchar](8) NOT NULL,
	[ShipModeID] [varchar](10) NOT NULL,
	[CYCFS] [varchar](6) NOT NULL,
	[Blno] [varchar](20) NOT NULL,
	[Packages] [numeric](5, 0) NOT NULL,
	[Vessel] [nvarchar](60) NOT NULL,
	[ProdFactory] [varchar](8) NOT NULL,
	[OrderTypeID] [varchar](20) NOT NULL,
	[ProjectID] [varchar](5) NOT NULL,
	[Category] [varchar](10) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[StyleName] [nvarchar](50) NOT NULL,
	[PoID] [varchar](13) NOT NULL,
	[Seq] [varchar](6) NOT NULL,
	[Refno] [varchar](36) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[ColorName] [varchar](300) NOT NULL,
	[Description] [varchar](3000) NOT NULL,
	[MtlType] [varchar](10) NOT NULL,
	[SubMtlType] [varchar](20) NOT NULL,
	[WeaveType] [varchar](20) NOT NULL,
	[SuppID] [varchar](6) NOT NULL,
	[SuppName] [varchar](12) NOT NULL,
	[UnitID] [varchar](8) NOT NULL,
	[SizeSpec] [varchar](50) NOT NULL,
	[ShipQty] [numeric](12, 2) NOT NULL,
	[FOC] [numeric](12, 2) NOT NULL,
	[NetKg] [numeric](10, 2) NOT NULL,
	[WeightKg] [numeric](10, 2) NOT NULL,
	[ArriveQty] [numeric](12, 2) NOT NULL,
	[ArriveQtyStockUnit] [numeric](12, 2) NOT NULL,
	[FirstBulkSewInLine] [date] NULL,
	[FirstCutDate] [date] NULL,
	[ReceiveQty] [numeric](11, 2) NOT NULL,
	[TotalRollsCalculated] [int] NOT NULL,
	[StockUnit] [varchar](8) NOT NULL,
	[MCHandle] [varchar](100) NOT NULL,
	[ContainerType] [varchar](255) NOT NULL,
	[ContainerNo] [varchar](255) NOT NULL,
	[PortArrival] [date] NULL,
	[WhseArrival] [date] NULL,
	[KPILETA] [date] NULL,
	[PFETA] [date] NULL,
	[EarliestSCIDelivery] [date] NULL,
	[EarlyDays] [int] NOT NULL,
	[EarliestPFETA] [int] NOT NULL,
	[MRMail] [varchar](100) NOT NULL,
	[SMRMail] [varchar](100) NOT NULL,
	[EditName] [varchar](45) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[FabricCombo] [varchar](10) NOT NULL,
	[BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_ImportScheduleList] PRIMARY KEY CLUSTERED 
(
	[ExportDetailUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_WK]  DEFAULT ('') FOR [WK]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ExportDetailUkey]  DEFAULT ((0)) FOR [ExportDetailUkey]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ETA]  DEFAULT (NULL) FOR [ETA]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Consignee]  DEFAULT ('') FOR [Consignee]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ShipModeID]  DEFAULT ('') FOR [ShipModeID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_CYCFS]  DEFAULT ('') FOR [CYCFS]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Blno]  DEFAULT ('') FOR [Blno]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Packages]  DEFAULT ((0)) FOR [Packages]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Vessel]  DEFAULT ('') FOR [Vessel]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ProdFactory]  DEFAULT ('') FOR [ProdFactory]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_OrderTypeID]  DEFAULT ('') FOR [OrderTypeID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ProjectID]  DEFAULT ('') FOR [ProjectID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_StyleName]  DEFAULT ('') FOR [StyleName]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_PoID]  DEFAULT ('') FOR [PoID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Refno]  DEFAULT ('') FOR [Refno]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ColorName]  DEFAULT ('') FOR [ColorName]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_MtlType]  DEFAULT ('') FOR [MtlType]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_SubMtlType]  DEFAULT ('') FOR [SubMtlType]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_WeaveType]  DEFAULT ('') FOR [WeaveType]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_SuppID]  DEFAULT ('') FOR [SuppID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_SuppName]  DEFAULT ('') FOR [SuppName]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_UnitID]  DEFAULT ('') FOR [UnitID]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_SizeSpec]  DEFAULT ('') FOR [SizeSpec]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ShipQty]  DEFAULT ((0)) FOR [ShipQty]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_FOC]  DEFAULT ((0)) FOR [FOC]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_NetKg]  DEFAULT ((0)) FOR [NetKg]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_WeightKg]  DEFAULT ((0)) FOR [WeightKg]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ArriveQty]  DEFAULT ((0)) FOR [ArriveQty]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ArriveQtyStockUnit]  DEFAULT ((0)) FOR [ArriveQtyStockUnit]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_FirstBulkSewInLine]  DEFAULT (NULL) FOR [FirstBulkSewInLine]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_FirstCutDate]  DEFAULT (NULL) FOR [FirstCutDate]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ReceiveQty]  DEFAULT ((0)) FOR [ReceiveQty]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_TotalRollsCalculated]  DEFAULT ((0)) FOR [TotalRollsCalculated]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_StockUnit]  DEFAULT ('') FOR [StockUnit]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_MCHandle]  DEFAULT ('') FOR [MCHandle]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ContainerType]  DEFAULT ('') FOR [ContainerType]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_ContainerNo]  DEFAULT ('') FOR [ContainerNo]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_PortArrival]  DEFAULT (NULL) FOR [PortArrival]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_WhseArrival]  DEFAULT (NULL) FOR [WhseArrival]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_KPILETA]  DEFAULT (NULL) FOR [KPILETA]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_PFETA]  DEFAULT (NULL) FOR [PFETA]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_EarliestSCIDelivery]  DEFAULT (NULL) FOR [EarliestSCIDelivery]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_EarlyDays]  DEFAULT ((0)) FOR [EarlyDays]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_EarliestPFETA]  DEFAULT ((0)) FOR [EarliestPFETA]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_MRMail]  DEFAULT ('') FOR [MRMail]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_SMRMail]  DEFAULT ('') FOR [SMRMail]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_AddDate]  DEFAULT (NULL) FOR [AddDate]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_EditDate]  DEFAULT (NULL) FOR [EditDate]
GO

ALTER TABLE [dbo].[P_ImportScheduleList] ADD  CONSTRAINT [DF_P_ImportScheduleList_FabricCombo]  DEFAULT ('') FOR [FabricCombo]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WK'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨明細Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ExportDetailUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計到達日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收件人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Consignee'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'運送類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ShipModeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'整櫃或散櫃' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'CYCFS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Blno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總件/箱數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Packages'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'船名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Vessel'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ProdFactory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類細項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'專案代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ProjectID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'StyleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'PoID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'料號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ColorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'詳細介紹' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MtlType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料細項類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SubMtlType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WeaveType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SuppID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供應商英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SuppName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'UnitID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SizeSpec'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ShipQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口FOC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FOC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'淨重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'NetKg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'毛重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WeightKg'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'本次出口數量+本次出口FOC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ArriveQtyStockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最初上產線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FirstBulkSewInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最初裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FirstCutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'庫存實收數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ReceiveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總卷數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'TotalRollsCalculated'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'使用單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'StockUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MC Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨櫃類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ContainerType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貨櫃編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'ContainerNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到港日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'PortArrival'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'WhseArrival'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'KPILETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'PFETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最早SCI交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EarliestSCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期-KPI L/ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EarlyDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'到達W/H日期-P/F ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EarliestPFETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MR的Mail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'MRMail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMR的Mail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'SMRMail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Export的編輯人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Export的新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Export的編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EachCons.FabricCombo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ImportScheduleList', @level2type=N'COLUMN',@level2name=N'FabricCombo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ImportScheduleList',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_ImportScheduleList',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'