CREATE TABLE [dbo].[P_CuttingBCS] (
    [MDivisionID]                VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]                  VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_FactoryID_New] DEFAULT ('') NOT NULL,
    [BrandID]                    VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]                    VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_StyleID_New] DEFAULT ('') NOT NULL,
    [SeasonID]                   VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_SeasonID_New] DEFAULT ('') NOT NULL,
    [CDCodeNew]                  VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_CDCodeNew_New] DEFAULT ('') NOT NULL,
    [FabricType]                 VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_FabricType_New] DEFAULT ('') NOT NULL,
    [POID]                       VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_POID_New] DEFAULT ('') NOT NULL,
    [Category]                   VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_Category_New] DEFAULT ('') NOT NULL,
    [WorkType]                   VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_WorkType_New] DEFAULT ('') NOT NULL,
    [MatchFabric]                VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_MatchFabric_New] DEFAULT ('') NOT NULL,
    [OrderID]                    VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_OrderID_New] DEFAULT ('') NOT NULL,
    [SciDelivery]                DATE           NULL,
    [BuyerDelivery]              DATE           NULL,
    [OrderQty]                   INT            CONSTRAINT [DF_P_CuttingBCS_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [SewInLineDate]              DATE           NULL,
    [SewOffLineDate]             DATE           NULL,
    [SewingLineID]               VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_SewingLineID_New] DEFAULT ('') NOT NULL,
    [RequestDate]                DATE           NOT NULL,
    [StdQty]                     INT            CONSTRAINT [DF_P_CuttingBCS_StdQty_New] DEFAULT ((0)) NOT NULL,
    [StdQtyByLine]               INT            CONSTRAINT [DF_P_CuttingBCS_StdQtyByLine_New] DEFAULT ((0)) NOT NULL,
    [AccuStdQty]                 INT            CONSTRAINT [DF_P_CuttingBCS_AccuStdQty_New] DEFAULT ((0)) NOT NULL,
    [AccuStdQtyByLine]           INT            CONSTRAINT [DF_P_CuttingBCS_AccuStdQtyByLine_New] DEFAULT ((0)) NOT NULL,
    [AccuEstCutQty]              INT            CONSTRAINT [DF_P_CuttingBCS_AccuEstCutQty_New] DEFAULT ((0)) NOT NULL,
    [AccuEstCutQtyByLine]        INT            CONSTRAINT [DF_P_CuttingBCS_AccuEstCutQtyByLine_New] DEFAULT ((0)) NOT NULL,
    [SupplyCutQty]               INT            CONSTRAINT [DF_P_CuttingBCS_SupplyCutQty_New] DEFAULT ((0)) NOT NULL,
    [SupplyCutQtyByLine]         INT            CONSTRAINT [DF_P_CuttingBCS_SupplyCutQtyByLine_New] DEFAULT ((0)) NOT NULL,
    [BalanceCutQty]              INT            CONSTRAINT [DF_P_CuttingBCS_BalanceCutQty_New] DEFAULT ((0)) NOT NULL,
    [BalanceCutQtyByLine]        INT            CONSTRAINT [DF_P_CuttingBCS_BalanceCutQtyByLine_New] DEFAULT ((0)) NOT NULL,
    [SupplyCutQtyVSStdQty]       INT            CONSTRAINT [DF_P_CuttingBCS_SupplyCutQtyVSStdQty_New] DEFAULT ((0)) NOT NULL,
    [SupplyCutQtyVSStdQtyByLine] INT            CONSTRAINT [DF_P_CuttingBCS_SupplyCutQtyVSStdQtyByLine_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]                VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]               DATETIME       NULL,
    [BIStatus]                   VARCHAR (8000) CONSTRAINT [DF_P_CuttingBCS_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CuttingBCS] PRIMARY KEY CLUSTERED ([OrderID] ASC, [SewingLineID] ASC, [RequestDate] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報產出的M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報產出的工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'母單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工單轉置方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'WorkType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'特殊body對格對條的布種' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'MatchFabric'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SciDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計的SewingDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SewInLineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計的Sewing offline Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SewOffLineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計的SewingDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'RequestDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該天的標準產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'StdQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該天該產線的的標準產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'StdQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'累計std qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuStdQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'累計std qty by line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuStdQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪量，用來與Accu Std qty比對' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuEstCutQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪量，用來與Accu Std qty by line比對' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'AccuEstCutQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預期裁剪供給量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預期裁剪供給量 by line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'如果預期裁剪供給量大於StdQty則帶入Stdqty，反之則帶入預期裁剪供給量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQtyVSStdQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'如果預期裁剪供給量大於StdQty則帶入Stdqty，反之則帶入預期裁剪供給量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'SupplyCutQtyVSStdQtyByLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingBCS', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingBCS', @level2type = N'COLUMN', @level2name = N'BIStatus';

