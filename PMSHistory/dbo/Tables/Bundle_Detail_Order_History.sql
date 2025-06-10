CREATE TABLE [dbo].[Bundle_Detail_Order_History] (
    [Ukey]     BIGINT       IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ID]       VARCHAR (13) NOT NULL,
    [BundleNo] VARCHAR (10) CONSTRAINT [DF_Bundle_Detail_Order_History_BundleNo] DEFAULT ('') NOT NULL,
    [OrderID]  VARCHAR (13) CONSTRAINT [DF_Bundle_Detail_Order_History_OrderID] DEFAULT ('') NOT NULL,
    [Qty]      NUMERIC (5)  CONSTRAINT [DF_Bundle_Detail_Order_History_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_Order_History_Order] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

