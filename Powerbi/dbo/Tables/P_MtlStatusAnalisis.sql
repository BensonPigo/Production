CREATE TABLE [dbo].[P_MtlStatusAnalisis] (
    [WK]                VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_WK_New] DEFAULT ('') NOT NULL,
    [LoadingCountry]    VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_LoadingCountry_New] DEFAULT ('') NOT NULL,
    [LoadingPort]       VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_LoadingPort_New] DEFAULT ('') NOT NULL,
    [Shipmode]          VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_Shipmode_New] DEFAULT ('') NOT NULL,
    [Close_Date]        DATE            NULL,
    [ETD]               DATE            NULL,
    [ETA]               DATE            NULL,
    [Arrive_WH_Date]    DATE            NULL,
    [KPI_LETA]          DATE            NULL,
    [Prod_LT]           INT             CONSTRAINT [DF_P_MtlStatusAnalisis_Prod_LT_New] DEFAULT ((0)) NOT NULL,
    [WK_Factory]        VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_WK_Factory_New] DEFAULT ('') NOT NULL,
    [FactoryID]         VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_FactoryID_New] DEFAULT ('') NOT NULL,
    [SPNo]              VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_SPNo_New] DEFAULT ('') NOT NULL,
    [SEQ]               VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_SEQ_New] DEFAULT ('') NOT NULL,
    [Category]          NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_Category_New] DEFAULT ('') NOT NULL,
    [PF_ETA]            DATE            NULL,
    [SewinLine]         DATE            NULL,
    [BuyerDelivery]     DATE            NULL,
    [SCIDelivery]       DATE            NULL,
    [PO_SMR]            NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_PO_SMR_New] DEFAULT ('') NOT NULL,
    [PO_Handle]         NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_PO_Handle_New] DEFAULT ('') NOT NULL,
    [SMR]               NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_SMR_New] DEFAULT ('') NOT NULL,
    [MR]                NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_MR_New] DEFAULT ('') NOT NULL,
    [PC_Handle]         NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_PC_Handle_New] DEFAULT ('') NOT NULL,
    [Style]             VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_Style_New] DEFAULT ('') NOT NULL,
    [SP_List]           VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_SP_List_New] DEFAULT ('') NOT NULL,
    [PO_Qty]            NUMERIC (38)    CONSTRAINT [DF_P_MtlStatusAnalisis_PO_Qty_New] DEFAULT ((0)) NULL,
    [Project]           VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_Project_New] DEFAULT ('') NOT NULL,
    [Early_Ship_Reason] VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_Early_Ship_Reason_New] DEFAULT ('') NOT NULL,
    [WK_Handle]         NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_WK_Handle_New] DEFAULT ('') NOT NULL,
    [MTL_Confirm]       VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_MTL_Confirm_New] DEFAULT ('') NOT NULL,
    [Duty]              NVARCHAR (1000) CONSTRAINT [DF_P_MtlStatusAnalisis_Duty_New] DEFAULT ('') NOT NULL,
    [PF_Remark]         VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_PF_Remark_New] DEFAULT ('') NOT NULL,
    [Type]              VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_Type_New] DEFAULT ('') NOT NULL,
    [Ukey]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [BIFactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]      DATETIME        NULL,
    [BIStatus]          VARCHAR (8000)  CONSTRAINT [DF_P_MtlStatusAnalisis_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_MtlStatusAnalisis] PRIMARY KEY CLUSTERED ([Ukey] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'WK'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LoadingCountry' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'LoadingCountry'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LoadingPort' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'LoadingPort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Shipmode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Shipmode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Close_Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Close_Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ETD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'ETD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Arrive_WH_Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Arrive_WH_Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'KPI_LETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'KPI_LETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prod_LT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Prod_LT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK_Factory' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'WK_Factory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SPNo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SEQ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Category' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PF_ETA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PF_ETA'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewinLine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SewinLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BuyerDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCIDelivery' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO_SMR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PO_SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO_Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PO_Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SMR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MR' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'MR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PC_Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PC_Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SP_List' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'SP_List'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PO_Qty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PO_Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Project' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Project'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Early_Ship_Reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Early_Ship_Reason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WK_Handle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'WK_Handle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MTL_Confirm' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'MTL_Confirm'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Duty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Duty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PF_Remark' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'PF_Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_MtlStatusAnalisis', @level2type = N'COLUMN', @level2name = N'BIStatus';

