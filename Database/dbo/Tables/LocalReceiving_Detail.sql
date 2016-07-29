CREATE TABLE [dbo].[LocalReceiving_Detail] (
    [Id]                 VARCHAR (13)   NOT NULL,
    [OrderId]            VARCHAR (13)   NOT NULL,
    [Refno]              VARCHAR (20)   NOT NULL,
    [ThreadColorID]      VARCHAR (15)   NULL,
    [Qty]                NUMERIC (8, 2) NOT NULL,
    [LocalPoId]          VARCHAR (13)   NOT NULL,
    [Remark]             NVARCHAR (100) NULL,
    [LocalPo_detailukey] BIGINT         NOT NULL,
    [Location]           VARCHAR (60)   NULL,
    [Category]           VARCHAR (20)   NULL,
    [Mdivisionid]        VARCHAR (8)    NULL,
    [OldSeq1]            VARCHAR (3)    NULL,
    [OldSeq2]            VARCHAR (2)    NULL,
    [Ukey]               BIGINT         IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_LocalReceiving_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Receiving Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'LocalPoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalReceiving_Detail', @level2type = N'COLUMN', @level2name = N'LocalPo_detailukey';

