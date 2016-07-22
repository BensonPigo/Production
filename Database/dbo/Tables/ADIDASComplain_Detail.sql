CREATE TABLE [dbo].[ADIDASComplain_Detail] (
    [ID]             VARCHAR (13)   CONSTRAINT [DF_ADIDASComplain_Detail_ID] DEFAULT ('') NOT NULL,
    [SalesID]        VARCHAR (5)    CONSTRAINT [DF_ADIDASComplain_Detail_SalesID] DEFAULT ('') NULL,
    [SalesName]      NVARCHAR (50)  CONSTRAINT [DF_ADIDASComplain_Detail_SalesName] DEFAULT ('') NULL,
    [Article]        VARCHAR (8)    CONSTRAINT [DF_ADIDASComplain_Detail_Article] DEFAULT ('') NULL,
    [ArticleName]    NVARCHAR (60)  CONSTRAINT [DF_ADIDASComplain_Detail_ArticleName] DEFAULT ('') NULL,
    [ProductionDate] DATE           NULL,
    [DefectMainID]   VARCHAR (2)    CONSTRAINT [DF_ADIDASComplain_Detail_DefectMainID] DEFAULT ('') NULL,
    [DefectSubID]    VARCHAR (1)    CONSTRAINT [DF_ADIDASComplain_Detail_DefectSubID] DEFAULT ('') NULL,
    [FOB]            NUMERIC (5, 2) CONSTRAINT [DF_ADIDASComplain_Detail_FOB] DEFAULT ((0)) NULL,
    [Qty]            NUMERIC (3)    CONSTRAINT [DF_ADIDASComplain_Detail_Qty] DEFAULT ((0)) NULL,
    [ValueinUSD]     NUMERIC (6, 2) CONSTRAINT [DF_ADIDASComplain_Detail_ValueinUSD] DEFAULT ((0)) NULL,
    [ValueINExRate]  NUMERIC (7, 2) CONSTRAINT [DF_ADIDASComplain_Detail_ValueINExRate] DEFAULT ((0)) NULL,
    [OrderID]        VARCHAR (13)   CONSTRAINT [DF_ADIDASComplain_Detail_OrderID] DEFAULT ('') NULL,
    [RuleNo]         NUMERIC (1)    CONSTRAINT [DF_ADIDASComplain_Detail_RuleNo] DEFAULT ((0)) NULL,
    [UKEY]           BIGINT         CONSTRAINT [DF_ADIDASComplain_Detail_UKEY] DEFAULT ((0)) NOT NULL,
    [BrandID]        VARCHAR (8)    NULL,
    [FactoryID]      VARCHAR (8)    NULL,
    [StyleID]        VARCHAR (20)   NULL,
    [Supplier]       VARCHAR (6)    NULL,
    [Refno]          VARCHAR (20)   NULL,
    [CustPONo]       VARCHAR (30)   NULL,
    [SeasonId]       VARCHAR (10)   NULL,
    [BulkMR]         VARCHAR (10)   NULL,
    [SampleMR]       VARCHAR (10)   NULL,
    CONSTRAINT [PK_ADIDASComplain_Detail] PRIMARY KEY CLUSTERED ([UKEY] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ADIDAS Complain 明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'SalesID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'SalesName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ArticleName', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'ArticleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'ProductionDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DefectMain ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'DefectMainID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DefectSub ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'DefectSubID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FOB', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'FOB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Conplaint Value in USD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'ValueinUSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Complaint Value in ex.Rate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'ValueINExRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OrderID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Rule No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail', @level2type = N'COLUMN', @level2name = N'RuleNo';

