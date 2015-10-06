CREATE TABLE [dbo].[Cutplan_Detail_Cons] (
    [ID]       VARCHAR (13)    CONSTRAINT [DF_Cutplan_Detail_Cons_ID] DEFAULT ('') NOT NULL,
    [Poid]     VARCHAR (13)    CONSTRAINT [DF_Cutplan_Detail_Cons_Poid] DEFAULT ('') NOT NULL,
    [SciRefno] VARCHAR (20)    CONSTRAINT [DF_Cutplan_Detail_Cons_SciRefno] DEFAULT ('') NOT NULL,
    [ColorId]  VARCHAR (6)     CONSTRAINT [DF_Cutplan_Detail_Cons_ColorId] DEFAULT ('') NOT NULL,
    [Cons]     NUMERIC (8, 2)  CONSTRAINT [DF_Cutplan_Detail_Cons_Cons] DEFAULT ((0)) NOT NULL,
    [OrderID]  VARCHAR (13)    CONSTRAINT [DF_Cutplan_Detail_Cons_OrderID] DEFAULT ('') NOT NULL,
    [SEQ]      VARCHAR (5)     CONSTRAINT [DF_Cutplan_Detail_Cons_SEQ] DEFAULT ('') NOT NULL,
    [CutQty]   NUMERIC (10, 2) CONSTRAINT [DF_Cutplan_Detail_Cons_CutQty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Cutplan_Detail_Cons] PRIMARY KEY CLUSTERED ([ID] ASC, [OrderID] ASC, [SEQ] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Daily Plan (Cons. 合併)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Poid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'SciRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'ColorId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cons', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'CutQty';

