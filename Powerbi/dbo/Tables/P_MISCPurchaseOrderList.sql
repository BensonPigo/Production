CREATE TABLE [dbo].[P_MISCPurchaseOrderList](
	[PurchaseFrom] [varchar](6) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[PONo] [varchar](13) NOT NULL,
	[PRConfirmedDate] [datetime] NULL,
	[CreateDate] [date] NULL,
	[DeliveryDate] [date] NULL,
	[Type] [varchar](20) NOT NULL,
	[Supplier] [nvarchar](30) NOT NULL,
	[Status] [varchar](15) NOT NULL,
	[ReqNo] [varchar](13) NOT NULL,
	[PRDate] [date] NULL,
	[Code] [varchar](23) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[POQty] [decimal](8, 2) NOT NULL,
	[UnitID] [varchar](8) NOT NULL,
	[CurrencyID] [varchar](3) NOT NULL,
	[UnitPrice] [numeric](16, 4) NOT NULL,
	[UnitPrice_USD] [numeric](16, 4) NOT NULL,
	[POAmount] [numeric](25, 6) NOT NULL,
	[POAmount_USD] [numeric](16, 4) NOT NULL,
	[AccInQty] [decimal](8, 2) NOT NULL,
	[TPEPO] [varchar](13) NOT NULL,
	[TPEQty] [numeric](8, 2) NOT NULL,
	[TPECurrencyID] [varchar](3) NOT NULL,
	[TPEPrice] [numeric](16, 4) NOT NULL,
	[TPEAmount] [numeric](25, 6) NOT NULL,
	[ApQty] [decimal](8, 2) NOT NULL,
	[APAmount] [numeric](23, 6) NOT NULL,
	[RentalDay] [int] NOT NULL,
	[IncomingDate] [date] NULL,
	[APApprovedDate] [date] NULL,
	[Invoice] [varchar](max) NOT NULL,
	[RequestReason] [nvarchar](300) NOT NULL,
	[ProjectItem] [varchar](1) NOT NULL,
	[Project] [varchar](211) NOT NULL,
	[DepartmentID] [varchar](8) NOT NULL,
	[AccountID] [varchar](8) NOT NULL,
	[AccountName] [nvarchar](40) NOT NULL,
	[AccountCategory] [nvarchar](50) NOT NULL,
	[Budget] [varchar](100) NOT NULL,
	[InternalRemarks] [nvarchar](max) NOT NULL,
	[APID] [varchar](300) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_MISCPurchaseOrderList] PRIMARY KEY CLUSTERED 
(
	[PONo] ASC,
	[Code] ASC,
	[ReqNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_PurchaseFrom]  DEFAULT ('') FOR [PurchaseFrom]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_PONo]  DEFAULT ('') FOR [PONo]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Type]  DEFAULT ('') FOR [Type]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Supplier]  DEFAULT ('') FOR [Supplier]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Status]  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_ReqNo]  DEFAULT ('') FOR [ReqNo]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Code]  DEFAULT ('') FOR [Code]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Description]  DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_POQty]  DEFAULT ((0)) FOR [POQty]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_UnitID]  DEFAULT ('') FOR [UnitID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_CurrencyID]  DEFAULT ('') FOR [CurrencyID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_UnitPrice_USD]  DEFAULT ((0)) FOR [UnitPrice_USD]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_POAmount]  DEFAULT ((0)) FOR [POAmount]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_POAmount_USD]  DEFAULT ((0)) FOR [POAmount_USD]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_AccInQty]  DEFAULT ((0)) FOR [AccInQty]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEPO]  DEFAULT ('') FOR [TPEPO]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEQty]  DEFAULT ((0)) FOR [TPEQty]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_TPECurrencyID]  DEFAULT ('') FOR [TPECurrencyID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEPrice]  DEFAULT ((0)) FOR [TPEPrice]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEAmount]  DEFAULT ((0)) FOR [TPEAmount]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_ApQty]  DEFAULT ((0)) FOR [ApQty]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_APAmount]  DEFAULT ((0)) FOR [APAmount]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_RentalDay]  DEFAULT ((0)) FOR [RentalDay]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Invoice]  DEFAULT ('') FOR [Invoice]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_RequestReason]  DEFAULT ('') FOR [RequestReason]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_ProjectItem]  DEFAULT ('') FOR [ProjectItem]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Project]  DEFAULT ('') FOR [Project]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_DepartmentID]  DEFAULT ('') FOR [DepartmentID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_AccountID]  DEFAULT ('') FOR [AccountID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_AccountName]  DEFAULT ('') FOR [AccountName]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_AccountCategory]  DEFAULT ('') FOR [AccountCategory]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_Budget]  DEFAULT ('') FOR [Budget]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_InternalRemarks]  DEFAULT ('') FOR [InternalRemarks]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_APID]  DEFAULT ('') FOR [APID]
GO

ALTER TABLE [dbo].[P_MISCPurchaseOrderList] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MISCPurchaseOrderList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MISCPurchaseOrderList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO