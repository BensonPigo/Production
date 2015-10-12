﻿CREATE TABLE [dbo].[LineMapping_Detail] (
    [ID]            BIGINT         CONSTRAINT [DF_LineMapping_Detail_ID] DEFAULT ((0)) NOT NULL,
    [No]            VARCHAR (4)    CONSTRAINT [DF_LineMapping_Detail_No] DEFAULT ('') NOT NULL,
    [Description]   NVARCHAR (200) CONSTRAINT [DF_LineMapping_Detail_Description] DEFAULT ('') NULL,
    [Annotation]    NVARCHAR (200) CONSTRAINT [DF_LineMapping_Detail_Annotation] DEFAULT ('') NULL,
    [GSD]           NUMERIC (6, 2) CONSTRAINT [DF_LineMapping_Detail_GSD] DEFAULT ((0)) NULL,
    [TotalGSD]      NUMERIC (7, 2) CONSTRAINT [DF_LineMapping_Detail_TotalGSD] DEFAULT ((0)) NULL,
    [Cycle]         NUMERIC (6, 2) CONSTRAINT [DF_LineMapping_Detail_Cycle] DEFAULT ((0)) NULL,
    [TotalCycle]    NUMERIC (7, 2) CONSTRAINT [DF_LineMapping_Detail_TotalCycle] DEFAULT ((0)) NULL,
    [MachineTypeID] VARCHAR (10)   CONSTRAINT [DF_LineMapping_Detail_MachineTypeID] DEFAULT ('') NULL,
    [OperationID]   VARCHAR (20)   CONSTRAINT [DF_LineMapping_Detail_OperationID] DEFAULT ('') NULL,
    [MoldID]        NVARCHAR (200) CONSTRAINT [DF_LineMapping_Detail_MoldID] DEFAULT ('') NULL,
    [GroupKey]      INT            CONSTRAINT [DF_LineMapping_Detail_GroupKey] DEFAULT ((0)) NOT NULL,
    [New]           BIT            CONSTRAINT [DF_LineMapping_Detail_New] DEFAULT ((0)) NULL,
    [EmployeeID]    VARCHAR (10)   CONSTRAINT [DF_LineMapping_Detail_EmployeeID] DEFAULT ('') NULL,
    CONSTRAINT [PK_LineMapping_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [GroupKey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Line Mapping Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'No';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Annotation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'Annotation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSD秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'GSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSD總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'TotalGSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cycle秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'Cycle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cycle總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'TotalCycle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'MachineTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'OperationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模具', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'MoldID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'GroupKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手動新增', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'New';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'員工編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'EmployeeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'ID';

