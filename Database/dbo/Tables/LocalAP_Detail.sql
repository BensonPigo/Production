CREATE TABLE [dbo].[LocalAP_Detail] (
    [Id]                 VARCHAR (13)    CONSTRAINT [DF_LocalAP_Detail_Id] DEFAULT ('') NOT NULL,
    [OrderId]            VARCHAR (13)    CONSTRAINT [DF_LocalAP_Detail_OrderId] DEFAULT ('') NOT NULL,
    [Refno]              VARCHAR (21)    CONSTRAINT [DF_LocalAP_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID]      VARCHAR (15)    CONSTRAINT [DF_LocalAP_Detail_ThreadColorID] DEFAULT ('') NULL,
    [Price]              NUMERIC (16, 4) CONSTRAINT [DF_LocalAP_Detail_Price] DEFAULT ((0)) NOT NULL,
    [Qty]                NUMERIC (8, 2)  CONSTRAINT [DF_LocalAP_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [UnitID]             VARCHAR (8)     CONSTRAINT [DF_LocalAP_Detail_UnitID] DEFAULT ('') NOT NULL,
    [LocalPoId]          VARCHAR (13)    CONSTRAINT [DF_LocalAP_Detail_LocalPoId] DEFAULT ('') NOT NULL,
    [LocalPo_DetailUkey] BIGINT          CONSTRAINT [DF_LocalAP_Detail_LocalPo_DetailUkey] DEFAULT ((0)) NOT NULL,
    [Ukey]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [OldSeq1]            VARCHAR (3)     NULL,
    [OldSeq2]            VARCHAR (2)     NULL,
    CONSTRAINT [PK_LocalAP_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local AP Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購付款單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'LocalPoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalAP_Detail', @level2type = N'COLUMN', @level2name = N'LocalPo_DetailUkey';

