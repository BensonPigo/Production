CREATE TABLE [dbo].[ReplacementReport_Detail] (
    [ID]                   VARCHAR (13)   CONSTRAINT [DF_ReplacementReport_Detail_ID] DEFAULT ('') NOT NULL,
    [Seq1]                 VARCHAR (3)    CONSTRAINT [DF_ReplacementReport_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]                 VARCHAR (2)    CONSTRAINT [DF_ReplacementReport_Detail_Seq2] DEFAULT ('') NOT NULL,
    [Refno]                VARCHAR (36)   CONSTRAINT [DF_ReplacementReport_Detail_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]             VARCHAR (30)   CONSTRAINT [DF_ReplacementReport_Detail_SCIRefno] DEFAULT ('') NOT NULL,
    [INVNo]                VARCHAR (25)   CONSTRAINT [DF_ReplacementReport_Detail_INVNo] DEFAULT ('') NOT NULL,
    [ETA]                  DATE           NULL,
    [ColorID]              VARCHAR (6)    CONSTRAINT [DF_ReplacementReport_Detail_ColorID] DEFAULT ('') NOT NULL,
    [EstInQty]             DECIMAL (8, 2) CONSTRAINT [DF_ReplacementReport_Detail_EstInQty] DEFAULT ((0)) NOT NULL,
    [ActInQty]             DECIMAL (8, 2) CONSTRAINT [DF_ReplacementReport_Detail_ActInQty] DEFAULT ((0)) NOT NULL,
    [AGradeDefect]         VARCHAR (20)   CONSTRAINT [DF_ReplacementReport_Detail_AGradeDefect] DEFAULT ('') NOT NULL,
    [AGradeRequest]        DECIMAL (8, 2) CONSTRAINT [DF_ReplacementReport_Detail_AGradeRequest] DEFAULT ((0)) NOT NULL,
    [BGradeDefect]         VARCHAR (20)   CONSTRAINT [DF_ReplacementReport_Detail_BGradeDefect] DEFAULT ('') NOT NULL,
    [BGradeRequest]        DECIMAL (8, 2) CONSTRAINT [DF_ReplacementReport_Detail_BGradeRequest] DEFAULT ((0)) NOT NULL,
    [NarrowWidth]          VARCHAR (20)   CONSTRAINT [DF_ReplacementReport_Detail_NarrowWidth] DEFAULT ('') NOT NULL,
    [NarrowRequest]        DECIMAL (8, 2) CONSTRAINT [DF_ReplacementReport_Detail_NarrowRequest] DEFAULT ((0)) NOT NULL,
    [Other]                VARCHAR (5)    CONSTRAINT [DF_ReplacementReport_Detail_Other] DEFAULT ('') NOT NULL,
    [OtherReason]          VARCHAR (30)   CONSTRAINT [DF_ReplacementReport_Detail_OtherReason] DEFAULT ('') NOT NULL,
    [OtherRequest]         DECIMAL (8, 2) CONSTRAINT [DF_ReplacementReport_Detail_OtherRequest] DEFAULT ((0)) NOT NULL,
    [TotalRequest]         DECIMAL (9, 2) CONSTRAINT [DF_ReplacementReport_Detail_TotalRequest] DEFAULT ((0)) NOT NULL,
    [AfterCutting]         VARCHAR (5)    CONSTRAINT [DF_ReplacementReport_Detail_AfterCutting] DEFAULT ('') NOT NULL,
    [AfterCuttingReason]   VARCHAR (30)   CONSTRAINT [DF_ReplacementReport_Detail_AfterCuttingReason] DEFAULT ('') NOT NULL,
    [AfterCuttingRequest]  DECIMAL (8, 2) CONSTRAINT [DF_ReplacementReport_Detail_AfterCuttingRequest] DEFAULT ((0)) NOT NULL,
    [DamageSendDate]       DATE           NULL,
    [AWBNo]                VARCHAR (30)   CONSTRAINT [DF_ReplacementReport_Detail_AWBNo] DEFAULT ('') NOT NULL,
    [ReplacementETA]       DATE           NULL,
    [Responsibility]       VARCHAR (1)    CONSTRAINT [DF_ReplacementReport_Detail_Responsibility] DEFAULT ('') NOT NULL,
    [ResponsibilityReason] NVARCHAR (100) CONSTRAINT [DF_ReplacementReport_Detail_ResponsibilityReason] DEFAULT ('') NOT NULL,
    [Suggested]            NVARCHAR (100) CONSTRAINT [DF_ReplacementReport_Detail_Suggested] DEFAULT ('') NOT NULL,
    [OccurCost]            DECIMAL (7, 3) CONSTRAINT [DF_ReplacementReport_Detail_OccurCost] DEFAULT ((0)) NOT NULL,
    [UKey]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [OldFabricUkey]        VARCHAR (10)   CONSTRAINT [DF_ReplacementReport_Detail_OldFabricUkey] DEFAULT ('') NOT NULL,
    [OldFabricVer]         VARCHAR (2)    CONSTRAINT [DF_ReplacementReport_Detail_OldFabricVer] DEFAULT ('') NOT NULL,
    [Junk]                 BIT            CONSTRAINT [DF_ReplacementReport_Detail_Junk] DEFAULT ((0)) NOT NULL,
    [NewSeq1]              VARCHAR (3)    CONSTRAINT [DF_ReplacementReport_Detail_NewSeq1] DEFAULT ('') NOT NULL,
    [NewSeq2]              VARCHAR (2)    CONSTRAINT [DF_ReplacementReport_Detail_NewSeq2] DEFAULT ('') NOT NULL,
    [PurchaseID]           VARCHAR (13)   CONSTRAINT [DF_ReplacementReport_Detail_PurchaseID] DEFAULT ('') NOT NULL,
    [FinalNeedQty]         NUMERIC (9, 2) CONSTRAINT [DF_ReplacementReport_Detail_FinalNeedQty] DEFAULT ((0)) NOT NULL,
    [ReplacementUnit]      VARCHAR (8)    CONSTRAINT [DF_ReplacementReport_Detail_ReplacementUnit] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ReplacementReport_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [UKey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Replacement Report Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料報告單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'INVNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計收料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'EstInQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際收貨數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'ActInQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A級品質點數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'AGradeDefect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A級需要碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'AGradeRequest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'B級品質點數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'BGradeDefect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'B級需要碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'BGradeRequest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Narrow Width', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'NarrowWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Narrow Width Request YDS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'NarrowRequest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Other Reason ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'Other';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Other Reason', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'OtherReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Other Request', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'OtherRequest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求總碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'TotalRequest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'剪裁後需求原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'AfterCutting';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'剪裁後需求原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'AfterCuttingReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'剪裁後需求碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'AfterCuttingRequest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵品寄回日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'DamageSendDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AWB# Of Damage Sample', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'AWBNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料最後到達日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'ReplacementETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'Responsibility';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'ResponsibilityReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠建議說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'Suggested';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'費用產生', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'OccurCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReplacementReport_Detail', @level2type = N'COLUMN', @level2name = N'UKey';

