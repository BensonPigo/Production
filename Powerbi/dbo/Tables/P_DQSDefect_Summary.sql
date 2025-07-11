CREATE TABLE [dbo].[P_DQSDefect_Summary] (
    [FirstInspectDate]   DATE            NOT NULL,
    [FactoryID]          VARCHAR (8000)  NOT NULL,
    [BrandID]            VARCHAR (8000)  NULL,
    [StyleID]            VARCHAR (8000)  NULL,
    [POID]               VARCHAR (8000)  NULL,
    [SPNO]               VARCHAR (8000)  NOT NULL,
    [Article]            VARCHAR (8000)  NOT NULL,
    [SizeCode]           VARCHAR (8000)  NOT NULL,
    [Destination]        VARCHAR (8000)  NULL,
    [CDCode]             VARCHAR (8000)  NULL,
    [ProductionFamilyID] VARCHAR (8000)  NULL,
    [Team]               VARCHAR (8000)  NULL,
    [QCName]             VARCHAR (8000)  NOT NULL,
    [Shift]              VARCHAR (8000)  NOT NULL,
    [Line]               VARCHAR (8000)  NOT NULL,
    [Cell]               VARCHAR (8000)  NULL,
    [InspectQty]         INT             NULL,
    [RejectQty]          INT             NULL,
    [WFT]                DECIMAL (18, 3) NULL,
    [RFT]                DECIMAL (18, 3) NULL,
    [CDCodeNew]          VARCHAR (8000)  NULL,
    [ProductType]        NVARCHAR (1000) NULL,
    [FabricType]         NVARCHAR (1000) NULL,
    [Lining]             VARCHAR (8000)  NULL,
    [Gender]             VARCHAR (8000)  NULL,
    [Construction]       NVARCHAR (1000) NULL,
    [Ukey]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [InspectionDate]     DATE            NOT NULL,
    [DefectQty]          INT             CONSTRAINT [DF_P_DQSDefect_Summary_DefectQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_DQSDefect_Summary_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]       DATETIME        NULL,
    [BIStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_DQSDefect_Summary_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_DQSDefect_Summary] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [FirstInspectDate] ASC, [SPNO] ASC, [Article] ASC, [SizeCode] ASC, [QCName] ASC, [Shift] ASC, [Line] ASC, [InspectionDate] ASC)
);



GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際產出日 (Last Inspection Date)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'InspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Defect數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'DefectQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_DQSDefect_Summary', @level2type = N'COLUMN', @level2name = N'BIStatus';

