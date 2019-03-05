CREATE TABLE [dbo].[WorkOrder_Estcutdate] (
    [WorkOrderUkey]    BIGINT       CONSTRAINT [DF_WorkOrder_Estcutdate_WorkOrderUkey] DEFAULT ((0)) NOT NULL,
    [OrgEstCutDate]    DATE         NULL,
    [NewEstCutDate]    DATE         NULL,
    [CutReasonid]      VARCHAR (5)  CONSTRAINT [DF_WorkOrder_Estcutdate_Reason] DEFAULT ('') NULL,
    [Ukey]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [ID]               VARCHAR (13) CONSTRAINT [DF_WorkOrder_Estcutdate_ID] DEFAULT ('') NULL,
    [OrgCutCellid]     VARCHAR (2)  NULL,
    [NewCutCellid]     VARCHAR (2)  NULL,
    [OrgSpreadingNoID] VARCHAR (3)  NULL,
    [NewSpreadingNoID] VARCHAR (3)  NULL,
    CONSTRAINT [PK_WorkOrder_Estcutdate] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Change Est Cut Date after Cutting daily Plan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Estcutdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrderUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Estcutdate', @level2type = N'COLUMN', @level2name = N'WorkOrderUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'舊預計裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Estcutdate', @level2type = N'COLUMN', @level2name = N'OrgEstCutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新預計裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Estcutdate', @level2type = N'COLUMN', @level2name = N'NewEstCutDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Estcutdate', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'理由', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder_Estcutdate', @level2type = N'COLUMN', @level2name = N'CutReasonid';

