CREATE TABLE [dbo].[Export_Detail_Carton] (
    [Ukey]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Export_DetailUkey] BIGINT         NULL,
    [Id]                VARCHAR (13)   NOT NULL,
    [PoID]              VARCHAR (13)   NOT NULL,
    [Seq1]              VARCHAR (3)    NOT NULL,
    [Seq2]              VARCHAR (2)    NOT NULL,
    [Carton]            NVARCHAR (30)  NOT NULL,
    [LotNo]             NVARCHAR(50)   NULL,
    [Qty]               NUMERIC (8, 2) NULL,
    [Foc]               NUMERIC (8, 2) NULL,
    [NetKg]             NUMERIC (7, 2) NULL,
    [WeightKg]          NUMERIC (7, 2) NULL,
    [EditName]          VARCHAR (10)   NULL,
    [EditDate]          DATETIME       NULL,
    CONSTRAINT [PK_Export_Detail_Carton] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口FOC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Foc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'LotNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Carton';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'PoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Working No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Export_DetailUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Export_DetailUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船務出貨小項的箱號對應', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export_Detail_Carton';

