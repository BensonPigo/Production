CREATE TABLE [dbo].[PadPrintReq_Detail] (
    [ID]            VARCHAR (13)    CONSTRAINT [DF_PadPrintReq_Detail_ID] DEFAULT ('') NOT NULL,
    [Seq2]          VARCHAR (2)     CONSTRAINT [DF_PadPrintReq_Detail_Seq2] DEFAULT ('') NOT NULL,
    [PadPrint_Ukey] BIGINT          NOT NULL,
    [Refno]         VARCHAR (36)    CONSTRAINT [DF_PadPrintReq_Detail_Refno] DEFAULT ('') NULL,
    [MoldID]        VARCHAR (10)    CONSTRAINT [DF_PadPrintReq_Detail_MoldID] DEFAULT ('') NOT NULL,
    [SourceID]      VARCHAR (13)    CONSTRAINT [DF_PadPrintReq_Detail_SourceID] DEFAULT ('') NOT NULL,
    [Price]         NUMERIC (10, 4) CONSTRAINT [DF_PadPrintReq_Detail_Price] DEFAULT ((0)) NOT NULL,
    [Qty]           NUMERIC (10)    CONSTRAINT [DF_PadPrintReq_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [Foc]           NUMERIC (10)    CONSTRAINT [DF_PadPrintReq_Detail_Foc] DEFAULT ((0)) NOT NULL,
    [ShipModeID]    VARCHAR (10)    CONSTRAINT [DF_PadPrintReq_Detail_ShipModeID] DEFAULT ('') NOT NULL,
    [SuppID]        VARCHAR (6)     CONSTRAINT [DF_PadPrintReq_Detail_SuppID] DEFAULT ('') NOT NULL,
    [CurrencyID]    VARCHAR (3)     CONSTRAINT [DF_PadPrintReq_Detail_CurrencyID] DEFAULT ('') NOT NULL,
    [Junk]          BIT             CONSTRAINT [DF_PadPrintReq_Detail_Junk] DEFAULT ((0)) NOT NULL,
    [Remark]        NVARCHAR (1000) CONSTRAINT [DF_PadPrintReq_Detail_Remark] DEFAULT ('') NOT NULL,
    [POID]          VARCHAR (13)    CONSTRAINT [DF_PadPrintReq_Detail_POID] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_PadPrintReq_Detail_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_PadPrintReq_Detail_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME        NULL,
    CONSTRAINT [PK_PadPrintReq_Detail_1] PRIMARY KEY CLUSTERED ([ID] ASC, [Seq2] ASC, [PadPrint_Ukey] ASC, [MoldID] ASC)
);

