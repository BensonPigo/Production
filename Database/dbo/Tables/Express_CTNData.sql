CREATE TABLE [dbo].[Express_CTNData] (
    [ID]        VARCHAR (13)   CONSTRAINT [DF_Express_CTNData_ID] DEFAULT ('') NOT NULL,
    [CTNNo]     VARCHAR (10)   CONSTRAINT [DF_Express_CTNData_CTNNo] DEFAULT ('') NOT NULL,
    [CtnLength] NUMERIC (5, 2) CONSTRAINT [DF_Express_CTNData_CtnLength] DEFAULT ((0)) NULL,
    [CtnWidth]  NUMERIC (5, 2) CONSTRAINT [DF_Express_CTNData_CtnWidth] DEFAULT ((0)) NULL,
    [CtnHeight] NUMERIC (5, 2) CONSTRAINT [DF_Express_CTNData_CtnHeight] DEFAULT ((0)) NULL,
    [CTNNW]     NUMERIC (8, 2) CONSTRAINT [DF_Express_CTNData_CTNNW] DEFAULT ((0)) NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Express_CTNData_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Express_CTNData_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_Express_CTNData] PRIMARY KEY CLUSTERED ([ID] ASC, [CTNNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'International Express - Carton Dimension & Weight', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HC No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'CTNNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱規格-長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'CtnLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱規格-寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'CtnWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱規格-高', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'CtnHeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'空箱子重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'CTNNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_CTNData', @level2type = N'COLUMN', @level2name = N'EditDate';

