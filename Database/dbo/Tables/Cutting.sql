CREATE TABLE [dbo].[Cutting] (
    [ID]                       VARCHAR (13) CONSTRAINT [DF_Cutting_ID] DEFAULT ('') NOT NULL,
    [WorkType]                 VARCHAR (1)  CONSTRAINT [DF_Cutting_WorkType] DEFAULT ('') NULL,
    [FactoryID]                VARCHAR (8)  CONSTRAINT [DF_Cutting_FactoryID] DEFAULT ('') NULL,
    [SewInLine]                DATE         NULL,
    [SewOffLine]               DATE         NULL,
    [Remark]                   NCHAR (60)   CONSTRAINT [DF_Cutting_Remark] DEFAULT ('') NULL,
    [FirstCutDate]             DATE         NULL,
    [LastCutDate]              DATE         NULL,
    [Finished]                 BIT          CONSTRAINT [DF_Cutting_Finished] DEFAULT ((0)) NULL,
    [AddName]                  VARCHAR (10) CONSTRAINT [DF_Cutting_AddName] DEFAULT ('') NULL,
    [AddDate]                  DATETIME     NULL,
    [EditName]                 VARCHAR (10) CONSTRAINT [DF_Cutting_EditName] DEFAULT ('') NULL,
    [EditDate]                 DATETIME     NULL,
    [MDivisionid]              VARCHAR (8)  CONSTRAINT [DF_Cutting_MDivisionid] DEFAULT ('') NOT NULL,
    [CutForPlanningInLine]     DATE         DEFAULT (NULL) NULL,
    [CutForPlanningOffLine]    DATE         DEFAULT (NULL) NULL,
    [CutForOutputInline]       DATE         DEFAULT (NULL) NULL,
    [CutForOutputOffLine]      DATE         DEFAULT (NULL) NULL,
    [UseCutRefToRequestFabric] TINYINT      CONSTRAINT [DF_Cutting_UseCutRefToRequestFabric] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Cutting] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Master List', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Work Order 類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'WorkType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁縫上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'SewInLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁縫下線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'SewOffLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'第一次實際裁剪日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'FirstCutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後一次實際裁剪日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'LastCutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進入歷史單據', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'Finished';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪工單計畫的最早預計生產日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cutting', @level2type=N'COLUMN',@level2name=N'CutForPlanningInline'


GO
EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪工單計畫的最晚預計生產日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cutting', @level2type=N'COLUMN',@level2name=N'CutForPlanningOffLine'


GO
EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪工單產出的最早預計生產日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cutting', @level2type=N'COLUMN',@level2name=N'CutForOutputInline'


GO
EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪工單產出的最晚預計生產日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Cutting', @level2type=N'COLUMN',@level2name=N'CutForOutputOffLine'


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否使用裁剪裁次要料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutting', @level2type = N'COLUMN', @level2name = N'UseCutRefToRequestFabric';

