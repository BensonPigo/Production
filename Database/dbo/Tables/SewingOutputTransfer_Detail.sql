CREATE TABLE [dbo].[SewingOutputTransfer_Detail] (
    [ID]            VARCHAR (13) CONSTRAINT [DF_SewingOutputTransfer_Detail_ID] DEFAULT ('') NOT NULL,
    [FromOrderID]   VARCHAR (13) CONSTRAINT [DF_SewingOutputTransfer_Detail_FromOrderID] DEFAULT ('') NOT NULL,
    [Article]       VARCHAR (8)  CONSTRAINT [DF_SewingOutputTransfer_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]      VARCHAR (8)  CONSTRAINT [DF_SewingOutputTransfer_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [FromComboType] VARCHAR (1)  CONSTRAINT [DF_SewingOutputTransfer_Detail_FromComboType] DEFAULT ('') NOT NULL,
    [FromQty]       INT          CONSTRAINT [DF_SewingOutputTransfer_Detail_FromQty] DEFAULT ((0)) NOT NULL,
    [FromSewingQty] INT          CONSTRAINT [DF_SewingOutputTransfer_Detail_FromSewingQty] DEFAULT ((0)) NOT NULL,
    [ToOrderID]     VARCHAR (13) CONSTRAINT [DF_SewingOutputTransfer_Detail_ToOrderID] DEFAULT ('') NOT NULL,
    [ToComboType]   VARCHAR (1)  CONSTRAINT [DF_SewingOutputTransfer_Detail_ToComboType] DEFAULT ('') NOT NULL,
    [ToArticle]     VARCHAR (8)  CONSTRAINT [DF_SewingOutputTransfer_Detail_ToArticle] DEFAULT ('') NOT NULL,
    [ToSizeCode]    VARCHAR (8)  CONSTRAINT [DF_SewingOutputTransfer_Detail_ToSizeCode] DEFAULT ('') NOT NULL,
    [ToQty]         INT          CONSTRAINT [DF_SewingOutputTransfer_Detail_ToQty] DEFAULT ((0)) NOT NULL,
    [ToSewingQty]   INT          CONSTRAINT [DF_SewingOutputTransfer_Detail_ToSewingQty] DEFAULT ((0)) NOT NULL,
    [TransferQty]   INT          CONSTRAINT [DF_SewingOutputTransfer_Detail_TransferQty] DEFAULT ((0)) NOT NULL,
    [Ukey]          BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_SewingOutputTransfer_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



