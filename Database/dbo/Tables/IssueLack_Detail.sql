CREATE TABLE [dbo].[IssueLack_Detail] (
    [Id]               VARCHAR (13)    CONSTRAINT [DF_IssueLack_Detail_Id] DEFAULT ('') NOT NULL,
    [FtyInventoryUkey] BIGINT          NULL,
    [Qty]              NUMERIC (10, 2) CONSTRAINT [DF_IssueLack_Detail_Qty] DEFAULT ((0)) NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_IssueLack_Detail_MDivisionID] DEFAULT ('') NULL,
    [POID]             VARCHAR (13)    CONSTRAINT [DF_IssueLack_Detail_POID] DEFAULT ('') NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_IssueLack_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]             VARCHAR (2)     CONSTRAINT [DF_IssueLack_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]             VARCHAR (8)     CONSTRAINT [DF_IssueLack_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]           VARCHAR (8)     CONSTRAINT [DF_IssueLack_Detail_Dyelot] DEFAULT ('') NULL,
    [StockType]        CHAR (1)        CONSTRAINT [DF_IssueLack_Detail_StockType] DEFAULT ('') NULL,
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Remark] NVARCHAR(60) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_IssueLack_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缺補料發料明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueLack_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO


