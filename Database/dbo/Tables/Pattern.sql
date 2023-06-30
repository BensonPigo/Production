CREATE TABLE [dbo].[Pattern] (
    [ID]            VARCHAR (10)   CONSTRAINT [DF_Pattern_ID] DEFAULT ('') NOT NULL,
    [Version]       VARCHAR (3)    CONSTRAINT [DF_Pattern_Version] DEFAULT ('') NOT NULL,
    [BrandID]       VARCHAR (8)    CONSTRAINT [DF_Pattern_BrandID] DEFAULT ('') NOT NULL,
    [ActFtyPattern] VARCHAR (8)    CONSTRAINT [DF_Pattern_ActFtyPattern] DEFAULT ('') NOT NULL,
    [PatternNO]     VARCHAR (10)   CONSTRAINT [DF_Pattern_PatternNO] DEFAULT ('') NOT NULL,
    [RevisedReason] VARCHAR (4)    CONSTRAINT [DF_Pattern_RevisedReason] DEFAULT ('') NOT NULL,
    [PatternName]   VARCHAR (10)   CONSTRAINT [DF_Pattern_PatternName] DEFAULT ('') NOT NULL,
    [EstFinDate]    DATE           NULL,
    [ActFinDate]    DATETIME       NULL,
    [CheckerName]   VARCHAR (10)   CONSTRAINT [DF_Pattern_CheckerName] DEFAULT ('') NOT NULL,
    [CheckerDate]   DATETIME       NULL,
    [Status]        VARCHAR (15)   CONSTRAINT [DF_Pattern_Status] DEFAULT ('') NOT NULL,
    [CFMName]       VARCHAR (10)   CONSTRAINT [DF_Pattern_CFMName] DEFAULT ('') NOT NULL,
    [UKey]          BIGINT         CONSTRAINT [DF_Pattern_UKey] DEFAULT ((0)) NOT NULL,
    [StyleRemark]   NVARCHAR (MAX) CONSTRAINT [DF_Pattern_StyleRemark] DEFAULT ('') NOT NULL,
    [HisRemark]     NVARCHAR (MAX) CONSTRAINT [DF_Pattern_HisRemark] DEFAULT ('') NOT NULL,
    [PendingRemark] NVARCHAR (MAX) CONSTRAINT [DF_Pattern_PendingRemark] DEFAULT ('') NOT NULL,
    [SizeRound]     BIT            CONSTRAINT [DF_Pattern_SizeRound] DEFAULT ((0)) NOT NULL,
    [SizeRange]     NVARCHAR (MAX) CONSTRAINT [DF_Pattern_SizeRange] DEFAULT ('') NOT NULL,
    [StyleUkey]     BIGINT         CONSTRAINT [DF_Pattern_StyleUkey] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_Pattern_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_Pattern_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME       NULL,
    CONSTRAINT [PK_Pattern] PRIMARY KEY CLUSTERED ([ID] ASC, [Version] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'查版人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'CheckerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'查版日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'CheckerDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'CFMName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'UKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版房對款示的備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'StyleRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷史備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'HisRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'待確認備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'PendingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否全段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'SizeRound';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'指定尺碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'SizeRange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'StyleUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版作業 - 基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = 'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際打版工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'ActFtyPattern';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'PatternNO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'改版原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'RevisedReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版師', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'PatternName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計完成日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'EstFinDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際完成日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'ActFinDate';


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20170519-110832]
    ON [dbo].[Pattern]([StyleUkey] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_Pattern_PatternNO]
    ON [dbo].[Pattern]([PatternNO] ASC);

