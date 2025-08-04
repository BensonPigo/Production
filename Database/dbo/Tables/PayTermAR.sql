CREATE TABLE [dbo].[PayTermAR] (
    [ID]              VARCHAR (10)  CONSTRAINT [DF_PayTermAR_ID] DEFAULT ('') NOT NULL,
    [Description]     NVARCHAR (50) CONSTRAINT [DF_PayTermAR_Description] DEFAULT ('') NOT NULL,
    [Term]            VARCHAR (5)   CONSTRAINT [DF_PayTermAR_Term] DEFAULT ('') NOT NULL,
    [BeforeAfter]     VARCHAR (1)   CONSTRAINT [DF_PayTermAR_BeforeAfter] DEFAULT ('') NOT NULL,
    [BaseDate]        VARCHAR (1)   CONSTRAINT [DF_PayTermAR_BaseDate] DEFAULT ('') NOT NULL,
    [AccountDay]      TINYINT       CONSTRAINT [DF_PayTermAR_AccountDay] DEFAULT ((0)) NOT NULL,
    [CloseAccountDay] VARCHAR (1)   CONSTRAINT [DF_PayTermAR_CloseAccountDay] DEFAULT ('') NOT NULL,
    [CloseMonth]      TINYINT       CONSTRAINT [DF_PayTermAR_CloseMonth] DEFAULT ((0)) NOT NULL,
    [CloseDay]        TINYINT       CONSTRAINT [DF_PayTermAR_CloseDay] DEFAULT ((0)) NOT NULL,
    [DueAccountday]   VARCHAR (1)   CONSTRAINT [DF_PayTermAR_DueAccountday] DEFAULT ('') NOT NULL,
    [DueMonth]        TINYINT       CONSTRAINT [DF_PayTermAR_DueMonth] DEFAULT ((0)) NOT NULL,
    [DueDay]          TINYINT       CONSTRAINT [DF_PayTermAR_DueDay] DEFAULT ((0)) NOT NULL,
    [JUNK]            BIT           CONSTRAINT [DF_PayTermAR_JUNK] DEFAULT ((0)) NOT NULL,
    [SamplePI]        BIT           CONSTRAINT [DF_PayTermAR_SamplePI] DEFAULT ((0)) NOT NULL,
    [BulkPI]          BIT           CONSTRAINT [DF_PayTermAR_BulkPI] DEFAULT ((0)) NOT NULL,
    [AddName]         VARCHAR (10)  CONSTRAINT [DF_PayTermAR_AddName] DEFAULT ('') NOT NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  CONSTRAINT [DF_PayTermAR_EditName] DEFAULT ('') NOT NULL,
    [EditDate]        DATETIME      NULL,
    CONSTRAINT [PK_PayTermAR] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'B14. Payment Term - AR', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'Term';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Before / After', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'BeforeAfter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'立帳基準日(1.結關日/2.發票日/3.出貨日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'BaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計結算日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'AccountDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結算日-當日/月底(結帳日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'CloseAccountDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 月(結帳日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'CloseMonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 天(結帳日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'CloseDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結算日-當日/月底(到期日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'DueAccountday';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 月(到期日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'DueMonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加 ?? 天(到期日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'DueDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'JUNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需要P/I#(銷樣)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'SamplePI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需要P/I#(大貨)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'BulkPI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PayTermAR', @level2type = N'COLUMN', @level2name = N'EditDate';

