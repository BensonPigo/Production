CREATE TABLE [dbo].[P_SewingDailyOutput](
	[Ukey] [bigint] NOT NULL,
	[MDivisionID] [varchar](20) NOT NULL,
	[FactoryID] [varchar](20) NULL,
	[ComboType] [varchar](1) NOT NULL,
	[Category] [varchar](20) NULL,
	[CountryID] [varchar](20) NULL,
	[OutputDate] [date] NULL,
	[SewingLineID] [varchar](10) NULL,
	[Shift] [varchar](30) NULL,
	[SubconOutFty] [varchar](15) NULL,
	[SubConOutContractNumber] [varchar](50) NULL,
	[Team] [varchar](10) NULL,
	[OrderID] [varchar](13) NULL,
	[Article] [varchar](8) NULL,
	[SizeCode] [varchar](8) NULL,
	[CustPONo] [varchar](30) NULL,
	[BuyerDelivery] [date] NULL,
	[OrderQty] [int] NULL,
	[BrandID] [varchar](20) NULL,
	[OrderCategory] [varchar](20) NULL,
	[ProgramID] [varchar](20) NULL,
	[OrderTypeID] [varchar](20) NULL,
	[DevSample] [varchar](5) NULL,
	[CPURate] [numeric](15, 1) NULL,
	[StyleID] [varchar](20) NULL,
	[Season] [varchar](10) NULL,
	[CdCodeID] [varchar](15) NULL,
	[ActualManpower] [numeric](12, 1) NULL,
	[NoOfHours] [numeric](10, 4) NULL,
	[TotalManhours] [numeric](12, 3) NULL,
	[TargetCPU] [numeric](10, 3) NULL,
	[TMS] [int] NULL,
	[CPUPrice] [numeric](10, 3) NULL,
	[TargetQty] [int] NULL,
	[TotalOutputQty] [int] NULL,
	[TotalCPU] [numeric](10, 3) NULL,
	[CPUSewerHR] [numeric](10, 3) NULL,
	[EFF] [numeric](10, 2) NULL,
	[RFT] [numeric](10, 2) NULL,
	[CumulateOfDays] [int] NULL,
	[DateRange] [varchar](15) NULL,
	[ProdOutput] [int] NULL,
	[Diff] [int] NULL,
	[Rate] [numeric](10, 2) NULL,
	[SewingReasonDesc] [nvarchar](1000) NULL,
	[SciDelivery] [date] NULL,
	[CDCodeNew] [varchar](5) NULL,
	[ProductType] [nvarchar](500) NULL,
	[FabricType] [nvarchar](500) NULL,
	[Lining] [varchar](20) NULL,
	[Gender] [varchar](10) NULL,
	[Construction] [nvarchar](50) NULL,
	[LockStatus] [varchar](12) Not NULL,
	[Cancel] [varchar](1) Not Null,
	[Remark] [varchar](MAX) Not Null,
	[SPFactory] [varchar](8) Not Null,
	[NonRevenue] [varchar](1) Not Null,
	-----------------------------------------------------
	[AT_HAND_TMS] [numeric](15, 4) Not Null,
	[AT_HAND_CPU] [numeric](15, 4) Not Null,
	[TTL_AT_HAND_TMS] [numeric](15, 4) Not Null,
	[TTL_AT_HAND_CPU] [numeric](15, 4) Not Null,
	[AT_MACHINE_TMS] [numeric](15, 4) Not Null,
	[AT_MACHINE_CPU] [numeric](15, 4) Not Null,
	[TTL_AT_MACHINE_TMS] [numeric](15, 4) Not Null,
	[TTL_AT_MACHINE_CPU] [numeric](15, 4) Not Null,
	[TTL_AT_CPU] [numeric](15, 4) Not Null,
	[BONDING_HAND_TMS] [numeric](15, 4) Not Null,
	[BONDING_HAND_CPU] [numeric](15, 4) Not Null,
	[TTL_BONDING_HAND_TMS] [numeric](15, 4) Not Null,
	[TTL_BONDING_HAND_CPU] [numeric](15, 4) Not Null,
	[BONDING_MACHINE_TMS] [numeric](15, 4) Not Null,
	[BONDING_MACHINE_CPU] [numeric](15, 4) Not Null,
	[TTL_BONDING_MACHINE_TMS] [numeric](15, 4) Not Null,
	[TTL_BONDING_MACHINE_CPU] [numeric](15, 4) Not Null,
	[CARTON_Price] [numeric](15, 4) Not Null,
	[TTL_CARTON_Price] [numeric](15, 4) Not Null,
	[CUTTING_TMS] [numeric](15, 4) Not Null,
	[CUTTING_CPU] [numeric](15, 4) Not Null,
	[TTL_CUTTING_TMS] [numeric](15, 4) Not Null,
	[TTL_CUTTING_CPU] [numeric](15, 4) Not Null,
	[DIE_CUT_TMS] [numeric](15, 4) Not Null,
	[DIE_CUT_CPU] [numeric](15, 4) Not Null,
	[TTL_DIE_CUT_TMS] [numeric](15, 4) Not Null,
	[TTL_DIE_CUT_CPU] [numeric](15, 4) Not Null,
	[DOWN_TMS] [numeric](15, 4) Not Null,
	[DOWN_CPU] [numeric](15, 4) Not Null,
	[TTL_DOWN_TMS] [numeric](15, 4) Not Null,
	[TTL_DOWN_CPU] [numeric](15, 4) Not Null,
	[EM_DEBOSS_I_H_TMS] [numeric](15, 4) Not Null,
	[EM_DEBOSS_I_H_CPU] [numeric](15, 4) Not Null,
	[TTL_EM_DEBOSS_I_H_TMS] [numeric](15, 4) Not Null,
	[TTL_EM_DEBOSS_I_H_CPU] [numeric](15, 4) Not Null,
	[EM_DEBOSS_MOLD_Price] [numeric](15, 4) Not Null,
	[TTL_EM_DEBOSS_MOLD_Price] [numeric](15, 4) Not Null,
	[EMB_THREAD] [numeric](15, 4) Not Null,
	[TTL_EMB_THREAD] [numeric](15, 4) Not Null,
	[EMBOSS_DEBOSS_PCS] [numeric](15, 4) Not Null,
	[TTL_EMBOSS_DEBOSS_PCS] [numeric](15, 4) Not Null,
	[EMBOSS_DEBOSS_Price] [numeric](15, 4) Not Null,
	[TTL_EMBOSS_DEBOSS_Price] [numeric](15, 4) Not Null,
	[EMBROIDERY_Price] [numeric](15, 4) Not Null,
	[TTL_EMBROIDERY_Price] [numeric](15, 4) Not Null,
	[EMBROIDERY_STITCH] [numeric](15, 4) Not Null,
	[TTL_EMBROIDERY_STITCH] [numeric](15, 4) Not Null,
	[FARM_OUT_QUILTING_PCS] [numeric](15, 4) Not Null,
	[TTL_FARM_OUT_QUILTING_PCS] [numeric](15, 4) Not Null,
	[FARM_OUT_QUILTING_Price] [numeric](15, 4) Not Null,
	[TTL_FARM_OUT_QUILTING_Price] [numeric](15, 4) Not Null,
	[Garment_Dye_PCS] [numeric](15, 4) Not Null,
	[TTL_Garment_Dye_PCS] [numeric](15, 4) Not Null,
	[Garment_Dye_Price] [numeric](15, 4) Not Null,
	[TTL_Garment_Dye_Price] [numeric](15, 4) Not Null,
	[GLUE_BO_HAND_TMS] [numeric](15, 4) Not Null,
	[GLUE_BO_HAND_CPU] [numeric](15, 4) Not Null,
	[TTL_GLUE_BO_HAND_TMS] [numeric](15, 4) Not Null,
	[TTL_GLUE_BO_HAND_CPU] [numeric](15, 4) Not Null,
	[GLUE_BO_MACHINE_TMS] [numeric](15, 4) Not Null,
	[GLUE_BO_MACHINE_CPU] [numeric](15, 4) Not Null,
	[TTL_GLUE_BO_MACHINE_TMS] [numeric](15, 4) Not Null,
	[TTL_GLUE_BO_MACHINE_CPU] [numeric](15, 4) Not Null,
	[GMT_DRY_PCS] [numeric](15, 4) Not Null,
	[TTL_GMT_DRY_PCS] [numeric](15, 4) Not Null,
	[GMT_DRY_Price] [numeric](15, 4) Not Null,
	[TTL_GMT_DRY_Price] [numeric](15, 4) Not Null,
	[GMT_WASH_PCS] [numeric](15, 4) Not Null,
	[TTL_GMT_WASH_PCS] [numeric](15, 4) Not Null,
	[GMT_WASH_Price] [numeric](15, 4) Not Null,
	[TTL_GMT_WASH_Price] [numeric](15, 4) Not Null,
	[HEAT_SET_PLEAT_PCS] [numeric](15, 4) Not Null,
	[TTL_HEAT_SET_PLEAT_PCS] [numeric](15, 4) Not Null,
	[HEAT_SET_PLEAT_Price] [numeric](15, 4) Not Null,
	[TTL_HEAT_SET_PLEAT_Price] [numeric](15, 4) Not Null,
	[HEAT_TRANSFER_PANEL] [numeric](15, 4) Not Null,
	[TTL_HEAT_TRANSFER_PANEL] [numeric](15, 4) Not Null,
	[HEAT_TRANSFER_TMS] [numeric](15, 4) Not Null,
	[HEAT_TRANSFER_CPU] [numeric](15, 4) Not Null,
	[TTL_HEAT_TRANSFER_TMS] [numeric](15, 4) Not Null,
	[TTL_HEAT_TRANSFER_CPU] [numeric](15, 4) Not Null,
	[HF_WELDED_PCS] [numeric](15, 4) Not Null,
	[TTL_HF_WELDED_PCS] [numeric](15, 4) Not Null,
	[HF_WELDED_Price] [numeric](15, 4) Not Null,
	[TTL_HF_WELDED_Price] [numeric](15, 4) Not Null,
	[INDIRECT_MANPOWER_TMS] [numeric](15, 4) Not Null,
	[INDIRECT_MANPOWER_CPU] [numeric](15, 4) Not Null,
	[TTL_INDIRECT_MANPOWER_TMS] [numeric](15, 4) Not Null,
	[TTL_INDIRECT_MANPOWER_CPU] [numeric](15, 4) Not Null,
	[INSPECTION_TMS] [numeric](15, 4) Not Null,
	[INSPECTION_CPU] [numeric](15, 4) Not Null,
	[TTL_INSPECTION_TMS] [numeric](15, 4) Not Null,
	[TTL_INSPECTION_CPU] [numeric](15, 4) Not Null,
	[JOKERTAG_TMS] [numeric](15, 4) Not Null,
	[JOKERTAG_CPU] [numeric](15, 4) Not Null,
	[TTL_JOKERTAG_TMS] [numeric](15, 4) Not Null,
	[TTL_JOKERTAG_CPU] [numeric](15, 4) Not Null,
	[LASER_TMS] [numeric](15, 4) Not Null,
	[LASER_CPU] [numeric](15, 4) Not Null,
	[TTL_LASER_TMS] [numeric](15, 4) Not Null,
	[TTL_LASER_CPU] [numeric](15, 4) Not Null,
	[PAD_PRINTING_PCS] [numeric](15, 4) Not Null,
	[TTL_PAD_PRINTING_PCS] [numeric](15, 4) Not Null,
	[PAD_PRINTING_Price] [numeric](15, 4) Not Null,
	[TTL_PAD_PRINTING_Price] [numeric](15, 4) Not Null,
	[POLYBAG_Price] [numeric](15, 4) Not Null,
	[TTL_POLYBAG_Price] [numeric](15, 4) Not Null,
	[PRINTING_PCS] [numeric](15, 4) Not Null,
	[TTL_PRINTING_PCS] [numeric](15, 4) Not Null,
	[PRINTING_Price] [numeric](15, 4) Not Null,
	[TTL_PRINTING_Price] [numeric](15, 4) Not Null,
	[SP_THREAD_Price] [numeric](15, 4) Not Null,
	[TTL_SP_THREAD_Price] [numeric](15, 4) Not Null,
	[SUBLIMATION_PRINT_TMS] [numeric](15, 4) Not Null,
	[SUBLIMATION_PRINT_CPU] [numeric](15, 4) Not Null,
	[TTL_SUBLIMATION_PRINT_TMS] [numeric](15, 4) Not Null,
	[TTL_SUBLIMATION_PRINT_CPU] [numeric](15, 4) Not Null,
	[SUBLIMATION_ROLLER_TMS] [numeric](15, 4) Not Null,
	[SUBLIMATION_ROLLER_CPU] [numeric](15, 4) Not Null,
	[TTL_SUBLIMATION_ROLLER_TMS] [numeric](15, 4) Not Null,
	[TTL_SUBLIMATION_ROLLER_CPU] [numeric](15, 4) Not Null,
	-----------------------------------------------------
	[Inline_Category] Nvarchar(65) Not Null,
	[Low_output_Reason] Nvarchar(65) Not Null,
	[New_Style_Repeat_style] Varchar(20) Not Null,
	[ArtworkType] Varchar(100) Not Null,
 CONSTRAINT [PK_P_SewingDailyOutput] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[MDivisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CountryID]  DEFAULT ('') FOR [CountryID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Shift]  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SubconOutFty]  DEFAULT ('') FOR [SubconOutFty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SubConOutContractNumber]  DEFAULT ('') FOR [SubConOutContractNumber]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Article]  DEFAULT ('') FOR [Article]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SizeCode]  DEFAULT ('') FOR [SizeCode]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CustPONo]  DEFAULT ('') FOR [CustPONo]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderCategory]  DEFAULT ('') FOR [OrderCategory]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ProgramID]  DEFAULT ('') FOR [ProgramID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_OrderTypeID]  DEFAULT ('') FOR [OrderTypeID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_DevSample]  DEFAULT ('') FOR [DevSample]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CPURate]  DEFAULT ((0)) FOR [CPURate]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CdCodeID]  DEFAULT ('') FOR [CdCodeID]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ActualManpower]  DEFAULT ((0)) FOR [ActualManpower]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_NoOfHours]  DEFAULT ((0)) FOR [NoOfHours]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TotalManhours]  DEFAULT ((0)) FOR [TotalManhours]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TargetCPU]  DEFAULT ((0)) FOR [TargetCPU]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TMS]  DEFAULT ((0)) FOR [TMS]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CPUPrice]  DEFAULT ((0)) FOR [CPUPrice]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TargetQty]  DEFAULT ((0)) FOR [TargetQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TotalOutputQty]  DEFAULT ((0)) FOR [TotalOutputQty]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_TotalCPU]  DEFAULT ((0)) FOR [TotalCPU]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CPUSewerHR]  DEFAULT ((0)) FOR [CPUSewerHR]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_EFF]  DEFAULT ((0)) FOR [EFF]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_RFT]  DEFAULT ((0)) FOR [RFT]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_CumulateOfDays]  DEFAULT ((0)) FOR [CumulateOfDays]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_DateRange]  DEFAULT ('') FOR [DateRange]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_ProdOutput]  DEFAULT ((0)) FOR [ProdOutput]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Diff]  DEFAULT ((0)) FOR [Diff]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_Rate]  DEFAULT ((0)) FOR [Rate]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_SewingReasonDesc]  DEFAULT ('') FOR [SewingReasonDesc]
