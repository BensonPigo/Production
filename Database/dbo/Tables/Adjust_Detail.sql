CREATE TABLE [dbo].[Adjust_Detail] (
    [ID]               VARCHAR (13)    NOT NULL,
    [FtyInventoryUkey] BIGINT          NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_Adjust_Detail_MDivisionID] DEFAULT ('') NULL,
    [POID]             VARCHAR (13)    CONSTRAINT [DF_Adjust_Detail_POID] DEFAULT ('') NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_Adjust_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]             VARCHAR (2)     CONSTRAINT [DF_Adjust_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]             VARCHAR (8)     CONSTRAINT [DF_Adjust_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]           VARCHAR (8)     CONSTRAINT [DF_Adjust_Detail_Dyelot] DEFAULT ('') NULL,
    [StockType]        CHAR (1)        CONSTRAINT [DF_Adjust_Detail_StockType] DEFAULT ('') NULL,
    [QtyBefore]        NUMERIC (11, 2) CONSTRAINT [DF_Adjust_Detail_QtyBefore] DEFAULT ((0)) NULL,
    [QtyAfter]         NUMERIC (11, 2) CONSTRAINT [DF_Adjust_Detail_QtyAfter] DEFAULT ((0)) NULL,
    [ReasonId]         VARCHAR (5)     CONSTRAINT [DF_Adjust_Detail_ReasonId] DEFAULT ('') NULL,
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompleteTime] DATETIME NULL, 
    CONSTRAINT [PK_Adjust_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存調整明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust_Detail';


GO



GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整前數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust_Detail', @level2type = N'COLUMN', @level2name = N'QtyBefore';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust_Detail', @level2type = N'COLUMN', @level2name = N'QtyAfter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adjust_Detail', @level2type = N'COLUMN', @level2name = N'ReasonId';


GO


