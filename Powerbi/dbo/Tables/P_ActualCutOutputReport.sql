CREATE TABLE [dbo].[P_ActualCutOutputReport] (
    [FactoryID]                 VARCHAR (8)     CONSTRAINT [DF_P_ActualCutOutputReport_FactoryID] DEFAULT ('') NOT NULL,
    [EstCutDate]                DATE            NULL,
    [ActCutDate]                DATE            NULL,
    [CutCellid]                 VARCHAR (2)     CONSTRAINT [DF_P_ActualCutOutputReport_CutCellid] DEFAULT ('') NOT NULL,
    [SpreadingNoID]             VARCHAR (5)     CONSTRAINT [DF_P_ActualCutOutputReport_SpreadingNoID] DEFAULT ('') NOT NULL,
    [CutplanID]                 VARCHAR (13)    CONSTRAINT [DF_P_ActualCutOutputReport_CutplanID] DEFAULT ('') NOT NULL,
    [CutRef]                    VARCHAR (10)     CONSTRAINT [DF_P_ActualCutOutputReport_CutRef] DEFAULT ('') NOT NULL,
    [SP]                        VARCHAR (13)    CONSTRAINT [DF_P_ActualCutOutputReport_SP] DEFAULT ('') NOT NULL,
    [SubSP]                     NVARCHAR (MAX)  CONSTRAINT [DF_P_ActualCutOutputReport_SubSP] DEFAULT ('') NOT NULL,
    [StyleID]                   VARCHAR (15)    CONSTRAINT [DF_P_ActualCutOutputReport_StyleID] DEFAULT ('') NOT NULL,
    [Size]                      NVARCHAR (100)   CONSTRAINT [DF_P_ActualCutOutputReport_Size] DEFAULT ('') NOT NULL,
    [noEXCESSqty]               NUMERIC (10)    CONSTRAINT [DF_P_ActualCutOutputReport_noEXCESSqty] DEFAULT ((0)) NOT NULL,
    [Description]               NVARCHAR (MAX)  CONSTRAINT [DF_P_ActualCutOutputReport_Description] DEFAULT ('') NOT NULL,
    [WeaveTypeID]               VARCHAR (20)    CONSTRAINT [DF_P_ActualCutOutputReport_WeaveTypeID] DEFAULT ('') NOT NULL,
    [FabricCombo]               VARCHAR (2)     CONSTRAINT [DF_P_ActualCutOutputReport_FabricCombo] DEFAULT ('') NOT NULL,
    [MarkerLength]              NUMERIC (10, 4) CONSTRAINT [DF_P_ActualCutOutputReport_MarkerLength] DEFAULT ((0)) NOT NULL,
    [PerimeterYd]               NVARCHAR (10)   CONSTRAINT [DF_P_ActualCutOutputReport_PerimeterYd] DEFAULT ('') NOT NULL,
    [Layer]                     NUMERIC (5)     CONSTRAINT [DF_P_ActualCutOutputReport_Layer] DEFAULT ((0)) NOT NULL,
    [SizeCode]                  NVARCHAR (MAX)  CONSTRAINT [DF_P_ActualCutOutputReport_SizeCode] DEFAULT ('') NOT NULL,
    [Cons]                      NUMERIC (12, 4) CONSTRAINT [DF_P_ActualCutOutputReport_Cons] DEFAULT ((0)) NOT NULL,
    [EXCESSqty]                 NUMERIC (10)    CONSTRAINT [DF_P_ActualCutOutputReport_EXCESSqty] DEFAULT ((0)) NOT NULL,
    [NoofRoll]                  INT             CONSTRAINT [DF_P_ActualCutOutputReport_NoofRoll] DEFAULT ((0)) NOT NULL,
    [DyeLot]                    INT             CONSTRAINT [DF_P_ActualCutOutputReport_DyeLot] DEFAULT ((0)) NOT NULL,
    [NoofWindow]                NUMERIC (9, 4)  CONSTRAINT [DF_P_ActualCutOutputReport_NoofWindow] DEFAULT ((0)) NOT NULL,
    [CuttingSpeed]              NUMERIC (5, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_CuttingSpeed] DEFAULT ((0)) NOT NULL,
    [PreparationTime]           NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_PreparationTime] DEFAULT ((0)) NOT NULL,
    [ChangeoverTime]            NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_ChangeoverTime] DEFAULT ((0)) NOT NULL,
    [SpreadingSetupTime]        NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_SpreadingSetupTime] DEFAULT ((0)) NOT NULL,
    [MachSpreadingTime]         NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_MachSpreadingTime] DEFAULT ((0)) NOT NULL,
    [SeparatorTime]             NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_SeparatorTime] DEFAULT ((0)) NOT NULL,
    [ForwardTime]               NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_ForwardTime] DEFAULT ((0)) NOT NULL,
    [CuttingSetupTime]          NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_CuttingSetupTime] DEFAULT ((0)) NOT NULL,
    [MachCuttingTime]           NUMERIC (20, 4) CONSTRAINT [DF_P_ActualCutOutputReport_MachCuttingTime] DEFAULT ((0)) NOT NULL,
    [WindowTime]                NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_WindowTime] DEFAULT ((0)) NOT NULL,
    [TotalSpreadingTime]        NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_TotalSpreadingTime] DEFAULT ((0)) NOT NULL,
    [TotalCuttingTime]          NUMERIC (8, 3)  CONSTRAINT [DF_P_ActualCutOutputReport_TotalCuttingTime] DEFAULT ((0)) NOT NULL,
    [ActualCutOutputReportUkey] BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_P_ActualCutOutputReport] PRIMARY KEY CLUSTERED ([ActualCutOutputReportUkey] ASC)
);









