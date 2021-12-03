CREATE TABLE [dbo].[ClogLocation] (
    [ID]          NVARCHAR(50)  CONSTRAINT [DF_ClogLocation_ID] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)   CONSTRAINT [DF_ClogLocation_MDivisionID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (40) CONSTRAINT [DF_ClogLocation_Description] DEFAULT ('') NOT NULL,
    [Junk]        BIT           CONSTRAINT [DF_ClogLocation_Junk] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_ClogLocation_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_ClogLocation_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_ClogLocation] PRIMARY KEY CLUSTERED ([ID] ASC, [MDivisionID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Clog Location資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogLocation', @level2type = N'COLUMN', @level2name = N'MDivisionID';

