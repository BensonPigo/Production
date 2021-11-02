CREATE TABLE [dbo].[WhseReason] (
    [Type]        VARCHAR (2)    CONSTRAINT [DF_WhseReason_Type] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (5)    CONSTRAINT [DF_WhseReason_ID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (200)  CONSTRAINT [DF_WhseReason_Description] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (100) CONSTRAINT [DF_WhseReason_Remark] DEFAULT ('') NULL,
    [Junk]        BIT            CONSTRAINT [DF_WhseReason_Junk] DEFAULT ((0)) NULL,
    [ActionCode]  VARCHAR (30)   CONSTRAINT [DF_WhseReason_ActionCode] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_WhseReason_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_WhseReason_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [No]          NUMERIC (3)    CONSTRAINT [DF_WhseReason_No] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_WhseReason] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Return Action', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'ActionCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WhseReason', @level2type = N'COLUMN', @level2name = N'No';

