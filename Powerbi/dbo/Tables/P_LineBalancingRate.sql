CREATE TABLE [dbo].[P_LineBalancingRate] (
    [SewingDate]           DATE            NOT NULL,
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_LineBalancingRate_FactoryID_New] DEFAULT ('') NOT NULL,
    [Total SP Qty]         INT             CONSTRAINT [DF_P_LineBalancingRate_Total SP Qty_New] DEFAULT ((0)) NOT NULL,
    [Total LBR]            NUMERIC (38, 2) CONSTRAINT [DF_P_LineBalancingRate_Total LBR_New] DEFAULT ((0)) NOT NULL,
    [Avg. LBR]             NUMERIC (38, 2) CONSTRAINT [DF_P_LineBalancingRate_Avg. LBR_New] DEFAULT ((0)) NOT NULL,
    [Total SP Qty In7Days] INT             CONSTRAINT [DF_P_LineBalancingRate_Total SP Qty In7Days_New] DEFAULT ((0)) NOT NULL,
    [Total LBR In7Days]    NUMERIC (38, 2) CONSTRAINT [DF_P_LineBalancingRate_Total LBR In7Days_New] DEFAULT ((0)) NOT NULL,
    [Avg. LBR In7Days]     NUMERIC (38, 2) CONSTRAINT [DF_P_LineBalancingRate_Avg. LBR In7Days_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_LineBalancingRate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_LineBalancingRate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_LineBalancingRate] PRIMARY KEY CLUSTERED ([SewingDate] ASC, [FactoryID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'SewingDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當日有在生產線上的訂單總數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total SP Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當日總LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total LBR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當日平均LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Avg. LBR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未來7天(不含當日)在生產線上的訂單總數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total SP Qty In7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未來7天(不含當日)總LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Total LBR In7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未來7天(不含當日)平均LBR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'Avg. LBR In7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineBalancingRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_LineBalancingRate', @level2type = N'COLUMN', @level2name = N'BIStatus';

