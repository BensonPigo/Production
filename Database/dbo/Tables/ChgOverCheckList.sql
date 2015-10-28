CREATE TABLE [dbo].[ChgOverCheckList] (
    [ID]          VARCHAR (4)   CONSTRAINT [DF_ChgOverCheckList_ID] DEFAULT ('') NOT NULL,
    [BrandID]     VARCHAR (8)   CONSTRAINT [DF_ChgOverCheckList_BrandID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60) CONSTRAINT [DF_ChgOverCheckList_Description] DEFAULT ('') NOT NULL,
    [DaysBefore]  SMALLINT      CONSTRAINT [DF_ChgOverCheckList_DaysBefore] DEFAULT ((0)) NULL,
    [BaseOn]      TINYINT       CONSTRAINT [DF_ChgOverCheckList_BaseOn] DEFAULT ((0)) NOT NULL,
    [UseFor]      VARCHAR (1)   CONSTRAINT [DF_ChgOverCheckList_UseFor] DEFAULT ('') NOT NULL,
    [Junk]        BIT           CONSTRAINT [DF_ChgOverCheckList_Junk] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_ChgOverCheckList_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_ChgOverCheckList_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_ChgOverCheckList_1] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Changeover Check List Activities index', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'換款前幾天', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'DaysBefore';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'準備日期是依照Change Over或SCI delivery計算', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'BaseOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'New or Repeat/All', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'UseFor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOverCheckList', @level2type = N'COLUMN', @level2name = N'EditDate';

