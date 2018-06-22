CREATE TABLE [dbo].[ThreadRequisition_Detail_Cons] (
    [OrderID]                      VARCHAR (13)   CONSTRAINT [DF_ThreadRequisition_Detail_Cons_OrderID] DEFAULT ('') NULL,
    [Article]                      VARCHAR (8)    CONSTRAINT [DF_ThreadRequisition_Detail_Cons_Article] DEFAULT ('') NULL,
    [ThreadCombID]                 VARCHAR (10)   CONSTRAINT [DF_ThreadRequisition_Detail_Cons_ThreadCombID] DEFAULT ('') NULL,
    [ThreadRequisition_DetailUkey] BIGINT         CONSTRAINT [DF_ThreadRequisition_Detail_Cons_ThreadRequisition_DetailUkey] DEFAULT ('') NOT NULL,
    [Operationid]                  VARCHAR (20)   CONSTRAINT [DF_ThreadRequisition_Detail_Cons_Operationid] DEFAULT ('') NULL,
    [SeamLength]                   NUMERIC (12, 2) CONSTRAINT [DF_ThreadRequisition_Detail_Cons_SeamLength] DEFAULT ((0)) NULL,
    [SEQ]                          VARCHAR (2)    CONSTRAINT [DF_ThreadRequisition_Detail_Cons_SEQ] DEFAULT ('') NULL,
    [ThreadLocationID]             VARCHAR (20)   CONSTRAINT [DF_ThreadRequisition_Detail_Cons_ThreadLocationID] DEFAULT ('') NULL,
    [UseRatio]                     VARCHAR (15)   CONSTRAINT [DF_ThreadRequisition_Detail_Cons_UseRatio] DEFAULT ('') NULL,
    [UseRatioNumeric]              NUMERIC (6, 2) NULL,
    [Machinetypeid]                VARCHAR (10)   CONSTRAINT [DF_ThreadRequisition_Detail_Cons_Machinetypeid] DEFAULT ('') NULL,
    [OrderQty]                     NUMERIC (6)    CONSTRAINT [DF_ThreadRequisition_Detail_Cons_OrderQty] DEFAULT ((0)) NULL,
    [Ukey]                         BIGINT         IDENTITY (1, 1) NOT NULL,
    [Allowance] NUMERIC(4, 2) NULL DEFAULT ((0)), 
    CONSTRAINT [PK_ThreadRequisition_Detail_Cons] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Cons Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Article';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線部位組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'ThreadCombID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'ThreadRequisition_DetailUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工段號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Operationid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用線車縫長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'SeamLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'ThreadLocationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用 Ratio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'UseRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器工段種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Machinetypeid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單同Article 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition_Detail_Cons', @level2type = N'COLUMN', @level2name = N'Ukey';


GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Allowance',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ThreadRequisition_Detail_Cons',
    @level2type = N'COLUMN',
    @level2name = N'Allowance'