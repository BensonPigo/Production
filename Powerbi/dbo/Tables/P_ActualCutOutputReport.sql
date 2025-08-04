CREATE TABLE [dbo].[P_ActualCutOutputReport] (
    [FactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_FactoryID_New] DEFAULT ('') NOT NULL,
    [EstCutDate]         DATE            NULL,
    [ActCutDate]         DATE            NULL,
    [CutCellid]          VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_CutCellid_New] DEFAULT ('') NOT NULL,
    [SpreadingNoID]      VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_SpreadingNoID_New] DEFAULT ('') NOT NULL,
    [CutplanID]          VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_CutplanID_New] DEFAULT ('') NOT NULL,
    [CutRef]             VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_CutRef_New] DEFAULT ('') NOT NULL,
    [SP]                 VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_SP_New] DEFAULT ('') NOT NULL,
    [SubSP]              NVARCHAR (MAX)  CONSTRAINT [DF_P_ActualCutOutputReport_SubSP_New] DEFAULT ('') NOT NULL,
    [StyleID]            VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_StyleID_New] DEFAULT ('') NOT NULL,
    [Size]               NVARCHAR (1000) CONSTRAINT [DF_P_ActualCutOutputReport_Size_New] DEFAULT ('') NOT NULL,
    [noEXCESSqty]        NUMERIC (38)    CONSTRAINT [DF_P_ActualCutOutputReport_noEXCESSqty_New] DEFAULT ((0)) NOT NULL,
    [Description]        NVARCHAR (MAX)  CONSTRAINT [DF_P_ActualCutOutputReport_Description_New] DEFAULT ('') NOT NULL,
    [WeaveTypeID]        VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_WeaveTypeID_New] DEFAULT ('') NOT NULL,
    [FabricCombo]        VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_FabricCombo_New] DEFAULT ('') NOT NULL,
    [MarkerLength]       NUMERIC (38, 4) CONSTRAINT [DF_P_ActualCutOutputReport_MarkerLength_New] DEFAULT ((0)) NOT NULL,
    [PerimeterYd]        NVARCHAR (1000) CONSTRAINT [DF_P_ActualCutOutputReport_PerimeterYd_New] DEFAULT ('') NOT NULL,
    [Layer]              NUMERIC (38)    CONSTRAINT [DF_P_ActualCutOutputReport_Layer_New] DEFAULT ((0)) NOT NULL,
    [SizeCode]           NVARCHAR (MAX)  CONSTRAINT [DF_P_ActualCutOutputReport_SizeCode_New] DEFAULT ('') NOT NULL,
    [Cons]               NUMERIC (38, 4) CONSTRAINT [DF_P_ActualCutOutputReport_Cons_New] DEFAULT ((0)) NOT NULL,
    [EXCESSqty]          NUMERIC (38)    CONSTRAINT [DF_P_ActualCutOutputReport_EXCESSqty_New] DEFAULT ((0)) NOT NULL,
    [NoofRoll]           INT             CONSTRAINT [DF_P_ActualCutOutputReport_NoofRoll_New] DEFAULT ((0)) NOT NULL,
    [DyeLot]             INT             CONSTRAINT [DF_P_ActualCutOutputReport_DyeLot_New] DEFAULT ((0)) NOT NULL,
    [NoofWindow]         NUMERIC (38, 4) CONSTRAINT [DF_P_ActualCutOutputReport_NoofWindow_New] DEFAULT ((0)) NOT NULL,
    [CuttingSpeed]       NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_CuttingSpeed_New] DEFAULT ((0)) NOT NULL,
    [PreparationTime]    NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_PreparationTime_New] DEFAULT ((0)) NOT NULL,
    [ChangeoverTime]     NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_ChangeoverTime_New] DEFAULT ((0)) NOT NULL,
    [SpreadingSetupTime] NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_SpreadingSetupTime_New] DEFAULT ((0)) NOT NULL,
    [MachSpreadingTime]  NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_MachSpreadingTime_New] DEFAULT ((0)) NOT NULL,
    [SeparatorTime]      NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_SeparatorTime_New] DEFAULT ((0)) NOT NULL,
    [ForwardTime]        NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_ForwardTime_New] DEFAULT ((0)) NOT NULL,
    [CuttingSetupTime]   NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_CuttingSetupTime_New] DEFAULT ((0)) NOT NULL,
    [MachCuttingTime]    NUMERIC (38, 4) CONSTRAINT [DF_P_ActualCutOutputReport_MachCuttingTime_New] DEFAULT ((0)) NOT NULL,
    [WindowTime]         NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_WindowTime_New] DEFAULT ((0)) NOT NULL,
    [TotalSpreadingTime] NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_TotalSpreadingTime_New] DEFAULT ((0)) NOT NULL,
    [TotalCuttingTime]   NUMERIC (38, 3) CONSTRAINT [DF_P_ActualCutOutputReport_TotalCuttingTime_New] DEFAULT ((0)) NOT NULL,
    [UKey]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [BIFactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]       DATETIME        NULL,
    [BIStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_ActualCutOutputReport_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ActualCutOutputReport] PRIMARY KEY CLUSTERED ([UKey] ASC)
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


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ActualCutOutputReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ActualCutOutputReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ActualCutOutputReport', @level2type = N'COLUMN', @level2name = N'BIStatus';

