CREATE TABLE [dbo].[P_MtlStatusAnalisis](
	[WK] [varchar](13) NOT NULL,
	[LoadingCountry] [varchar](2) NOT NULL,
	[LoadingPort] [varchar](20) NOT NULL,
	[Shipmode] [varchar](10) NOT NULL,
	[Close_Date] [date] NULL,
	[ETD] [date] NULL,
	[ETA] [date] NULL,
	[Arrive_WH_Date] [date] NULL,
	[KPI_LETA] [date] NULL,
	[Prod_LT] [int] NOT NULL,
	[WK_Factory] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SPNo] [varchar](13) NOT NULL,
	[SEQ] [varchar](max) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[PF_ETA] [date] NULL,
	[SewinLine] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[SCIDelivery] [date] NULL,
	[PO_SMR] [nvarchar](69) NOT NULL,
	[PO_Handle] [nvarchar](69) NOT NULL,
	[SMR] [nvarchar](69) NOT NULL,
	[MR] [nvarchar](69) NOT NULL,
	[PC_Handle] [nvarchar](69) NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[SP_List] [varchar](max) NOT NULL,
	[PO_Qty] [numeric](10, 0) NULL,
	[Project] [varchar](5) NOT NULL,
	[Early_Ship_Reason] [varchar](max) NOT NULL,
	[WK_Handle] [nvarchar](69) NOT NULL,
	[MTL_Confirm] [varchar](1) NOT NULL,
	[Duty] [nvarchar](600) NOT NULL,
	[PF_Remark] [varchar](max) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_MtlStatusAnalisis] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_WK]  DEFAULT ('') FOR [WK]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_LoadingCountry]  DEFAULT ('') FOR [LoadingCountry]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_LoadingPort]  DEFAULT ('') FOR [LoadingPort]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Shipmode]  DEFAULT ('') FOR [Shipmode]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Prod_LT]  DEFAULT ((0)) FOR [Prod_LT]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_WK_Factory]  DEFAULT ('') FOR [WK_Factory]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_SPNo]  DEFAULT ('') FOR [SPNo]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_SEQ]  DEFAULT ('') FOR [SEQ]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Category]  DEFAULT ('') FOR [Category]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_PO_SMR]  DEFAULT ('') FOR [PO_SMR]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_PO_Handle]  DEFAULT ('') FOR [PO_Handle]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_SMR]  DEFAULT ('') FOR [SMR]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_MR]  DEFAULT ('') FOR [MR]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_PC_Handle]  DEFAULT ('') FOR [PC_Handle]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_SP_List]  DEFAULT ('') FOR [SP_List]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_PO_Qty]  DEFAULT ((0)) FOR [PO_Qty]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Project]  DEFAULT ('') FOR [Project]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Early_Ship_Reason]  DEFAULT ('') FOR [Early_Ship_Reason]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_WK_Handle]  DEFAULT ('') FOR [WK_Handle]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_MTL_Confirm]  DEFAULT ('') FOR [MTL_Confirm]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Duty]  DEFAULT ('') FOR [Duty]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_PF_Remark]  DEFAULT ('') FOR [PF_Remark]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[P_MtlStatusAnalisis] ADD  CONSTRAINT [DF_P_MtlStatusAnalisis_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'WK'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LoadingCountry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'LoadingCountry'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LoadingPort' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'LoadingPort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipmode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Shipmode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Close_Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Close_Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ETD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'ETD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Arrive_WH_Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Arrive_WH_Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI_LETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'KPI_LETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prod_LT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Prod_LT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK_Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'WK_Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SPNo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SEQ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Category' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PF_ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PF_ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewinLine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SewinLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCIDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO_SMR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PO_SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO_Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PO_Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'MR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PC_Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PC_Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SP_List' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SP_List'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO_Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PO_Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Project'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Early_Ship_Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Early_Ship_Reason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK_Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'WK_Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MTL_Confirm' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'MTL_Confirm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Duty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Duty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PF_Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PF_Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO