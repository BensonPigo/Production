CREATE TABLE [dbo].[FreightCollectByCustomer] (
    [BrandID]     VARCHAR (8)    NOT NULL,
    [Dest]        VARCHAR (2)    NOT NULL,
    [CarrierID]   VARCHAR (15)   NOT NULL,
    [Account]     VARCHAR (15)   NOT NULL,
    [CustCDID]    VARCHAR (15)   CONSTRAINT [DF_FreightCollectByCustomer_CustCDID] DEFAULT ('') NOT NULL,
    [DestPort]    VARCHAR (15)   CONSTRAINT [DF_FreightCollectByCustomer_DestPort] DEFAULT ('') NOT NULL,
    [OrderTypeID] NVARCHAR (100) CONSTRAINT [DF_FreightCollectByCustomer_OrderTypeID] DEFAULT ('') NOT NULL,
    [Remarks]     NVARCHAR (200) CONSTRAINT [DF_FreightCollectByCustomer_Remarks] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    [AddName]     VARCHAR (10)   NULL,
    [EditDate]    DATETIME       NULL,
    [EditName]    VARCHAR (10)   NULL,
    CONSTRAINT [PK_FreightCollectByCustomer] PRIMARY KEY CLUSTERED ([Dest] ASC, [BrandID] ASC, [CarrierID] ASC, [Account] ASC)
);

