CREATE TABLE [dbo].[LearnCurve] (
    [ID]          INT            CONSTRAINT [DF_LearnCurve_ID] DEFAULT ((0)) NOT NULL,
    [Name]        VARCHAR (50)   CONSTRAINT [DF_LearnCurve_Name] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (500) CONSTRAINT [DF_LearnCurve_Description] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_LearnCurve_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_LearnCurve_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_LearnCurve] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'學習曲線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'學習曲線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'學習曲線名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'學習曲線說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LearnCurve', @level2type = N'COLUMN', @level2name = N'EditDate';

