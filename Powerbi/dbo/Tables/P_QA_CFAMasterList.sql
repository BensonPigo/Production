CREATE TABLE [dbo].[P_QA_CFAMasterList] (
    [FinalInsp]       VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_FinalInsp_New] DEFAULT ('') NOT NULL,
    [Thirdinsp]       VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_Thirdinsp_New] DEFAULT ('') NOT NULL,
    [ThirdinspResult] VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_ThirdinspResult_New] DEFAULT ('') NOT NULL,
    [MDivision]       VARCHAR (8000)  NULL,
    [Factory]         VARCHAR (8000)  NULL,
    [BuyerDelivery]   DATE            NULL,
    [Brand]           VARCHAR (8000)  NULL,
    [OrderID]         VARCHAR (8000)  NOT NULL,
    [Catery]          VARCHAR (8000)  NOT NULL,
    [OrderType]       VARCHAR (8000)  NULL,
    [CustPoNo]        VARCHAR (8000)  NULL,
    [Style]           VARCHAR (8000)  NULL,
    [StyleName]       NVARCHAR (1000) CONSTRAINT [DF_P_QA_CFAMasterList_StyleName_New] DEFAULT ('') NOT NULL,
    [Season]          VARCHAR (8000)  NULL,
    [Dest]            VARCHAR (8000)  NOT NULL,
    [GTNPONO]         VARCHAR (8000)  NULL,
    [CustCD]          VARCHAR (8000)  NULL,
    [ShipSeq]         VARCHAR (8000)  NOT NULL,
    [ShipMode]        VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_ShipMode_New] DEFAULT ('') NOT NULL,
    [ColorWay]        VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_ColorWay_New] DEFAULT ('') NOT NULL,
    [SewingLine]      VARCHAR (8000)  NULL,
    [Qty]             INT             CONSTRAINT [DF_P_QA_CFAMasterList_Qty_New] DEFAULT ((0)) NOT NULL,
    [StaggeredOutput] INT             CONSTRAINT [DF_P_QA_CFAMasterList_StaggeredOutput_New] DEFAULT ((0)) NOT NULL,
    [CMPOutput]       VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_CMPOutput_New] DEFAULT ('') NOT NULL,
    [CMPOutputPCT]    VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_CMPOutputPCT_New] DEFAULT ('') NOT NULL,
    [ClogRcvQty]      VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_ClogRcvQty_New] DEFAULT ('') NOT NULL,
    [CLOGRcVQtyPCT]   VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_CLOGRcVQtyPCT_New] DEFAULT ('') NOT NULL,
    [TtlCtn]          VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_TtlCtn_New] DEFAULT ('') NOT NULL,
    [StaggeredCtn]    VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_StaggeredCtn_New] DEFAULT ('') NOT NULL,
    [ClogCtn]         VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_ClogCtn_New] DEFAULT ('') NOT NULL,
    [ClogCtnPCT]      VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_ClogCtnPCT_New] DEFAULT ('') NOT NULL,
    [LastCtnRcvDate]  DATE            NULL,
    [FinalInspDate]   DATE            NULL,
    [Last3rdInspDate] DATE            NULL,
    [Remark]          NVARCHAR (1000) CONSTRAINT [DF_P_QA_CFAMasterList_Remark_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]     VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]    DATETIME        NULL,
    [BIStatus]        VARCHAR (8000)  CONSTRAINT [DF_P_QA_CFAMasterList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_QA_CFAMasterList] PRIMARY KEY CLUSTERED ([OrderID] ASC, [ShipSeq] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最終檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'FinalInsp'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要第三方檢驗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Thirdinsp'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方檢驗結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'ThirdinspResult'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠所屬 M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'MDivision'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單該次出貨的交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI 訂單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類Bulk, Sample, Garment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Catery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'OrderType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客人訂單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'CustPoNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'StyleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣出口的代碼的國家' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Dest'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'
客人訂單號碼
此單號與  PO# 不同

以 LLL 為例
流程上是會先有 PO#
等到正式下單後才會有 GTN PO#
兩個 PO# 都是有意義的
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'GTNPONO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶品牌成衣進口地點代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'CustCD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該訂單出貨編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'ShipSeq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨運輸方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'ShipMode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨成衣的色組清單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'ColorWay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線計畫 - 預排的生產線清單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'SewingLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨的成衣 (含套裝) 數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已完成 Stagger 檢驗的成衣 (含套裝) 數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'StaggeredOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨已生產多少件成衣
如果該訂單有出貨多次
該欄位會呈現 N/A
因為無法確定該訂單目前的生產量
有多少是歸屬在這次出貨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'CMPOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨已生產完成的比例
計算公式 (CMP output)/Qty×100
但有以下 2 種情況會呈現 N/A
	1. 如果該訂單有出貨多次
因為無法確定該訂單目前的生產量
有多少是歸屬在這次出貨
	2. 該次出貨數為 0
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'CMPOutputPCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨已進入 Clog / CFA 的成衣數
如果該訂單為 Sample 單
則會顯示 N/A
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'ClogRcvQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨已放入 Clog / CFA 的成衣比例
計算公式 (CLOG received qty)/Qty×100
但有以下 2 種情況會呈現 N/A
	1. Sample 單
	2. 該次出貨數為 0
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'CLOGRcVQtyPCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨裝入多少個紙箱
如果該訂單為 Sample 單
則會顯示 N/A
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'TtlCtn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨有多少紙箱已完成 Stagger 檢驗
如果該訂單為 Sample 單
則會顯示 N/A
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'StaggeredCtn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨已進入 Clog / CFA 的紙箱數
如果該訂單為 Sample 單
則會顯示 N/A' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'ClogCtn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨已放入 Clog / CFA 的紙箱比例
計算公式 (Clog Ctn)/(Ttl Ctn)×100
但有以下 2 種情況會呈現 N/A
	1. Sample 單
放入 Clog / CFA 的箱數為 0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'ClogCtnPCT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該次出貨所有紙箱皆放入成品倉後
呈現最後一個紙箱放入成品倉的日期
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'LastCtnRcvDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Final Inspection 的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'FinalInspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後一次第三方檢驗的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Last3rdInspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CFA 檢驗後的備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_CFAMasterList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_QA_CFAMasterList', @level2type = N'COLUMN', @level2name = N'BIStatus';

