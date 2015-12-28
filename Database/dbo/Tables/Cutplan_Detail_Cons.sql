CREATE TABLE [dbo].[Cutplan_Detail_Cons] (
    [ID]   VARCHAR (13)   CONSTRAINT [DF_Cutplan_Detail_Cons_ID] DEFAULT ('') NOT NULL,
    [Poid] VARCHAR (13)   CONSTRAINT [DF_Cutplan_Detail_Cons_Poid] DEFAULT ('') NOT NULL,
    [Cons] NUMERIC (8, 2) CONSTRAINT [DF_Cutplan_Detail_Cons_Cons] DEFAULT ((0)) NOT NULL,
    [SEQ1] VARCHAR (3)    CONSTRAINT [DF_Cutplan_Detail_Cons_SEQ] DEFAULT ('') NOT NULL,
    [SEQ2] VARCHAR (2)    NOT NULL,
    CONSTRAINT [PK_Cutplan_Detail_Cons_1] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC, [SEQ2] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Daily Plan (Cons. 合併)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Poid';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cons', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Cons';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cutplan_Detail_Cons', @level2type = N'COLUMN', @level2name = N'SEQ1';

