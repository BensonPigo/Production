CREATE TABLE [dbo].[BundleReplacement] (
    [ID]              VARCHAR (13)  DEFAULT ('') NOT NULL,
    [POID]            VARCHAR (13)  DEFAULT ('') NOT NULL,
    [MDivisionid]     VARCHAR (8)   DEFAULT ('') NOT NULL,
    [Sizecode]        VARCHAR (100) DEFAULT ('') NOT NULL,
    [Colorid]         VARCHAR (6)   DEFAULT ('') NOT NULL,
    [Article]         VARCHAR (8)   DEFAULT ('') NOT NULL,
    [PatternPanel]    VARCHAR (2)   DEFAULT ('') NOT NULL,
    [Cutno]           NUMERIC (6)   NULL,
    [Cdate]           DATE          NULL,
    [Orderid]         VARCHAR (13)  DEFAULT ('') NOT NULL,
    [Sewinglineid]    VARCHAR (2)   DEFAULT ('') NOT NULL,
    [Item]            VARCHAR (20)  NULL,
    [SewingCell]      VARCHAR (2)   DEFAULT ('') NOT NULL,
    [Ratio]           VARCHAR (100) NULL,
    [Startno]         NUMERIC (5)   NULL,
    [Qty]             NUMERIC (2)   NULL,
    [PrintDate]       DATETIME      NULL,
    [AllPart]         NUMERIC (5)   NULL,
    [OriginBundleNo]  VARCHAR (10)  NULL,
    [AddName]         VARCHAR (10)  NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  NULL,
    [EditDate]        DATETIME      NULL,
    [oldid]           VARCHAR (13)  NULL,
    [FabricPanelCode] VARCHAR (2)   NULL,
    [Remake]          BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BundleReplacement] PRIMARY KEY CLUSTERED ([ID] ASC)
);



