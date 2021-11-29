CREATE TABLE [dbo].[Issue_MIND] (
    [Ukey]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [ID]       VARCHAR (13) NOT NULL,
    [Releaser] VARCHAR (10) NOT NULL,
    [AddName]  VARCHAR (10) NOT NULL,
    [AddDate]  DATETIME     NULL,
    CONSTRAINT [PK_Issue_MIND] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_MIND', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_MIND', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MIND被指派的發料人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_MIND', @level2type = N'COLUMN', @level2name = N'Releaser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_MIND', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主鍵(流水號)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_MIND', @level2type = N'COLUMN', @level2name = N'Ukey';

