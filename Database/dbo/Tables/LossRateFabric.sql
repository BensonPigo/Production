CREATE TABLE [dbo].[LossRateFabric] (
    [WeaveTypeID]    VARCHAR (20)   CONSTRAINT [DF_LossRateFabric_WeaveTypeID] DEFAULT ('') NOT NULL,
    [LossType]       TINYINT        CONSTRAINT [DF_LossRateFabric_LossType] DEFAULT ((0)) NOT NULL,
    [Limit]          SMALLINT       CONSTRAINT [DF_LossRateFabric_Limit] DEFAULT ((0)) NOT NULL,
    [LimitDown]      TINYINT        CONSTRAINT [DF_LossRateFabric_LimitDown] DEFAULT ((0)) NOT NULL,
    [TWLimitDown]    NUMERIC (4, 1) CONSTRAINT [DF_LossRateFabric_TWLimitDown] DEFAULT ((0)) NOT NULL,
    [NonTWLimitDown] NUMERIC (4, 1) CONSTRAINT [DF_LossRateFabric_NonTWLimitDown] DEFAULT ((0)) NOT NULL,
    [LimitUp]        TINYINT        CONSTRAINT [DF_LossRateFabric_LimitUp] DEFAULT ((0)) NOT NULL,
    [TWLimitUp]      NUMERIC (4, 1) CONSTRAINT [DF_LossRateFabric_TWLimitUp] DEFAULT ((0)) NOT NULL,
    [NonTWLimitUP]   NUMERIC (4, 1) CONSTRAINT [DF_LossRateFabric_NonTWLimitUP] DEFAULT ((0)) NOT NULL,
    [Allowance]      SMALLINT       CONSTRAINT [DF_LossRateFabric_Allowance] DEFAULT ((0)) NOT NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_LossRateFabric_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME       NULL,
    [EditName]       VARCHAR (10)   CONSTRAINT [DF_LossRateFabric_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME       NULL,
    [MaxLossQty]     NUMERIC (6)    DEFAULT ((0)) NULL,
    [MinGmtQty]      NUMERIC (6)    NULL,
    [MinLossQty]     NUMERIC (6)    NULL,
    [PerGmtQty]      NUMERIC (6)    NULL,
    [PlsLossQty]     NUMERIC (6)    NULL,
    CONSTRAINT [PK_LossRateFabric] PRIMARY KEY CLUSTERED ([WeaveTypeID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主料損耗設定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種織法', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'WeaveTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗判定', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'LossType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分界點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'Limit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分界點以下損耗單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'LimitDown';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗 - 台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'TWLimitDown';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗 - 台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'NonTWLimitDown';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分界點以上損耗單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'LimitUp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗 - 台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'TWLimitUp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗 - 非台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'NonTWLimitUP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗上限值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'Allowance';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateFabric', @level2type = N'COLUMN', @level2name = N'EditDate';

