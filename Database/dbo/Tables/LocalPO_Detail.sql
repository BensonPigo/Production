CREATE TABLE [dbo].[LocalPO_Detail] (
    [Id]            VARCHAR (13)    CONSTRAINT [DF_LocalPO_Detail_Id] DEFAULT ('') NOT NULL,
    [OrderId]       VARCHAR (13)    CONSTRAINT [DF_LocalPO_Detail_OrderId] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (21)    CONSTRAINT [DF_LocalPO_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID] VARCHAR (15)    CONSTRAINT [DF_LocalPO_Detail_ThreadColorID] DEFAULT ('') NULL,
    [Price]         NUMERIC (12, 4) CONSTRAINT [DF_LocalPO_Detail_Price] DEFAULT ((0)) NULL,
    [Qty]           NUMERIC (8, 2)  CONSTRAINT [DF_LocalPO_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [UnitId]        VARCHAR (8)     CONSTRAINT [DF_LocalPO_Detail_UnitId] DEFAULT ('') NOT NULL,
    [InQty]         NUMERIC (8, 2)  CONSTRAINT [DF_LocalPO_Detail_InQty] DEFAULT ((0)) NULL,
    [APQty]         NUMERIC (8, 2)  CONSTRAINT [DF_LocalPO_Detail_APQty] DEFAULT ((0)) NULL,
    [Ukey]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [RequestID]     VARCHAR (13)    CONSTRAINT [DF_LocalPO_Detail_RequestID] DEFAULT ('') NULL,
    [Delivery]      DATE            NULL,
    [OldSeq1]       VARCHAR (3)     CONSTRAINT [DF_LocalPO_Detail_OldSeq1] DEFAULT ('') NULL,
    [OldSeq2]       VARCHAR (2)     CONSTRAINT [DF_LocalPO_Detail_OldSeq2] DEFAULT ('') NULL,
    [Remark]        VARCHAR (MAX)   NULL,
    [POID] VARCHAR(13) NULL, 
    [BuyerID] VARCHAR(8) CONSTRAINT [DF_LocalPO_Detail_BuyerID] DEFAULT ('') NULL,
    [ReasonID] VARCHAR(10) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_LocalPO_Detail] PRIMARY KEY ([Ukey]) 
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Purchase Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'UnitId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'InQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'APQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'匯入需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'RequestID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到貨日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'Delivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OldSeq1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'OldSeq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OldSeq2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalPO_Detail', @level2type = N'COLUMN', @level2name = N'OldSeq2';

GO
CREATE NONCLUSTERED INDEX [IDX_LocalPO_Detail_OrderIdRefno] ON [dbo].[LocalPO_Detail]
(
	[OrderId] ASC,
	[Refno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

