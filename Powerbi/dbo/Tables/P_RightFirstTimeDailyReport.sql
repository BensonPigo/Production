CREATE TABLE [dbo].[P_RightFirstTimeDailyReport] (
    [FactoryID]     VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_FactoryID_New] DEFAULT ('') NOT NULL,
    [CDate]         DATE            NOT NULL,
    [OrderID]       VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_OrderID_New] DEFAULT ('') NOT NULL,
    [Destination]   VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Destination_New] DEFAULT ('') NOT NULL,
    [BrandID]       VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]       VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_StyleID_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery] DATE            NULL,
    [CDCodeID]      VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_CDCodeID_New] DEFAULT ('') NOT NULL,
    [CDCodeNew]     VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_CDCodeNew_New] DEFAULT ('') NOT NULL,
    [ProductType]   NVARCHAR (1000) CONSTRAINT [DF_P_RightFirstTimeDailyReport_ProductType_New] DEFAULT ('') NOT NULL,
    [FabricType]    NVARCHAR (1000) CONSTRAINT [DF_P_RightFirstTimeDailyReport_FabricType_New] DEFAULT ('') NOT NULL,
    [Lining]        VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Lining_New] DEFAULT ('') NOT NULL,
    [Gender]        VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Gender_New] DEFAULT ('') NOT NULL,
    [Construction]  NVARCHAR (1000) CONSTRAINT [DF_P_RightFirstTimeDailyReport_Construction_New] DEFAULT ('') NOT NULL,
    [Team]          VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Team_New] DEFAULT ('') NOT NULL,
    [Shift]         VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Shift_New] DEFAULT ('') NOT NULL,
    [Line]          VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Line_New] DEFAULT ('') NOT NULL,
    [Cell]          VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Cell_New] DEFAULT ('') NOT NULL,
    [InspectQty]    NUMERIC (38)    CONSTRAINT [DF_P_RightFirstTimeDailyReport_InspectQty_New] DEFAULT ((0)) NOT NULL,
    [RejectQty]     NUMERIC (38)    CONSTRAINT [DF_P_RightFirstTimeDailyReport_RejectQty_New] DEFAULT ((0)) NOT NULL,
    [RFTPercentage] NUMERIC (38, 2) CONSTRAINT [DF_P_RightFirstTimeDailyReport_RFTPercentage_New] DEFAULT ((0)) NOT NULL,
    [Over]          VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_Over_New] DEFAULT ('') NOT NULL,
    [QC]            DECIMAL (18, 4) CONSTRAINT [DF_P_RightFirstTimeDailyReport_QC_New] DEFAULT ((0)) NOT NULL,
    [Remark]        NVARCHAR (1000) CONSTRAINT [DF_P_RightFirstTimeDailyReport_Remark_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]   VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]  DATETIME        NULL,
    [BIStatus]      VARCHAR (8000)  CONSTRAINT [DF_P_RightFirstTimeDailyReport_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_RightFirstTimeDailyReport] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [CDate] ASC, [OrderID] ASC, [Team] ASC, [Shift] ASC, [Line] ASC)
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

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'OrderID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'BrandID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'StyleID';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CD#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'CDCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CDCodeNew' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'CDCodeNew'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ReasonName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ReasonName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'FabricType'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'襯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Lining';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'性別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Gender';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Construction' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'Construction'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Team';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Shift';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Line';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Cell';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'InspectQty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退回數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'RejectQty';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RFT(%)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'RFTPercentage'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Over';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_RightFirstTimeDailyReport', @level2type=N'COLUMN',@level2name=N'QC'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'Remark';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'記錄哪間工廠的資料，ex PH1, PH2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'BIFactoryID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'時間戳記，紀錄寫入table時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'BIInsertDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_RightFirstTimeDailyReport', @level2type = N'COLUMN', @level2name = N'BIStatus';

