CREATE TABLE [dbo].[Bundle_Detail_History] (
    [BundleNo]    VARCHAR (10)   CONSTRAINT [DF_Bundle_Detail_History_BundleNo] DEFAULT ('') NOT NULL,
    [Id]          VARCHAR (13)   CONSTRAINT [DF_Bundle_Detail_History_Id] DEFAULT ((0)) NOT NULL,
    [BundleGroup] NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_History_BundleGroup] DEFAULT ((0)) NULL,
    [Patterncode] VARCHAR (20)   CONSTRAINT [DF_Bundle_Detail_History_Patterncode] DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (100) CONSTRAINT [DF_Bundle_Detail_History_PatternDesc] DEFAULT ('') NOT NULL,
    [SizeCode]    VARCHAR (8)    CONSTRAINT [DF_Bundle_Detail_History_SizeCode] DEFAULT ('') NULL,
    [Qty]         NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_History_Qty] DEFAULT ((0)) NULL,
    [Parts]       NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_History_Parts] DEFAULT ((0)) NULL,
    [Farmin]      NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_History_Farmin] DEFAULT ((0)) NULL,
    [FarmOut]     NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_History_FarmOut] DEFAULT ((0)) NULL,
    [PrintDate]   DATETIME       NULL,
    [IsPair]      BIT            NULL,
    [Location]    VARCHAR (1)    DEFAULT ('') NOT NULL,
    [RFUID]       VARCHAR (10)   CONSTRAINT [DF_Bundle_Detail_History_RFUID] DEFAULT ('') NULL,
    [Tone]        VARCHAR (15)   CONSTRAINT [DF_Bundle_Detail_History_Tone] DEFAULT ('') NOT NULL,
    [RFPrintDate] DATETIME       NULL,
    [PrintGroup]  TINYINT        NULL,
    [RFIDScan]    BIT            CONSTRAINT [DF_Bundle_Detail_History_RFIDScan] DEFAULT ((0)) NOT NULL,
    [Dyelot]      VARCHAR (50)   CONSTRAINT [DF_Bundle_Detail_History_Dyelot] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_History] PRIMARY KEY CLUSTERED ([BundleNo] ASC, [Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle 資訊印在 RFID card 上的時候, 是否要加上 RFID 的mark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'RFIDScan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'列印Bundle Card時的群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'PrintGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'RF Print Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'RFPrintDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'for SNP RF Card UID, printer(CHP_1800)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'RFUID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發發出數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'FarmOut';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發收入數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'Farmin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Part 數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'Parts';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'Patterncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'BundleGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捆包號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle Detail History', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_History';