GO

ALTER TABLE [dbo].[P_SewingDailyOutput] ADD  CONSTRAINT [DF_P_SewingDailyOutput_LockStatus]  DEFAULT ('') FOR [LockStatus]
GO

ALTER Table [dbo].[P_SewingDailyOutput] Add CONSTRAINT [DF_P_SewingDailyOutput_Cancel] DEFAULT ('') FOR [Cancel]
GO

ALTER Table [dbo].[P_SewingDailyOutput] Add CONSTRAINT [DF_P_SewingDailyOutput_Remark] DEFAULT ('') FOR [Remark]
GO

ALTER Table [dbo].[P_SewingDailyOutput] Add CONSTRAINT [DF_P_SewingDailyOutput_SPFactory] DEFAULT ('') FOR [SPFactory]
GO

ALTER Table [dbo].[P_SewingDailyOutput] Add CONSTRAINT [DF_P_SewingDailyOutput_NonRevenue] DEFAULT ('') FOR [NonRevenue]
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_AT_HAND_TMS] DEFAULT ((0)) FOR [AT_HAND_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_AT_HAND_CPU] DEFAULT ((0)) FOR [AT_HAND_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_HAND_TMS] DEFAULT ((0)) FOR [TTL_AT_HAND_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_HAND_CPU] DEFAULT ((0)) FOR [TTL_AT_HAND_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_AT_MACHINE_TMS] DEFAULT ((0)) FOR [AT_MACHINE_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_AT_MACHINE_CPU] DEFAULT ((0)) FOR [AT_MACHINE_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_MACHINE_TMS] DEFAULT ((0)) FOR [TTL_AT_MACHINE_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_MACHINE_CPU] DEFAULT ((0)) FOR [TTL_AT_MACHINE_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_AT_CPU] DEFAULT ((0)) FOR [TTL_AT_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_BONDING_HAND_TMS] DEFAULT ((0)) FOR [BONDING_HAND_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_BONDING_HAND_CPU] DEFAULT ((0)) FOR [BONDING_HAND_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_HAND_TMS] DEFAULT ((0)) FOR [TTL_BONDING_HAND_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_HAND_CPU] DEFAULT ((0)) FOR [TTL_BONDING_HAND_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_BONDING_MACHINE_TMS] DEFAULT ((0)) FOR [BONDING_MACHINE_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_BONDING_MACHINE_CPU] DEFAULT ((0)) FOR [BONDING_MACHINE_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_MACHINE_TMS] DEFAULT ((0)) FOR [TTL_BONDING_MACHINE_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_BONDING_MACHINE_CPU] DEFAULT ((0)) FOR [TTL_BONDING_MACHINE_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_CARTON_Price] DEFAULT ((0)) FOR [CARTON_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_CARTON_Price] DEFAULT ((0)) FOR [TTL_CARTON_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_CUTTING_TMS] DEFAULT ((0)) FOR [CUTTING_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_CUTTING_CPU] DEFAULT ((0)) FOR [CUTTING_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_CUTTING_TMS] DEFAULT ((0)) FOR [TTL_CUTTING_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_CUTTING_CPU] DEFAULT ((0)) FOR [TTL_CUTTING_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_DIE_CUT_TMS] DEFAULT ((0)) FOR [DIE_CUT_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_DIE_CUT_CPU] DEFAULT ((0)) FOR [DIE_CUT_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_DIE_CUT_TMS] DEFAULT ((0)) FOR [TTL_DIE_CUT_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_DIE_CUT_CPU] DEFAULT ((0)) FOR [TTL_DIE_CUT_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_DOWN_TMS] DEFAULT ((0)) FOR [DOWN_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_DOWN_CPU] DEFAULT ((0)) FOR [DOWN_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_DOWN_TMS] DEFAULT ((0)) FOR [TTL_DOWN_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_DOWN_CPU] DEFAULT ((0)) FOR [TTL_DOWN_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EM_DEBOSS_I_H_TMS] DEFAULT ((0)) FOR [EM_DEBOSS_I_H_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EM_DEBOSS_I_H_CPU] DEFAULT ((0)) FOR [EM_DEBOSS_I_H_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EM_DEBOSS_I_H_TMS] DEFAULT ((0)) FOR [TTL_EM_DEBOSS_I_H_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EM_DEBOSS_I_H_CPU] DEFAULT ((0)) FOR [TTL_EM_DEBOSS_I_H_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EM_DEBOSS_MOLD_Price] DEFAULT ((0)) FOR [EM_DEBOSS_MOLD_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EM_DEBOSS_MOLD_Price] DEFAULT ((0)) FOR [TTL_EM_DEBOSS_MOLD_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EMB_THREAD] DEFAULT ((0)) FOR [EMB_THREAD]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMB_THREAD] DEFAULT ((0)) FOR [TTL_EMB_THREAD]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EMBOSS_DEBOSS_PCS] DEFAULT ((0)) FOR [EMBOSS_DEBOSS_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBOSS_DEBOSS_PCS] DEFAULT ((0)) FOR [TTL_EMBOSS_DEBOSS_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EMBOSS_DEBOSS_Price] DEFAULT ((0)) FOR [EMBOSS_DEBOSS_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBOSS_DEBOSS_Price] DEFAULT ((0)) FOR [TTL_EMBOSS_DEBOSS_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EMBROIDERY_Price] DEFAULT ((0)) FOR [EMBROIDERY_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBROIDERY_Price] DEFAULT ((0)) FOR [TTL_EMBROIDERY_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_EMBROIDERY_STITCH] DEFAULT ((0)) FOR [EMBROIDERY_STITCH]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_EMBROIDERY_STITCH] DEFAULT ((0)) FOR [TTL_EMBROIDERY_STITCH]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_FARM_OUT_QUILTING_PCS] DEFAULT ((0)) FOR [FARM_OUT_QUILTING_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_FARM_OUT_QUILTING_PCS] DEFAULT ((0)) FOR [TTL_FARM_OUT_QUILTING_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_FARM_OUT_QUILTING_Price] DEFAULT ((0)) FOR [FARM_OUT_QUILTING_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_FARM_OUT_QUILTING_Price] DEFAULT ((0)) FOR [TTL_FARM_OUT_QUILTING_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_Garment_Dye_PCS] DEFAULT ((0)) FOR [Garment_Dye_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_Garment_Dye_PCS] DEFAULT ((0)) FOR [TTL_Garment_Dye_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_Garment_Dye_Price] DEFAULT ((0)) FOR [Garment_Dye_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_Garment_Dye_Price] DEFAULT ((0)) FOR [TTL_Garment_Dye_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_HAND_TMS] DEFAULT ((0)) FOR [GLUE_BO_HAND_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_HAND_CPU] DEFAULT ((0)) FOR [GLUE_BO_HAND_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_HAND_TMS] DEFAULT ((0)) FOR [TTL_GLUE_BO_HAND_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_HAND_CPU] DEFAULT ((0)) FOR [TTL_GLUE_BO_HAND_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_MACHINE_TMS] DEFAULT ((0)) FOR [GLUE_BO_MACHINE_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GLUE_BO_MACHINE_CPU] DEFAULT ((0)) FOR [GLUE_BO_MACHINE_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_MACHINE_TMS] DEFAULT ((0)) FOR [TTL_GLUE_BO_MACHINE_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GLUE_BO_MACHINE_CPU] DEFAULT ((0)) FOR [TTL_GLUE_BO_MACHINE_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GMT_DRY_PCS] DEFAULT ((0)) FOR [GMT_DRY_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_DRY_PCS] DEFAULT ((0)) FOR [TTL_GMT_DRY_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GMT_DRY_Price] DEFAULT ((0)) FOR [GMT_DRY_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_DRY_Price] DEFAULT ((0)) FOR [TTL_GMT_DRY_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GMT_WASH_PCS] DEFAULT ((0)) FOR [GMT_WASH_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_WASH_PCS] DEFAULT ((0)) FOR [TTL_GMT_WASH_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_GMT_WASH_Price] DEFAULT ((0)) FOR [GMT_WASH_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_GMT_WASH_Price] DEFAULT ((0)) FOR [TTL_GMT_WASH_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_HEAT_SET_PLEAT_PCS] DEFAULT ((0)) FOR [HEAT_SET_PLEAT_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_SET_PLEAT_PCS] DEFAULT ((0)) FOR [TTL_HEAT_SET_PLEAT_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_HEAT_SET_PLEAT_Price] DEFAULT ((0)) FOR [HEAT_SET_PLEAT_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_SET_PLEAT_Price] DEFAULT ((0)) FOR [TTL_HEAT_SET_PLEAT_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_HEAT_TRANSFER_PANEL] DEFAULT ((0)) FOR [HEAT_TRANSFER_PANEL]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_TRANSFER_PANEL] DEFAULT ((0)) FOR [TTL_HEAT_TRANSFER_PANEL]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_HEAT_TRANSFER_TMS] DEFAULT ((0)) FOR [HEAT_TRANSFER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_HEAT_TRANSFER_CPU] DEFAULT ((0)) FOR [HEAT_TRANSFER_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_TRANSFER_TMS] DEFAULT ((0)) FOR [TTL_HEAT_TRANSFER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_HEAT_TRANSFER_CPU] DEFAULT ((0)) FOR [TTL_HEAT_TRANSFER_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_HF_WELDED_PCS] DEFAULT ((0)) FOR [HF_WELDED_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_HF_WELDED_PCS] DEFAULT ((0)) FOR [TTL_HF_WELDED_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_HF_WELDED_Price] DEFAULT ((0)) FOR [HF_WELDED_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_HF_WELDED_Price] DEFAULT ((0)) FOR [TTL_HF_WELDED_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_INDIRECT_MANPOWER_TMS] DEFAULT ((0)) FOR [INDIRECT_MANPOWER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_INDIRECT_MANPOWER_CPU] DEFAULT ((0)) FOR [INDIRECT_MANPOWER_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_INDIRECT_MANPOWER_TMS] DEFAULT ((0)) FOR [TTL_INDIRECT_MANPOWER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_INDIRECT_MANPOWER_CPU] DEFAULT ((0)) FOR [TTL_INDIRECT_MANPOWER_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_INSPECTION_TMS] DEFAULT ((0)) FOR [INSPECTION_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_INSPECTION_CPU] DEFAULT ((0)) FOR [INSPECTION_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_INSPECTION_TMS] DEFAULT ((0)) FOR [TTL_INSPECTION_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_INSPECTION_CPU] DEFAULT ((0)) FOR [TTL_INSPECTION_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_JOKERTAG_TMS] DEFAULT ((0)) FOR [JOKERTAG_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_JOKERTAG_CPU] DEFAULT ((0)) FOR [JOKERTAG_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_JOKERTAG_TMS] DEFAULT ((0)) FOR [TTL_JOKERTAG_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_JOKERTAG_CPU] DEFAULT ((0)) FOR [TTL_JOKERTAG_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_LASER_TMS] DEFAULT ((0)) FOR [LASER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_LASER_CPU] DEFAULT ((0)) FOR [LASER_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_LASER_TMS] DEFAULT ((0)) FOR [TTL_LASER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_LASER_CPU] DEFAULT ((0)) FOR [TTL_LASER_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_PAD_PRINTING_PCS] DEFAULT ((0)) FOR [PAD_PRINTING_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_PAD_PRINTING_PCS] DEFAULT ((0)) FOR [TTL_PAD_PRINTING_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_PAD_PRINTING_Price] DEFAULT ((0)) FOR [PAD_PRINTING_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_PAD_PRINTING_Price] DEFAULT ((0)) FOR [TTL_PAD_PRINTING_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_POLYBAG_Price] DEFAULT ((0)) FOR [POLYBAG_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_POLYBAG_Price] DEFAULT ((0)) FOR [TTL_POLYBAG_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_PRINTING_PCS] DEFAULT ((0)) FOR [PRINTING_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_PRINTING_PCS] DEFAULT ((0)) FOR [TTL_PRINTING_PCS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_PRINTING_Price] DEFAULT ((0)) FOR [PRINTING_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_PRINTING_Price] DEFAULT ((0)) FOR [TTL_PRINTING_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_SP_THREAD_Price] DEFAULT ((0)) FOR [SP_THREAD_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_SP_THREAD_Price] DEFAULT ((0)) FOR [TTL_SP_THREAD_Price]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_PRINT_TMS] DEFAULT ((0)) FOR [SUBLIMATION_PRINT_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_PRINT_CPU] DEFAULT ((0)) FOR [SUBLIMATION_PRINT_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_PRINT_TMS] DEFAULT ((0)) FOR [TTL_SUBLIMATION_PRINT_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_PRINT_CPU] DEFAULT ((0)) FOR [TTL_SUBLIMATION_PRINT_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_ROLLER_TMS] DEFAULT ((0)) FOR [SUBLIMATION_ROLLER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_SUBLIMATION_ROLLER_CPU] DEFAULT ((0)) FOR [SUBLIMATION_ROLLER_CPU]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_ROLLER_TMS] DEFAULT ((0)) FOR [TTL_SUBLIMATION_ROLLER_TMS]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_TTL_SUBLIMATION_ROLLER_CPU] DEFAULT ((0)) FOR [TTL_SUBLIMATION_ROLLER_CPU]
GO

