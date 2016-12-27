CREATE TABLE [dbo].[VNConsumption_Detail_Detail] (
    [ID]            VARCHAR (13)    NOT NULL,
    [NLCode]        VARCHAR (5)     CONSTRAINT [DF_VNConsumption_Detail _Detail_NLCode] DEFAULT ('') NOT NULL,
    [SCIRefno]      VARCHAR (26)    CONSTRAINT [DF_VNConsumption_Detail _Detail_SCIRefno] DEFAULT ('') NOT NULL,
    [RefNo]         VARCHAR (21)    CONSTRAINT [DF_VNConsumption_Detail _Detail_RefNo] DEFAULT ('') NOT NULL,
    [Qty]           NUMERIC (12, 4) CONSTRAINT [DF_VNConsumption_Detail _Detail_Qty] DEFAULT ((0)) NOT NULL,
    [LocalItem]     BIT             CONSTRAINT [DF_VNConsumption_Detail _Detail_LocalItem] DEFAULT ((0)) NULL,
    [FabricBrandID] VARCHAR (8)     DEFAULT ('') NULL,
    [FabricType]    VARCHAR (1)     DEFAULT ('') NULL,
    [UKey]          BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_VNConsumption_Detail _Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);



