CREATE TABLE [dbo].[PayTermAP] (
    [ID]              VARCHAR (5)   CONSTRAINT [DF_PayTermAP_ID] DEFAULT ('') NOT NULL,
    [Description]     NVARCHAR (40) CONSTRAINT [DF_PayTermAP_Description] DEFAULT ('') NULL,
    [Term]            VARCHAR (5)   CONSTRAINT [DF_PayTermAP_Term] DEFAULT ('') NULL,
    [BeforeAfter]     VARCHAR (1)   CONSTRAINT [DF_PayTermAP_BeforeAfter] DEFAULT ('') NULL,
    [BaseDate]        VARCHAR (1)   CONSTRAINT [DF_PayTermAP_BaseDate] DEFAULT ('') NULL,
    [AccountDay]      TINYINT       CONSTRAINT [DF_PayTermAP_AccountDay] DEFAULT ((0)) NULL,
    [CloseAccountDay] VARCHAR (1)   CONSTRAINT [DF_PayTermAP_CloseAccountDay] DEFAULT ('') NULL,
    [CloseMonth]      TINYINT       CONSTRAINT [DF_PayTermAP_CloseMonth] DEFAULT ((0)) NULL,
    [CloseDay]        TINYINT       CONSTRAINT [DF_PayTermAP_CloseDay] DEFAULT ((0)) NULL,
    [DueAccountday]   VARCHAR (1)   CONSTRAINT [DF_PayTermAP_DueAccountday] DEFAULT ('') NULL,
    [DueMonth]        TINYINT       CONSTRAINT [DF_PayTermAP_DueMonth] DEFAULT ((0)) NULL,
    [DueDay]          TINYINT       CONSTRAINT [DF_PayTermAP_DueDay] DEFAULT ((0)) NULL,
    [JUNK]            BIT           CONSTRAINT [DF_PayTermAP_JUNK] DEFAULT ((0)) NULL,
    [AddName]         VARCHAR (10)  CONSTRAINT [DF_PayTermAP_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  CONSTRAINT [DF_PayTermAP_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME      NULL,
    CONSTRAINT [PK_PayTermAP] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'B13. Payment Term - AP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'Term';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Before / After', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'BeforeAfter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'立帳基準日(1.結關日/2.發票日/3.出貨日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'BaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計結算日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'AccountDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結算日-當日/月底(結帳日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'CloseAccountDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 月(結帳日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'CloseMonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 天(結帳日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'CloseDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結算日-當日/月底(到期日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'DueAccountday';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 月(到期日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'DueMonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 天(到期日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'DueDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'JUNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAP', @level2type = N'COLUMN', @level2name = N'AddName';

