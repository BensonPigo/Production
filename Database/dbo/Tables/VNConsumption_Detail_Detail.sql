CREATE TABLE [dbo].[VNConsumption_Detail_Detail] (
    [ID]        VARCHAR (13)    NOT NULL,
    [NLCode]    VARCHAR (5)     CONSTRAINT [DF_VNConsumption_Detail _Detail_NLCode] DEFAULT ('') NOT NULL,
    [SCIRefno]  VARCHAR (26)    CONSTRAINT [DF_VNConsumption_Detail _Detail_SCIRefno] DEFAULT ('') NOT NULL,
    [RefNo]     VARCHAR (20)    CONSTRAINT [DF_VNConsumption_Detail _Detail_RefNo] DEFAULT ('') NULL,
    [Qty]       NUMERIC (12, 4) CONSTRAINT [DF_VNConsumption_Detail _Detail_Qty] DEFAULT ((0)) NULL,
    [LocalItem] BIT             CONSTRAINT [DF_VNConsumption_Detail _Detail_LocalItem] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_VNConsumption_Detail _Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [NLCode] ASC, [SCIRefno] ASC)
);

