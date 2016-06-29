CREATE TABLE [dbo].[MD] (
    [ID]       VARCHAR (13) CONSTRAINT [DF_MD_ID] DEFAULT ('') NOT NULL,
    [Type]     VARCHAR (20) CONSTRAINT [DF_MD_Type] DEFAULT ('') NOT NULL,
    [Item]     VARCHAR (20) CONSTRAINT [DF_MD_Item] DEFAULT ('') NOT NULL,
    [Colorid]  VARCHAR (70) CONSTRAINT [DF_MD_Colorid] DEFAULT ('') NOT NULL,
    [InspDate] DATE         NULL,
    [Result]   VARCHAR (60) CONSTRAINT [DF_MD_Result] DEFAULT ('') NULL,
    CONSTRAINT [PK_MD] PRIMARY KEY CLUSTERED ([ID] ASC, [Type] ASC, [Item] ASC, [Colorid] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MD Master List', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MD', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MD', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MD', @level2type = N'COLUMN', @level2name = N'Item';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MD', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MD', @level2type = N'COLUMN', @level2name = N'InspDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MD', @level2type = N'COLUMN', @level2name = N'Result';


GO

GO
