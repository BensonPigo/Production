CREATE TABLE [dbo].[P_MISCPurchaseOrderList] (
    [PurchaseFrom]    VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_PurchaseFrom_New] DEFAULT ('') NOT NULL,
    [MDivisionID]     VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_FactoryID_New] DEFAULT ('') NOT NULL,
    [PONo]            VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_PONo_New] DEFAULT ('') NOT NULL,
    [PRConfirmedDate] DATETIME        NULL,
    [CreateDate]      DATE            NULL,
    [DeliveryDate]    DATE            NULL,
    [Type]            VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_Type_New] DEFAULT ('') NOT NULL,
    [Supplier]        NVARCHAR (1000) CONSTRAINT [DF_P_MISCPurchaseOrderList_Supplier_New] DEFAULT ('') NOT NULL,
    [Status]          VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_Status_New] DEFAULT ('') NOT NULL,
    [ReqNo]           VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_ReqNo_New] DEFAULT ('') NOT NULL,
    [PRDate]          DATE            NULL,
    [Code]            VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_Code_New] DEFAULT ('') NOT NULL,
    [Description]     NVARCHAR (1000) CONSTRAINT [DF_P_MISCPurchaseOrderList_Description_New] DEFAULT ('') NOT NULL,
    [POQty]           DECIMAL (18, 2) CONSTRAINT [DF_P_MISCPurchaseOrderList_POQty_New] DEFAULT ((0)) NOT NULL,
    [UnitID]          VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_UnitID_New] DEFAULT ('') NOT NULL,
    [CurrencyID]      VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_CurrencyID_New] DEFAULT ('') NOT NULL,
    [UnitPrice]       NUMERIC (38, 4) CONSTRAINT [DF_P_MISCPurchaseOrderList_UnitPrice_New] DEFAULT ((0)) NOT NULL,
    [UnitPrice_USD]   NUMERIC (38, 4) CONSTRAINT [DF_P_MISCPurchaseOrderList_UnitPrice_USD_New] DEFAULT ((0)) NOT NULL,
    [POAmount]        NUMERIC (38, 6) CONSTRAINT [DF_P_MISCPurchaseOrderList_POAmount_New] DEFAULT ((0)) NOT NULL,
    [POAmount_USD]    NUMERIC (38, 4) CONSTRAINT [DF_P_MISCPurchaseOrderList_POAmount_USD_New] DEFAULT ((0)) NOT NULL,
    [AccInQty]        DECIMAL (18, 2) CONSTRAINT [DF_P_MISCPurchaseOrderList_AccInQty_New] DEFAULT ((0)) NOT NULL,
    [TPEPO]           VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEPO_New] DEFAULT ('') NOT NULL,
    [TPEQty]          NUMERIC (38, 2) CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEQty_New] DEFAULT ((0)) NOT NULL,
    [TPECurrencyID]   VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_TPECurrencyID_New] DEFAULT ('') NOT NULL,
    [TPEPrice]        NUMERIC (38, 4) CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEPrice_New] DEFAULT ((0)) NOT NULL,
    [TPEAmount]       NUMERIC (38, 6) CONSTRAINT [DF_P_MISCPurchaseOrderList_TPEAmount_New] DEFAULT ((0)) NOT NULL,
    [ApQty]           DECIMAL (18, 2) CONSTRAINT [DF_P_MISCPurchaseOrderList_ApQty_New] DEFAULT ((0)) NOT NULL,
    [APAmount]        NUMERIC (38, 6) CONSTRAINT [DF_P_MISCPurchaseOrderList_APAmount_New] DEFAULT ((0)) NOT NULL,
    [RentalDay]       INT             CONSTRAINT [DF_P_MISCPurchaseOrderList_RentalDay_New] DEFAULT ((0)) NOT NULL,
    [IncomingDate]    DATE            NULL,
    [APApprovedDate]  DATE            NULL,
    [Invoice]         VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_Invoice_New] DEFAULT ('') NOT NULL,
    [RequestReason]   NVARCHAR (1000) CONSTRAINT [DF_P_MISCPurchaseOrderList_RequestReason_New] DEFAULT ('') NOT NULL,
    [ProjectItem]     VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_ProjectItem_New] DEFAULT ('') NOT NULL,
    [Project]         VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_Project_New] DEFAULT ('') NOT NULL,
    [DepartmentID]    VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_DepartmentID_New] DEFAULT ('') NOT NULL,
    [AccountID]       VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_AccountID_New] DEFAULT ('') NOT NULL,
    [AccountName]     NVARCHAR (1000) CONSTRAINT [DF_P_MISCPurchaseOrderList_AccountName_New] DEFAULT ('') NOT NULL,
    [AccountCategory] NVARCHAR (1000) CONSTRAINT [DF_P_MISCPurchaseOrderList_AccountCategory_New] DEFAULT ('') NOT NULL,
    [Budget]          VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_Budget_New] DEFAULT ('') NOT NULL,
    [InternalRemarks] NVARCHAR (MAX)  CONSTRAINT [DF_P_MISCPurchaseOrderList_InternalRemarks_New] DEFAULT ('') NOT NULL,
    [APID]            VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_APID_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]     VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]    DATETIME        NULL,
    [BIStatus]        VARCHAR (8000)  CONSTRAINT [DF_P_MISCPurchaseOrderList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_MISCPurchaseOrderList] PRIMARY KEY CLUSTERED ([PONo] ASC, [Code] ASC, [ReqNo] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MISCPurchaseOrderList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MISCPurchaseOrderList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_MISCPurchaseOrderList', @level2type = N'COLUMN', @level2name = N'BIStatus';

