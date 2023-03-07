CREATE TABLE [dbo].[VNContractQtyAdjust_Detail_Detail] (
    [ID]         BIGINT          NOT NULL,
    [Refno]      VARCHAR (36)    NOT NULL,
    [FabricType] VARCHAR (4)     NULL,
    [NLCode]     VARCHAR (9)     NOT NULL,
    [BrandID]    VARCHAR (8)     NOT NULL,
    [Qty]        NUMERIC (14, 3) NULL,
    [UsageUnit]  VARCHAR (8)     DEFAULT ('') NULL,
    CONSTRAINT [PK_VNContractQtyAdjust_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [NLCode] ASC, [BrandID] ASC)
);







