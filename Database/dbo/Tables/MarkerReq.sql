CREATE TABLE [dbo].[MarkerReq] (
    [ID]          VARCHAR (13) CONSTRAINT [DF_MarkerReq_ID] DEFAULT ('') NOT NULL,
    [EstCutdate]  DATE         NULL,
    [MDivisionid] VARCHAR (8)  CONSTRAINT [DF_MarkerReq_MDivisionid] DEFAULT ('') NOT NULL,
    [CutCellID]   VARCHAR (2)  CONSTRAINT [DF_MarkerReq_CutCellID] DEFAULT ('') NOT NULL,
    [Status]      VARCHAR (15) CONSTRAINT [DF_MarkerReq_Status] DEFAULT ('') NULL,
    [Cutplanid]   VARCHAR (13) CONSTRAINT [DF_MarkerReq_Cutplanid] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10) CONSTRAINT [DF_MarkerReq_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) CONSTRAINT [DF_MarkerReq_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME     NULL,
    [SendDate]    DATETIME     NULL,
    [FactoryID]   VARCHAR (8)  CONSTRAINT [DF_MarkerReq_FactoryID] DEFAULT　('') NOT NULL
    CONSTRAINT [PK_MarkerReq] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bulk Marker Request', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'EstCutdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = 'MDivisionid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'CutCellID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪計畫單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'Cutplanid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠代' , @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq', @level2type = N'COLUMN', @level2name = N'FactoryID'

