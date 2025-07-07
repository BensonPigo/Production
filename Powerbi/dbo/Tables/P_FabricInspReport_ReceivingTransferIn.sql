CREATE TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn](
	[POID] [varchar](13) NOT NULL,
	[SEQ] [varchar](6) NOT NULL,
	[Wkno] [varchar](13) NOT NULL,
	[ReceivingID] [varchar](13) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[Supplier] [varchar](20) NOT NULL,
	[Refno] [varchar](36) NOT NULL,
	[Color] [varchar](50) NOT NULL,
	[ArriveWHDate] [date] NULL,
	[ArriveQty] [numeric](11, 2) NOT NULL,
	[WeaveTypeID] [varchar](20) NOT NULL,
	[Dyelot] [varchar](8) NOT NULL,
	[CutWidth] [numeric](5, 2) NOT NULL,
	[Weight] [numeric](5, 1) NOT NULL,
	[Composition] [varchar](105) NOT NULL,
	[Desc] [nvarchar](max) NOT NULL,
	[FabricConstructionID] [varchar](20) NOT NULL,
	[Roll] [varchar](8) NOT NULL,
	[InspDate] [date] NULL,
	[Result] [varchar](5) NOT NULL,
	[Grade] [varchar](10) NOT NULL,
	[DefectCode] [varchar](2) NOT NULL,
	[DefectType] [varchar](20) NOT NULL,
	[DefectDesc] [varchar](60) NOT NULL,
	[Points] [int] NOT NULL,
	[DefectRate] [numeric](9, 2) NOT NULL,
	[Inspector] [nvarchar](50) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_FabricInspReport_ReceivingTransferIn] PRIMARY KEY CLUSTERED 
(
	[POID] ASC,
	[SEQ] ASC,
	[ReceivingID] ASC,
	[Dyelot] ASC,
	[Roll] ASC,
	[DefectCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_POID]  DEFAULT ('') FOR [POID]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_Table_1_採購單號]  DEFAULT ('') FOR [SEQ]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Wkno]  DEFAULT ('') FOR [Wkno]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_ReceivingID]  DEFAULT ('') FOR [ReceivingID]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Supplier]  DEFAULT ('') FOR [Supplier]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Refno]  DEFAULT ('') FOR [Refno]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Color]  DEFAULT ('') FOR [Color]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_ArriveQty]  DEFAULT ((0)) FOR [ArriveQty]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_WeaveTypeID]  DEFAULT ('') FOR [WeaveTypeID]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Dyelot]  DEFAULT ('') FOR [Dyelot]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_CutWidth]  DEFAULT ((0)) FOR [CutWidth]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Weight]  DEFAULT ((0)) FOR [Weight]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Composition]  DEFAULT ('') FOR [Composition]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Desc]  DEFAULT ('') FOR [Desc]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_FabricConstructionID]  DEFAULT ('') FOR [FabricConstructionID]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Roll]  DEFAULT ('') FOR [Roll]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Result]  DEFAULT ('') FOR [Result]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Grade]  DEFAULT ('') FOR [Grade]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectCode]  DEFAULT ('') FOR [DefectCode]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectType]  DEFAULT ('') FOR [DefectType]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectDesc]  DEFAULT ('') FOR [DefectDesc]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Points]  DEFAULT ((0)) FOR [Points]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectRate]  DEFAULT ((0)) FOR [DefectRate]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Inspector]  DEFAULT ('') FOR [Inspector]
GO

ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項-小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作底稿編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Wkno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌
	' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商代碼-英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Supplier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Color
	' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫收料日/單據日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'WeaveTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幅寬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'CutWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平方米重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Weight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Composition' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Composition'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Desc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組成代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'FabricConstructionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捲號
	' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'InspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等級' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Grade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'英文描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Points' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Points'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectRate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Inspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO