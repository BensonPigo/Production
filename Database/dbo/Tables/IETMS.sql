CREATE TABLE [dbo].[IETMS] (
    [ID]            VARCHAR (10)  CONSTRAINT [DF_IETMS_ID] DEFAULT ('') NOT NULL,
    [Version]       VARCHAR (3)   CONSTRAINT [DF_IETMS_Version] DEFAULT ('') NOT NULL,
    [Ukey]          BIGINT        CONSTRAINT [DF_IETMS_IETMSUkey] DEFAULT ((0)) NULL,
    [IEName]        VARCHAR (10)  CONSTRAINT [DF_IETMS_IEName] DEFAULT ('') NULL,
    [ActFinDate]    DATETIME      NULL,
    [GSDStyleCode]  VARCHAR (15)  CONSTRAINT [DF_IETMS_GSDStyleCode] DEFAULT ('') NULL,
    [GSDStyleTitle] NVARCHAR (50) CONSTRAINT [DF_IETMS_GSDStyleTitle] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_IETMS_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_IETMS_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [GSDType]       VARCHAR (1)   CONSTRAINT [DF_IETMS_GSDType] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_IETMS] PRIMARY KEY CLUSTERED ([ID] ASC, [Version] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE Operation 基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'Version';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE 人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'IEName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際完成日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'ActFinDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSD Style Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'GSDStyleCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSD Style Title', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'GSDStyleTitle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IEUKEY', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS', @level2type = N'COLUMN', @level2name = N'Ukey';

