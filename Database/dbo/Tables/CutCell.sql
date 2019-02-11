CREATE TABLE [dbo].[CutCell] (
    [ID]               VARCHAR (2)   CONSTRAINT [DF_CutCell_ID] DEFAULT ('') NOT NULL,
    [MDivisionid]      VARCHAR (8)   CONSTRAINT [DF_CutCell_FactoryID] DEFAULT ('') NOT NULL,
    [Description]      NVARCHAR (60) CONSTRAINT [DF_CutCell_Description] DEFAULT ('') NULL,
    [Junk]             BIT           CONSTRAINT [DF_CutCell_Junk] DEFAULT ((0)) NULL,
    [AddName]          VARCHAR (10)  CONSTRAINT [DF_CutCell_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME      NULL,
    [EditName]         VARCHAR (10)  CONSTRAINT [DF_CutCell_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME      NULL,
    [CuttingWidth]     INT           CONSTRAINT [DF_CutCell_CuttingWidth] DEFAULT ((0)) NULL,
    [CuttingMachineID] VARCHAR (10)  NULL,
    CONSTRAINT [PK_CutCell] PRIMARY KEY CLUSTERED ([ID] ASC, [MDivisionid] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutCell', @level2type = N'COLUMN', @level2name = N'MDivisionid';