------------------------------------------- -------------------------------------------------------------------------------------------------------
ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_Inline_Inline_Category] DEFAULT ('') FOR [Inline_Category]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_Inline_Low_output_Reason] DEFAULT ('') FOR [Low_output_Reason]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_Inline_New_Style_Repeat_style] DEFAULT ('') FOR [New_Style_Repeat_style]
GO

ALTER Table [dbo].[P_SewingDailyOutput] ADD CONSTRAINT [DF_P_SewingDailyOutput_ArtworkType] DEFAULT ('') FOR [ArtworkType]
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingOutput_Detail_Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manufacturing Division ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組合型態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order or Mockup order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國家別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CountryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發外工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SubconOutFty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發外條款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SubConOutContractNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SizeCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CustPONo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderCategory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ProgramID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'OrderTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'開發樣品 OrderType.IsDevSample轉入寫入 Y/N' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'DevSample'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單CPU Rate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CPURate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CdCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ActualManpower'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平均一人工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'NoOfHours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總人力工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TotalManhours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目標CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TargetCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Costing TMS (sec)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每件需多少CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CPUPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目標數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TargetQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TotalOutputQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'TotalCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直接人員每人每小時產出(PPH)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CPUSewerHR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'效率值EFF(%)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'EFF'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Right First Time(%)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'RFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該Style在這條線上累積做多久' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'CumulateOfDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等同於CumulateOfDays，當大於10則顯示>=10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'DateRange'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InlineQty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ProdOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'差異(QAQty-InlineQty)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Diff'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'比例' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'Rate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原因描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SewingReasonDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'飛雁交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出的Lock狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'LockStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為訂單公司別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutput', @level2type=N'COLUMN',@level2name=N'ArtworkType'
GO