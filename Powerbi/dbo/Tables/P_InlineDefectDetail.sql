CREATE TABLE [dbo].[P_InlineDefectDetail]
(
	[Ukey]                  BIGINT IDENTITY (1,1), 
    [Zone]                  VARCHAR(6)      CONSTRAINT [DF_P_InlineDefectDetail_Zone]   NOT NULL, 
    [BrandID]               VARCHAR(8)      CONSTRAINT [DF_P_InlineDefectDetail_BrandID]   NOT NULL, 
    [BuyerDelivery]         DATE NULL, 
    [FactoryID]             VARCHAR(8)      CONSTRAINT [DF_P_InlineDefectDetail_FactoryID]   NOT NULL DEFAULT (('')), 
    [Line]                  VARCHAR(5)      CONSTRAINT [DF_P_InlineDefectDetail_Line]   NOT NULL DEFAULT (('')), 
    [Team]                  VARCHAR(10)     CONSTRAINT [DF_P_InlineDefectDetail_Team]   NOT NULL DEFAULT (('')), 
    [Shift]                 VARCHAR(5)      CONSTRAINT [DF_P_InlineDefectDetail_Shift]   NOT NULL DEFAULT (('')), 
    [CustPoNo]              VARCHAR(30)     CONSTRAINT [DF_P_InlineDefectDetail_CustPoNo]   NOT NULL DEFAULT (('')), 
    [StyleID]               VARCHAR(15)     CONSTRAINT [DF_P_InlineDefectDetail_StyleID]   NOT NULL DEFAULT (('')), 
    [OrderId]               VARCHAR(13)     CONSTRAINT [DF_P_InlineDefectDetail_OrderId]   NOT NULL DEFAULT (('')), 
    [Article]               VARCHAR(8)      CONSTRAINT [DF_P_InlineDefectDetail_Article]   NOT NULL DEFAULT (('')), 
    [FirstInspectionDate]   DATE NULL, 
    [FirstInspectedTime]    DATETIME NULL, 
    [InspectedQC]           NVARCHAR(30)    CONSTRAINT [DF_P_InlineDefectDetail_InspectedQC]   NOT NULL DEFAULT (('')), 
    [ProductType]           VARCHAR(10)     CONSTRAINT [DF_P_InlineDefectDetail_ProductType]   NOT NULL DEFAULT (('')), 
    [Operation]             NVARCHAR(50)    CONSTRAINT [DF_P_InlineDefectDetail_Operation]   NOT NULL DEFAULT (('')), 
    [SewerName]             NVARCHAR(80)    CONSTRAINT [DF_P_InlineDefectDetail_SewerName]   NOT NULL DEFAULT (('')), 
    [GarmentDefectTypeID]   VARCHAR(3)      CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectTypeID]   NOT NULL DEFAULT (('')), 
    [GarmentDefectTypeDesc] NVARCHAR(60)    CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectTypeDesc]   NOT NULL DEFAULT (('')), 
    [GarmentDefectCodeID]   VARCHAR(5)      CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectCodeID]   NOT NULL DEFAULT (('')), 
    [GarmentDefectCodeDesc] NVARCHAR(100)   CONSTRAINT [DF_P_InlineDefectDetail_GarmentDefectCodeDesc]   NOT NULL DEFAULT (('')), 
    [IsCriticalDefect]      VARCHAR         CONSTRAINT [DF_P_InlineDefectDetail_IsCriticalDefect]   NOT NULL DEFAULT (('')), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (('')), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_InlineDefectDetail] PRIMARY KEY ([Ukey])

)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為嚴重defect code',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'IsCriticalDefect'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產線線別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'Line'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'組別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'Team'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'班別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'Shift'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'客戶訂單單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'CustPoNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'款式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'StyleID'
GO

GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'首次檢驗日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'FirstInspectionDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠地區別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'Zone'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'客戶交期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'BuyerDelivery'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'OrderId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'Article'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'首次檢驗時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'FirstInspectedTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Inspected QC',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'InspectedQC'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗位置',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'ProductType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'Operation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Sewer NM',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'SewerName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'瑕疵種類代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'GarmentDefectTypeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'瑕疵種類描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'GarmentDefectTypeDesc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'瑕疵代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'GarmentDefectCodeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'瑕疵描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'GarmentDefectCodeDesc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'P_InlineDefectDetail'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_InlineDefectDetail',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'