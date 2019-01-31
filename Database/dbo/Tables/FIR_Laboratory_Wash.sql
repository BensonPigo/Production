CREATE TABLE [dbo].[FIR_Laboratory_Wash] (
    [ID]                 BIGINT         CONSTRAINT [DF_FIR_Laboratory_Wash_ID] DEFAULT ((0)) NOT NULL,
    [Roll]               VARCHAR (8)    CONSTRAINT [DF_FIR_Laboratory_Wash_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]             VARCHAR (8)    CONSTRAINT [DF_FIR_Laboratory_Wash_Dyelot] DEFAULT ('') NOT NULL,
    [Inspdate]           DATE           NULL,
    [Inspector]          VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Wash_Inspector] DEFAULT ('') NULL,
    [Result]             VARCHAR (5)    CONSTRAINT [DF_FIR_Laboratory_Wash_Result] DEFAULT ('') NULL,
    [Remark]             NVARCHAR (100) CONSTRAINT [DF_FIR_Laboratory_Wash_Remark] DEFAULT ('') NULL,
    [AddName]            VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Wash_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME       NULL,
    [EditName]           VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Wash_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME       NULL,
    [HorizontalRate]     NUMERIC (6, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_HorizontalRate] DEFAULT ((0)) NULL,
    [HorizontalOriginal] NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_HorizontalOriginal] DEFAULT ((0)) NULL,
    [HorizontalTest1]    NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_HorizontalTest1] DEFAULT ((0)) NULL,
    [HorizontalTest2]    NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_HorizontalTest2] DEFAULT ((0)) NULL,
    [HorizontalTest3]    NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_HorizontalTest3] DEFAULT ((0)) NULL,
    [VerticalRate]       NUMERIC (6, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_VerticalRate] DEFAULT ((0)) NULL,
    [VerticalOriginal]   NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_VerticalOriginal] DEFAULT ((0)) NULL,
    [VerticalTest1]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_VerticalTest1] DEFAULT ((0)) NULL,
    [VerticalTest2]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_VerticalTest2] DEFAULT ((0)) NULL,
    [VerticalTest3]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_VerticalTest3] DEFAULT ((0)) NULL,
    [SkewnessTest1]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_SkewnessTest1] DEFAULT ((0)) NULL,
    [SkewnessTest2]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_SkewnessTest2] DEFAULT ((0)) NULL,
    [SkewnessTest3]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_SkewnessTest3] DEFAULT ((0)) NULL,
    [SkewnessTest4]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_SkewnessTest4] DEFAULT ((0)) NULL,
    [SkewnessRate]       NUMERIC (6, 2) CONSTRAINT [DF_FIR_Laboratory_Wash_SkewnessRate] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_FIR_Laboratory_Wash] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Laboratory - WashTest', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'Inspdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平縮律', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'HorizontalRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平原長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'HorizontalOriginal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'HorizontalTest1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'HorizontalTest2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'HorizontalTest3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直縮律', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'VerticalRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直原長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'VerticalOriginal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'VerticalTest1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'VerticalTest2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'VerticalTest3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'偏斜度測試4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'SkewnessTest4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'偏斜度測試3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Wash', @level2type = N'COLUMN', @level2name = N'SkewnessTest3';

