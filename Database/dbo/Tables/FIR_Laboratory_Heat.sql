CREATE TABLE [dbo].[FIR_Laboratory_Heat] (
    [ID]                 BIGINT         CONSTRAINT [DF_FIR_Laboratory_Heat_ID] DEFAULT ((0)) NOT NULL,
    [Roll]               VARCHAR (8)    CONSTRAINT [DF_FIR_Laboratory_Heat_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]             VARCHAR (8)    CONSTRAINT [DF_FIR_Laboratory_Heat_Dyelot] DEFAULT ('') NOT NULL,
    [Inspdate]           DATE           NULL,
    [Inspector]          VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Heat_Inspector] DEFAULT ('') NULL,
    [Result]             VARCHAR (5)    CONSTRAINT [DF_FIR_Laboratory_Heat_Result] DEFAULT ('') NULL,
    [Remark]             NVARCHAR (100) CONSTRAINT [DF_FIR_Laboratory_Heat_Remark] DEFAULT ('') NULL,
    [AddName]            VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Heat_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME       NULL,
    [EditName]           VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Heat_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME       NULL,
    [HorizontalRate]     NUMERIC (5, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_HorizontalRate] DEFAULT ((0)) NULL,
    [HorizontalOriginal] NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_HorizontalOriginal] DEFAULT ((0)) NULL,
    [HorizontalTest1]    NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_HorizontalTest1] DEFAULT ((0)) NULL,
    [HorizontalTest2]    NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_HorizontalTest2] DEFAULT ((0)) NULL,
    [HorizontalTest3]    NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_HorizontalTest3] DEFAULT ((0)) NULL,
    [VerticalRate]       NUMERIC (5, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_VerticalRate] DEFAULT ((0)) NULL,
    [VerticalOriginal]   NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_VerticalOriginal] DEFAULT ((0)) NULL,
    [VerticalTest1]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_VerticalTest1] DEFAULT ((0)) NULL,
    [VerticalTest2]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_VerticalTest2] DEFAULT ((0)) NULL,
    [VerticalTest3]      NUMERIC (4, 2) CONSTRAINT [DF_FIR_Laboratory_Heat_VerticalTest3] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_FIR_Laboratory_Heat] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Laboratory - Heat Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'Inspdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平縮律', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'HorizontalRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平原長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'HorizontalOriginal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'HorizontalTest1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'HorizontalTest2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水平3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'HorizontalTest3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直縮律', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'VerticalRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直原長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'VerticalOriginal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'VerticalTest1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'VerticalTest2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'垂直3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Heat', @level2type = N'COLUMN', @level2name = N'VerticalTest3';

