CREATE TABLE [dbo].[PPICReason] (
    [Type]        VARCHAR (2)    CONSTRAINT [DF_PPICReason_Type] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (5)    CONSTRAINT [DF_PPICReason_ID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60)  CONSTRAINT [DF_PPICReason_Description] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (100) CONSTRAINT [DF_PPICReason_Remark] DEFAULT ('') NOT NULL,
    [Junk]        BIT            CONSTRAINT [DF_PPICReason_Junk] DEFAULT ((0)) NOT NULL,
    [TypeForUse]  VARCHAR (1)    CONSTRAINT [DF_PPICReason_TypeForUse] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_PPICReason_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_PPICReason_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME       NULL,
    [DeptID]      VARCHAR (8)    CONSTRAINT [DF_PPICReason_DeptID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_PPICReason] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lacking & Replacement時分別帶出此原因是給誰使用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'TypeForUse';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PPICReason', @level2type = N'COLUMN', @level2name = N'DeptID';

