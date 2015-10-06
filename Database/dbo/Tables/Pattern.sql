CREATE TABLE [dbo].[Pattern] (
    [ID]            VARCHAR (10)   CONSTRAINT [DF_Pattern_ID] DEFAULT ('') NOT NULL,
    [Version]       VARCHAR (3)    CONSTRAINT [DF_Pattern_Version] DEFAULT ('') NOT NULL,
    [Brand]         VARCHAR (8)    CONSTRAINT [DF_Pattern_Brand] DEFAULT ('') NULL,
    [ActFtyPattern] VARCHAR (8)    CONSTRAINT [DF_Pattern_ActFtyPattern] DEFAULT ('') NULL,
    [PatternNO]     VARCHAR (10)   CONSTRAINT [DF_Pattern_PatternNO] DEFAULT ('') NULL,
    [RevisedReason] VARCHAR (4)    CONSTRAINT [DF_Pattern_RevisedReason] DEFAULT ('') NULL,
    [PatternName]   VARCHAR (10)   CONSTRAINT [DF_Pattern_PatternName] DEFAULT ('') NULL,
    [EstFinDate]    DATE           NULL,
    [ActFinDate]    DATETIME       NULL,
    [CheckerName]   VARCHAR (10)   CONSTRAINT [DF_Pattern_CheckerName] DEFAULT ('') NULL,
    [CheckerDate]   DATETIME       NULL,
    [Status]        VARCHAR (1)    CONSTRAINT [DF_Pattern_Status] DEFAULT ('') NULL,
    [CFMName]       VARCHAR (10)   CONSTRAINT [DF_Pattern_CFMName] DEFAULT ('') NULL,
    [UKey]          BIGINT         CONSTRAINT [DF_Pattern_UKey] DEFAULT ((0)) NULL,
    [StyleRemark]   NVARCHAR (MAX) CONSTRAINT [DF_Pattern_StyleRemark] DEFAULT ('') NULL,
    [HisRemark]     NVARCHAR (MAX) CONSTRAINT [DF_Pattern_HisRemark] DEFAULT ('') NULL,
    [PendingRemark] NVARCHAR (MAX) CONSTRAINT [DF_Pattern_PendingRemark] DEFAULT ('') NULL,
    [SizeRound]     BIT            CONSTRAINT [DF_Pattern_SizeRound] DEFAULT ((0)) NULL,
    [SizeRange]     NVARCHAR (100) CONSTRAINT [DF_Pattern_SizeRange] DEFAULT ('') NULL,
    [StyleUkey]     VARCHAR (10)   CONSTRAINT [DF_Pattern_StyleUkey] DEFAULT ('') NULL,
    [Translate]     BIT            CONSTRAINT [DF_Pattern_Translate] DEFAULT ((0)) NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_Pattern_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_Pattern_EditName] DEFAULT ('') NULL,
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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'翻譯', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'Translate';


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pattern', @level2type = N'COLUMN', @level2name = N'Brand';


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

