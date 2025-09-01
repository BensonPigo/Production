CREATE TABLE [dbo].[P_DQSDefect_Detail] (
    [FtyZon]               VARCHAR (8000)  NULL,
    [BrandID]              VARCHAR (8000)  NULL,
    [BuyerDelivery]        DATE            NULL,
    [Line]                 VARCHAR (8000)  NULL,
    [FactoryID]            VARCHAR (8000)  NOT NULL,
    [Team]                 VARCHAR (8000)  NULL,
    [Shift]                VARCHAR (8000)  NULL,
    [POID]                 VARCHAR (8000)  NULL,
    [StyleID]              VARCHAR (8000)  NULL,
    [SPNO]                 VARCHAR (8000)  NULL,
    [Article]              VARCHAR (8000)  NULL,
    [Status]               VARCHAR (8000)  NULL,
    [FixType]              VARCHAR (8000)  NULL,
    [FirstInspectDate]     DATE            NULL,
    [FirstInspectTime]     TIME (7)        NULL,
    [InspectQCName]        NVARCHAR (1000) NULL,
    [FixedTime]            VARCHAR (8000)  NULL,
    [FixedQCName]          NVARCHAR (1000) NULL,
    [ProductType]          VARCHAR (8000)  NULL,
    [SizeCode]             VARCHAR (8000)  NULL,
    [DefectTypeDesc]       NVARCHAR (1000) NULL,
    [DefectCodeDesc]       NVARCHAR (1000) NULL,
    [AreaCode]             VARCHAR (8000)  NULL,
    [ReworkCardNo]         VARCHAR (8000)  NULL,
    [GarmentDefectTypeID]  VARCHAR (8000)  NULL,
    [GarmentDefectCodeID]  VARCHAR (8000)  NULL,
    [Ukey]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [DefectCodeLocalDesc]  NVARCHAR (1000) CONSTRAINT [DF_P_DQSDefect_Detail_DefectCodeLocalDesc] DEFAULT ('') NOT NULL,
    [IsCriticalDefect]     VARCHAR (8000)  CONSTRAINT [DF_P_DQSDefect_Detail_IsCriticalDefect] DEFAULT ('') NOT NULL,
    [InspectionDetailUkey] BIGINT          CONSTRAINT [DF_P_DQSDefect_Detail_InspectionDetailUkey] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_DQSDefect_Detail_BIFactoryID] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_DQSDefect_Detail_BIStatus] DEFAULT (N'New') NULL,
	[InspectQCID]          VARCHAR (8000)  CONSTRAINT [DF_P_DQSDefect_Detail_InspectQCID] DEFAULT ('') NOT NULL,
	[InspectionDate]       DATE			   NULL,
    CONSTRAINT [PK_P_DQSDefect_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectCode當地描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'DefectCodeLocalDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為嚴重defect code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'IsCriticalDefect'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_DQSDefect_Detail', @level2type = N'COLUMN', @level2name = N'BIStatus';

