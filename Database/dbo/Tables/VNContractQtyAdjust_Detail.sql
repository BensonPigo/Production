CREATE TABLE [dbo].[VNContractQtyAdjust_Detail] (
    [ID]     BIGINT          NOT NULL,
    [NLCode] VARCHAR (5)     CONSTRAINT [DF_VNContractQtyAdjust_Detail_NLCode] DEFAULT ('') NOT NULL,
    [Qty]    NUMERIC (14, 3) CONSTRAINT [DF_VNContractQtyAdjust_Detail_Qty] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_VNContractQtyAdjust_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [NLCode] ASC)
);

