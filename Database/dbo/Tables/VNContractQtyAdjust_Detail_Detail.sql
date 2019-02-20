CREATE TABLE [dbo].[VNContractQtyAdjust_Detail_Detail] (
    [ID]         BIGINT          NOT NULL,
    [Refno]      VARCHAR (20)    NOT NULL,
    [FabricType] VARCHAR (1)     NULL,
    [NLCode]     VARCHAR (5)     NOT NULL,
    [Qty]        NUMERIC (14, 3) NULL,
    CONSTRAINT [PK_VNContractQtyAdjust_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [NLCode] ASC)
);



