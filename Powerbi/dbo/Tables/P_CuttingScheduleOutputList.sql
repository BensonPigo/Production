CREATE TABLE [dbo].[P_CuttingScheduleOutputList] (
    [MDivisionID]           VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]             VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_FactoryID_New] DEFAULT ('') NOT NULL,
    [Fabrication]           VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_Fabrication_New] DEFAULT ('') NOT NULL,
    [EstCuttingDate]        DATE            NULL,
    [ActCuttingDate]        DATE            NULL,
    [EarliestSewingInline]  DATE            NULL,
    [POID]                  VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_POID_New] DEFAULT ('') NOT NULL,
    [BrandID]               VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]               VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_StyleID_New] DEFAULT ('') NOT NULL,
    [FabRef]                VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_FabRef_New] DEFAULT ('') NOT NULL,
    [SwitchToWorkorderType] VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_SwitchToWorkorderType_New] DEFAULT ('') NOT NULL,
    [CutRef]                VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_CutRef_New] DEFAULT ('') NOT NULL,
    [CutNo]                 NUMERIC (38)    CONSTRAINT [DF_P_CuttingScheduleOutputList_CutNo_New] DEFAULT ((0)) NOT NULL,
    [SpreadingNoID]         VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_SpreadingNoID_New] DEFAULT ('') NOT NULL,
    [CutCell]               VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_CutCell_New] DEFAULT ('') NOT NULL,
    [Combination]           VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_Combination_New] DEFAULT ('') NOT NULL,
    [Layers]                NUMERIC (38)    CONSTRAINT [DF_P_CuttingScheduleOutputList_Layers_New] DEFAULT ((0)) NOT NULL,
    [LayersLevel]           VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_LayersLevel_New] DEFAULT ('') NOT NULL,
    [LackingLayers]         NUMERIC (38)    CONSTRAINT [DF_P_CuttingScheduleOutputList_LackingLayers_New] DEFAULT ((0)) NOT NULL,
    [Ratio]                 VARCHAR (8000)  NOT NULL,
    [Consumption]           NUMERIC (38, 4) CONSTRAINT [DF_P_CuttingScheduleOutputList_Consumption_New] DEFAULT ((0)) NOT NULL,
    [ActConsOutput]         NUMERIC (38, 4) CONSTRAINT [DF_P_CuttingScheduleOutputList_ActConsOutput_New] DEFAULT ((0)) NOT NULL,
    [BalanceCons]           NUMERIC (38, 4) CONSTRAINT [DF_P_CuttingScheduleOutputList_BalanceCons_New] DEFAULT ((0)) NOT NULL,
    [MarkerName]            VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_MarkerName_New] DEFAULT ('') NOT NULL,
    [MarkerNo]              VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_MarkerNo_New] DEFAULT ('') NOT NULL,
    [MarkerLength]          VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_MarkerLength_New] DEFAULT ('') NOT NULL,
    [CuttingPerimeter]      NVARCHAR (1000) CONSTRAINT [DF_P_CuttingScheduleOutputList_CuttingPerimeter_New] DEFAULT ('') NOT NULL,
    [StraightLength]        VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_StraightLength_New] DEFAULT ('') NOT NULL,
    [CurvedLength]          VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_CurvedLength_New] DEFAULT ('') NOT NULL,
    [DelayReason]           NVARCHAR (1000) CONSTRAINT [DF_P_CuttingScheduleOutputList_DelayReason_New] DEFAULT ('') NOT NULL,
    [Remark]                NVARCHAR (MAX)  CONSTRAINT [DF_P_CuttingScheduleOutputList_Remark_New] DEFAULT ('') NOT NULL,
    [Ukey]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [BIFactoryID]           VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]          DATETIME        NULL,
    [BIStatus]              VARCHAR (8000)  CONSTRAINT [DF_P_CuttingScheduleOutputList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_CuttingScheduleOutputList] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingScheduleOutputList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CuttingScheduleOutputList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_CuttingScheduleOutputList', @level2type = N'COLUMN', @level2name = N'BIStatus';
GO

