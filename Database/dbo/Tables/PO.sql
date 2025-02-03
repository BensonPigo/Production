CREATE TABLE [dbo].[PO](
	[ID] [varchar](13) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[SeasonId] [varchar](10) NOT NULL,
	[StyleUkey] [bigint] NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[POSMR] [varchar](10) NOT NULL,
	[POHandle] [varchar](10) NOT NULL,
	[PCHandle] [varchar](10) NOT NULL,
	[PCSMR] [varchar](10) NOT NULL,
	[McHandle] [varchar](10) NOT NULL,
	[ShipMark] [nvarchar](max) NOT NULL,
	[FTYMark] [varchar](20) NOT NULL,
	[Complete] [bit] NOT NULL,
	[PoRemark] [nvarchar](max) NOT NULL,
	[CostRemark] [nvarchar](max) NOT NULL,
	[IrregularRemark] [nvarchar](max) NOT NULL,
	[FirstPoError] [varchar](3) NOT NULL,
	[FirstEditName] [varchar](10) NOT NULL,
	[FirstEditDate] [datetime] NULL,
	[FirstAddDate] [datetime] NULL,
	[FirstCostDate] [datetime] NULL,
	[LastPoError] [varchar](3) NOT NULL,
	[LastEditName] [varchar](10) NOT NULL,
	[LastEditDate] [datetime] NULL,
	[LastAddDate] [datetime] NULL,
	[LastCostDate] [datetime] NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[FIRRemark] [varchar](60) NOT NULL,
	[AIRRemark] [varchar](60) NOT NULL,
	[FIRLaboratoryRemark] [varchar](60) NOT NULL,
	[AIRLaboratoryRemark] [varchar](60) NOT NULL,
	[OvenLaboratoryRemark] [varchar](60) NOT NULL,
	[ColorFastnessLaboratoryRemark] [varchar](60) NOT NULL,
	[MTLDelay] [date] NULL,
	[MinSciDelivery] [date] NULL,
	[FIRInspPercent] [numeric](5, 2) NOT NULL,
	[AIRInspPercent] [numeric](5, 2) NOT NULL,
	[FIRLabInspPercent] [numeric](5, 2) NOT NULL,
	[LabColorFastnessPercent] [numeric](5, 2) NOT NULL,
	[LabOvenPercent] [numeric](5, 2) NOT NULL,
	[AIRLabInspPercent] [numeric](5, 2) NOT NULL,
	[ThreadVersion] [varchar](5) NOT NULL,
	[WaterFastnessLaboratoryRemark] [varchar](60) NOT NULL,
	[LabWaterFastnessPercent] [numeric](5, 2) NOT NULL,
	[PerspirationFastnessLaboratoryRemark] [varchar](60) NOT NULL,
	[LabPerspirationFastnessPercent] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_PO] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_SeasonId]  DEFAULT ('') FOR [SeasonId]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_StyleUkey]  DEFAULT ((0)) FOR [StyleUkey]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_POSMR]  DEFAULT ('') FOR [POSMR]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_POHandle]  DEFAULT ('') FOR [POHandle]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_PCHandle]  DEFAULT ('') FOR [PCHandle]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_PCSMR]  DEFAULT ('') FOR [PCSMR]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_McHandle]  DEFAULT ('') FOR [McHandle]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_ShipMark]  DEFAULT ('') FOR [ShipMark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_FTYMark]  DEFAULT ('') FOR [FTYMark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_Complete]  DEFAULT ((0)) FOR [Complete]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_PoRemark]  DEFAULT ('') FOR [PoRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_CostRemark]  DEFAULT ('') FOR [CostRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_IrregularRemark]  DEFAULT ('') FOR [IrregularRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_FirstPoError]  DEFAULT ('') FOR [FirstPoError]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_FirstEditName]  DEFAULT ('') FOR [FirstEditName]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_LastPoError]  DEFAULT ('') FOR [LastPoError]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_LastEditName]  DEFAULT ('') FOR [LastEditName]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_FIRRemark]  DEFAULT ('') FOR [FIRRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_AIRRemark]  DEFAULT ('') FOR [AIRRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_FIRLaboratoryRemark]  DEFAULT ('') FOR [FIRLaboratoryRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_AIRLaboratoryRemark]  DEFAULT ('') FOR [AIRLaboratoryRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_OvenLaboratoryRemark]  DEFAULT ('') FOR [OvenLaboratoryRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_ColorFastnessLaboratoryRemark]  DEFAULT ('') FOR [ColorFastnessLaboratoryRemark]
GO

ALTER TABLE [dbo].[PO] ADD  DEFAULT ((0.00)) FOR [FIRInspPercent]
GO

ALTER TABLE [dbo].[PO] ADD  DEFAULT ((0.00)) FOR [AIRInspPercent]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_FIRLabInspPercent]  DEFAULT ((0)) FOR [FIRLabInspPercent]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_LabColorFastnessPercent]  DEFAULT ((0)) FOR [LabColorFastnessPercent]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_LabOvenPercent]  DEFAULT ((0)) FOR [LabOvenPercent]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_AIRLabInspPercent]  DEFAULT ((0)) FOR [AIRLabInspPercent]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_ThreadVersion]  DEFAULT ('') FOR [ThreadVersion]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_WaterFastnessLaboratoryRemark]  DEFAULT ('') FOR [WaterFastnessLaboratoryRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_LabWaterFastnessPercent]  DEFAULT ((0)) FOR [LabWaterFastnessPercent]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_PerspirationFastnessLaboratoryRemark]  DEFAULT ('') FOR [PerspirationFastnessLaboratoryRemark]
GO

ALTER TABLE [dbo].[PO] ADD  CONSTRAINT [DF_PO_LabPerspirationFastnessPercent]  DEFAULT ((0)) FOR [LabPerspirationFastnessPercent]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'SeasonId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式的唯一值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'StyleUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購主管' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'POSMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'po Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'POHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排船表的PC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'PCHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PC的主管' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'PCSMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠MC handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'McHandle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'由 <工廠> 代入' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'ShipMark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠嘜頭' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FTYMark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'結清' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'Complete'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'PoRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'<COST>備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'CostRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'<IRRE L/ETA>的備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'IrregularRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購成本異常代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FirstPoError'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購成本異常修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FirstEditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購成本異常修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FirstEditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'異常代碼第一次填入日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FirstAddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第一次 COST異常發生日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FirstCostDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後採購成本異常代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'LastPoError'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後採購成本異常修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'LastEditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後採購成本異常修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'LastEditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後異常代碼第一次填入日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'LastAddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後COST異常發生日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'LastCostDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料檢驗備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FIRRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'輔料檢驗備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'AIRRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主料水洗房備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'FIRLaboratoryRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'輔料水洗房備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'AIRLaboratoryRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'烘箱備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'OvenLaboratoryRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'掉色備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'ColorFastnessLaboratoryRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MTL contiguous delay' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO', @level2type=N'COLUMN',@level2name=N'MTLDelay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PO'
GO