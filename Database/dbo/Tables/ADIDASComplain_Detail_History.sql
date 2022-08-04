CREATE TABLE [dbo].[ADIDASComplain_Detail_History] (
	[UKEY]           BIGINT         CONSTRAINT [DF_ADIDASComplain_Detail_History_UKEY] DEFAULT ((0)) NOT NULL,
	[Version]        tinyint   CONSTRAINT [DF_ADIDASComplain_Detail_History_Version] DEFAULT (1) NOT NULL,
    [ID]             VARCHAR (13)   CONSTRAINT [DF_ADIDASComplain_Detail_History_ID] DEFAULT ('') NOT NULL,
    [SalesID]        VARCHAR (5)    CONSTRAINT [DF_ADIDASComplain_Detail_History_SalesID] DEFAULT ('') NOT NULL,
    [SalesName]      NVARCHAR (50)  CONSTRAINT [DF_ADIDASComplain_Detail_History_SalesName] DEFAULT ('') NOT NULL,
    [Article]        VARCHAR (8)    CONSTRAINT [DF_ADIDASComplain_Detail_History_Article] DEFAULT ('') NOT NULL,
    [ArticleName]    NVARCHAR (60)  CONSTRAINT [DF_ADIDASComplain_Detail_History_ArticleName] DEFAULT ('') NOT NULL,
    [ProductionDate] DATE           NULL,
    [DefectMainID]   VARCHAR (2)    CONSTRAINT [DF_ADIDASComplain_Detail_History_DefectMainID] DEFAULT ('') NOT NULL,
    [DefectSubID]    VARCHAR (1)    CONSTRAINT [DF_ADIDASComplain_Detail_History_DefectSubID] DEFAULT ('') NOT NULL,
    [FOB]            NUMERIC (5, 2) CONSTRAINT [DF_ADIDASComplain_Detail_History_FOB] DEFAULT ((0)) NOT NULL,
    [Qty]            NUMERIC (3)    CONSTRAINT [DF_ADIDASComplain_Detail_History_Qty] DEFAULT ((0)) NOT NULL,
    [ValueinUSD]     NUMERIC (6, 2) CONSTRAINT [DF_ADIDASComplain_Detail_History_ValueinUSD] DEFAULT ((0)) NOT NULL,
    [ValueINExRate]  NUMERIC (7, 2) CONSTRAINT [DF_ADIDASComplain_Detail_History_ValueINExRate] DEFAULT ((0)) NOT NULL,
    [OrderID]        VARCHAR (13)   CONSTRAINT [DF_ADIDASComplain_Detail_History_OrderID] DEFAULT ('') NOT NULL,
    [RuleNo]         NUMERIC (1)    CONSTRAINT [DF_ADIDASComplain_Detail_History_RuleNo] DEFAULT ((0)) NOT NULL,
    [BrandID]        VARCHAR (8)   NOT NULL DEFAULT (''),
    [FactoryID]      VARCHAR (8)   NOT NULL DEFAULT (''),
    [StyleID]        VARCHAR (20)  NOT NULL DEFAULT (''),
    [SuppID]       VARCHAR (6)   NOT NULL DEFAULT (''),
    [Refno]          VARCHAR (36)  NOT NULL DEFAULT (''),
    [CustPONo]       VARCHAR (30)  NOT NULL DEFAULT (''),
    [SeasonId]       VARCHAR (10)  NOT NULL DEFAULT (''),
    [BulkMR]         VARCHAR (10)  NOT NULL DEFAULT (''),
    [SampleMR]       VARCHAR (10)  NOT NULL DEFAULT (''),
    [IsEM] BIT CONSTRAINT [DF_ADIDASComplain_Detail_History_IsEM] DEFAULT (0) NOT NULL, 
    [Responsibility] VARCHAR(2) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_ADIDASComplain_Detail_History] PRIMARY KEY CLUSTERED ([UKEY],[Version] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ADIDAS Complain 明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'SalesID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sales Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'SalesName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Article', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ArticleName', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'ArticleName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'ProductionDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DefectMain ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'DefectMainID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'DefectSub ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'DefectSubID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FOB', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'FOB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Conplaint Value in USD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'ValueinUSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Complaint Value in ex.Rate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'ValueINExRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'OrderID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Rule No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain_Detail_History', @level2type = N'COLUMN', @level2name = N'RuleNo';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'責任歸屬',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ADIDASComplain_Detail_History',
    @level2type = N'COLUMN',
    @level2name = N'Responsibility'