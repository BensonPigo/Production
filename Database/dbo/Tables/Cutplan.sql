CREATE TABLE [dbo].[Cutplan] (
    [ID]          VARCHAR (13) CONSTRAINT [DF_Cutplan_ID] DEFAULT ('') NOT NULL,
    [IssueDate]   DATE         NULL,
    [IssueID]     VARCHAR (13) CONSTRAINT [DF_Cutplan_IssueID] DEFAULT ('') NULL,
    [CuttingID]   VARCHAR (13) CONSTRAINT [DF_Cutplan_CuttingID] DEFAULT ('') NULL,
    [MDivisionid] VARCHAR (8)  CONSTRAINT [DF_Cutplan_Factoryid] DEFAULT ('') NOT NULL,
    [CutCellID]   VARCHAR (2)  CONSTRAINT [DF_Cutplan_CutCellID] DEFAULT ('') NOT NULL,
    [EstCutdate]  DATE         NULL,
    [MarkerReqid] VARCHAR (13) CONSTRAINT [DF_Cutplan_MarkerReqid] DEFAULT ('') NULL,
    [Status]      VARCHAR (15) CONSTRAINT [DF_Cutplan_Status] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10) CONSTRAINT [DF_Cutplan_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) CONSTRAINT [DF_Cutplan_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME     NULL,
    [POID]        VARCHAR (13) CONSTRAINT [DF_Cutplan_POID] DEFAULT ('') NULL,
    CONSTRAINT [PK_Cutplan] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Daily Plan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'IssueID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'CuttingID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪Cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'CutCellID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'EstCutdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'MarkerReqid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan', @level2type = N'COLUMN', @level2name = N'MDivisionid';

