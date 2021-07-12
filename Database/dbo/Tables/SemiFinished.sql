CREATE TABLE [dbo].[SemiFinished] (
    [Desc]     NVARCHAR (MAX) CONSTRAINT [DF_SemiFinished_Desc] DEFAULT ('') NOT NULL,
    [Unit]     VARCHAR (8)    CONSTRAINT [DF_SemiFinished_Unit] DEFAULT ('') NOT NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_SemiFinished_AddName] DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_SemiFinished_EditName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME       NULL,
    [Color]    VARCHAR (10)   DEFAULT ('') NOT NULL,
    [POID]     VARCHAR (13)   DEFAULT ('') NOT NULL,
    [Seq]      VARCHAR (6)    DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SemiFinished] PRIMARY KEY CLUSTERED ([POID] ASC, [Seq] ASC)
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品項次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'Desc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinished', @level2type = N'COLUMN', @level2name = N'Color';

