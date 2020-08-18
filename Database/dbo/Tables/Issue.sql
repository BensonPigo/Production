CREATE TABLE [dbo].[Issue] (
    [Id]           VARCHAR (13)  CONSTRAINT [DF_Issue_Id] DEFAULT ('') NOT NULL,
    [Type]         VARCHAR (1)   CONSTRAINT [DF_Issue_Type] DEFAULT ('') NOT NULL,
    [IssueDate]    DATE          NOT NULL,
    [MDivisionID]  VARCHAR (8)   CONSTRAINT [DF_Issue_MDivisionID] DEFAULT ('') NOT NULL,
	[FactoryID]    VARCHAR (8)   CONSTRAINT [DF_Issue_FactoryID] DEFAULT ('') NOT NULL,
    [Status]       VARCHAR (15)  CONSTRAINT [DF_Issue_Status] DEFAULT ('') NULL,
    [CutplanID]    VARCHAR (13)  CONSTRAINT [DF_Issue_CutplanID] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (100) CONSTRAINT [DF_Issue_Remark] DEFAULT ('') NULL,
    [ApvName]      VARCHAR (10)  CONSTRAINT [DF_Issue_ApvName] DEFAULT ('') NULL,
    [ApvDate]      DATE          NULL,
    [WhseReasonID] VARCHAR (7)   CONSTRAINT [DF_Issue_WhseReasonID] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)  CONSTRAINT [DF_Issue_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [EditName]     VARCHAR (10)  CONSTRAINT [DF_Issue_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    [OrderId]      VARCHAR (13)  CONSTRAINT [DF_Issue_OrderId] DEFAULT ('') NULL,
    [Combo] BIT NOT NULL DEFAULT ('0'), 
    [PrintDate] DATETIME NULL, 
    [PrintName] VARCHAR(10) CONSTRAINT [DF_Issue_PrintName] DEFAULT ('') NULL,
    [ToSisterFty] BIT NOT NULL DEFAULT ((0)), 
    [ToFactory] VARCHAR(8) NULL, 
    [SewLine] VARCHAR(500) NULL, 
    CONSTRAINT [PK_Issue] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪計畫id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'CutplanID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'WhseReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄這項物料是否是要給姊妹場的',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Issue',
    @level2type = N'COLUMN',
    @level2name = N'ToSisterFty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發給姊妹廠的哪間工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Issue',
    @level2type = N'COLUMN',
    @level2name = N'ToFactory'