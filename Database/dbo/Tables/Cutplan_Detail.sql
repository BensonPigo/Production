CREATE TABLE [dbo].[Cutplan_Detail] (
    [ID]            VARCHAR (13)   CONSTRAINT [DF_Cutplan_Detail_ID] DEFAULT ('') NOT NULL,
    [Sewinglineid]  VARCHAR (5)    CONSTRAINT [DF_Cutplan_Detail_SewingInline] DEFAULT ('') NOT NULL,
    [CutRef]        VARCHAR (6)    CONSTRAINT [DF_Cutplan_Detail_CutRef] DEFAULT ('') NOT NULL,
    [CutNo]         NUMERIC (6)    CONSTRAINT [DF_Cutplan_Detail_CutNo] DEFAULT ((0)) NULL,
    [OrderID]       VARCHAR (13)   CONSTRAINT [DF_Cutplan_Detail_OrderID] DEFAULT ('') NOT NULL,
    [StyleID]       VARCHAR (15)   CONSTRAINT [DF_Cutplan_Detail_StyleID] DEFAULT ('') NOT NULL,
    [Colorid]       VARCHAR (6)    CONSTRAINT [DF_Cutplan_Detail_Colorid] DEFAULT ('') NOT NULL,
    [Cons]          NUMERIC (8, 2) CONSTRAINT [DF_Cutplan_Detail_Cons] DEFAULT ((0)) NOT NULL,
    [WorkorderUkey] BIGINT         CONSTRAINT [DF_Cutplan_Detail_WorkorderUkey] DEFAULT ((0)) NOT NULL,
    [Remark]        NVARCHAR (MAX) CONSTRAINT [DF_Cutplan_Detail_Remark] DEFAULT ('') NULL,
    [POID]          VARCHAR (13)   CONSTRAINT [DF_Cutplan_Detail_POID] DEFAULT ('') NULL,
    CONSTRAINT [PK_Cutplan_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [WorkorderUkey] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Daily Plan detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cut Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'CutNo';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款示', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'WorkorderUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫產線號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail', @level2type = N'COLUMN', @level2name = N'Sewinglineid';

