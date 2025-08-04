CREATE TABLE [dbo].[P_InlineDefectDetail] (
    [Ukey]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Zone]                  VARCHAR (8000)  NOT NULL,
    [BrandID]               VARCHAR (8000)  NOT NULL,
    [BuyerDelivery]         DATE            NULL,
    [FactoryID]             VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_FactoryID_New] DEFAULT ('') NOT NULL,
    [Line]                  VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_Line_New] DEFAULT ('') NOT NULL,
    [Team]                  VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_Team_New] DEFAULT ('') NOT NULL,
    [Shift]                 VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_Shift_New] DEFAULT ('') NOT NULL,
    [CustPoNo]              VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_CustPoNo_New] DEFAULT ('') NOT NULL,
    [StyleID]               VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_StyleID_New] DEFAULT ('') NOT NULL,
    [OrderId]               VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_OrderId_New] DEFAULT ('') NOT NULL,
    [Article]               VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_Article_New] DEFAULT ('') NOT NULL,
    [FirstInspectionDate]   DATE            NULL,
    [FirstInspectedTime]    DATETIME        NULL,
    [InspectedQC]           NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectDetail_InspectedQC_New] DEFAULT ('') NOT NULL,
    [ProductType]           VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_ProductType_New] DEFAULT ('') NOT NULL,
    [Operation]             NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectDetail_Operation_New] DEFAULT ('') NOT NULL,
    [SewerName]             NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectDetail_SewerName_New] DEFAULT ('') NOT NULL,
    [GarmentDefectTypeID]   VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectTypeID_New] DEFAULT ('') NOT NULL,
    [GarmentDefectTypeDesc] NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectTypeDesc_New] DEFAULT ('') NOT NULL,
    [GarmentDefectCodeID]   VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectCodeID_New] DEFAULT ('') NOT NULL,
    [GarmentDefectCodeDesc] NVARCHAR (1000) CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectCodeDesc_New] DEFAULT ('') NOT NULL,
    [IsCriticalDefect]      VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_IsCriticalDefect_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]           VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]          DATETIME        NULL,
    [BIStatus]              VARCHAR (8000)  CONSTRAINT [DF_P_InlineDefectDetail_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_InlineDefectDetail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠地區別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Zone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線線別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'CustPoNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'OrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次檢驗日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'FirstInspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次檢驗時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'FirstInspectedTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inspected QC' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'InspectedQC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'Operation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewer NM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'SewerName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵種類代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵種類描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectTypeDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectCodeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'GarmentDefectCodeDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為嚴重defect code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'IsCriticalDefect'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InlineDefectDetail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_InlineDefectDetail', @level2type = N'COLUMN', @level2name = N'BIStatus';

