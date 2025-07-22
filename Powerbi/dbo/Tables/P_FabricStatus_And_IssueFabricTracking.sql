CREATE TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking] (
    [SewingCell]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SewingCell_New] DEFAULT ('') NOT NULL,
    [LineID]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_LineID_New] DEFAULT ('') NOT NULL,
    [ReplacementID]           VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_ReplacementID_New] DEFAULT ('') NOT NULL,
    [StyleID]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_StyleID_New] DEFAULT ('') NOT NULL,
    [SP]                      VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SP_New] DEFAULT ('') NOT NULL,
    [Seq]                     VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Seq_New] DEFAULT ('') NOT NULL,
    [FabricType]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_FabricType_New] DEFAULT ('') NOT NULL,
    [Color]                   NVARCHAR (1000) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Color_New] DEFAULT ('') NOT NULL,
    [RefNo]                   VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_RefNo_New] DEFAULT ('') NOT NULL,
    [ApvDate]                 DATETIME        NULL,
    [NoOfPcsRejected]         INT             CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_NoOfPcsRejected_New] DEFAULT ((0)) NOT NULL,
    [RequestQtyYrds]          NUMERIC (38, 2) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_RequestQtyYrds_New] DEFAULT ((0)) NOT NULL,
    [IssueQtyYrds]            NUMERIC (38, 2) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_IssueQtyYrds_New] DEFAULT ((0)) NOT NULL,
    [ReplacementFinishedDate] DATETIME        NULL,
    [Type]                    VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Type_New] DEFAULT ('') NOT NULL,
    [Process]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Process_New] DEFAULT ('') NOT NULL,
    [Description]             NVARCHAR (1000) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Description_New] DEFAULT ('') NOT NULL,
    [OnTime]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_OnTime_New] DEFAULT ('') NOT NULL,
    [Remark]                  NVARCHAR (1000) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Remark_New] DEFAULT ('') NOT NULL,
    [Department]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_Department_New] DEFAULT ('') NOT NULL,
    [DetailRemark]            NVARCHAR (1000) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_DetailRemark_New] DEFAULT ('') NOT NULL,
    [StyleName]               NVARCHAR (1000) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_StyleName_New] DEFAULT ('') NOT NULL,
    [FactoryID]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_FactoryID_New] DEFAULT ('') NOT NULL,
    [MaterialType]            NVARCHAR (1000) CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_MaterialType_New] DEFAULT ('') NOT NULL,
    [SewingQty]               INT             CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_SewingQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]            DATETIME        CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_BIInsertDate_New] DEFAULT ('') NULL,
    [BIStatus]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricStatus_And_IssueFabricTracking] PRIMARY KEY CLUSTERED ([ReplacementID] ASC, [SP] ASC, [Seq] ASC, [RefNo] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'補料報告表身備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking', @level2type=N'COLUMN',@level2name=N'DetailRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking', @level2type=N'COLUMN',@level2name=N'StyleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking', @level2type=N'COLUMN',@level2name=N'MaterialType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Output Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking', @level2type=N'COLUMN',@level2name=N'SewingQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricStatus_And_IssueFabricTracking', @level2type = N'COLUMN', @level2name = N'BIStatus';

