CREATE TABLE [dbo].[Brand_Month] (
    [ID]         VARCHAR (8)  CONSTRAINT [DF_Brand_Month_ID] DEFAULT ('') NOT NULL,
    [Year]       VARCHAR (4)  CONSTRAINT [DF_Brand_Month_Year] DEFAULT ('') NOT NULL,
    [Month]      VARCHAR (2)  CONSTRAINT [DF_Brand_Month_Month] DEFAULT ('') NOT NULL,
    [MonthLabel] VARCHAR (10) CONSTRAINT [DF_Brand_Month_MonthLabel] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_Brand_Month_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_Brand_Month_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME     NULL,
    CONSTRAINT [PK_Brand_Month] PRIMARY KEY CLUSTERED ([ID] ASC, [Year] ASC, [Month] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Month在Label上的顯示名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'MonthLabel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'Month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶基本資料-Buy Month', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand_Month';

