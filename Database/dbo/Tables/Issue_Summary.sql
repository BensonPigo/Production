CREATE TABLE [dbo].[Issue_Summary] (
    [Id]              VARCHAR (13)    CONSTRAINT [DF_Issue_Summary_Id] DEFAULT ('') NOT NULL,
    [Ukey]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [Poid]            VARCHAR (13)    CONSTRAINT [DF_Issue_Summary_Poid] DEFAULT ('') NOT NULL,
    [SCIRefno]        VARCHAR (30)    CONSTRAINT [DF_Issue_Summary_SCIRefno] DEFAULT ('') NOT NULL,
    [Colorid]         VARCHAR (6)     CONSTRAINT [DF_Issue_Summary_Colorid] DEFAULT ('') NOT NULL,
    [SizeSpec]        VARCHAR (15)    CONSTRAINT [DF_Issue_Summary_SizeSpec] DEFAULT ('') NOT NULL,
    [BomFactory]      VARCHAR (10)    CONSTRAINT [DF_Issue_Summary_BomFactory] DEFAULT ('') NOT NULL,
    [BomCountry]      VARCHAR (2)     CONSTRAINT [DF_Issue_Summary_BomCountry] DEFAULT ('') NOT NULL,
    [BomStyle]        VARCHAR (15)    CONSTRAINT [DF_Issue_Summary_BomStyle] DEFAULT ('') NOT NULL,
    [BomCustCD]       VARCHAR (20)    CONSTRAINT [DF_Issue_Summary_BomCustCD] DEFAULT ('') NOT NULL,
    [BomArticle]      VARCHAR (8)     CONSTRAINT [DF_Issue_Summary_BomArticle] DEFAULT ('') NOT NULL,
    [BomZipperInsert] VARCHAR (5)     CONSTRAINT [DF_Issue_Summary_BomZipperInsert] DEFAULT ('') NOT NULL,
    [BomBuymonth]     VARCHAR (10)    CONSTRAINT [DF_Issue_Summary_BomBuymonth] DEFAULT ('') NOT NULL,
    [BomCustPONo]     VARCHAR (30)    CONSTRAINT [DF_Issue_Summary_BomCustPONo] DEFAULT ('') NOT NULL,
    [Qty]             NUMERIC (10, 2) CONSTRAINT [DF_Issue_Summary_Qty] DEFAULT ((0)) NULL,
    [seq1]            VARCHAR (3)     NULL,
    [seq2]            VARCHAR (2)     NULL,
    [OldFabricUkey] VARCHAR(10) NULL DEFAULT (''), 
    [OldFabricVer] VARCHAR(2) NULL DEFAULT (''), 
    [SuppColor] NVARCHAR(MAX) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_Issue_Summary_1] PRIMARY KEY CLUSTERED ([Id] ASC, [Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Issue Summary', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'Poid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomStyle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依Bom Type展開的CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomCustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomArticle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉鋉左右拉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomZipperInsert';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂購月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomBuymonth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Summary', @level2type = N'COLUMN', @level2name = N'BomCustPONo';


GO

CREATE INDEX [Poid_SCIRefo_Color] ON [dbo].[Issue_Summary] ([Poid],[SCIRefno],[Colorid])
