CREATE TABLE [dbo].[GarmentTest] (
    [ID]            BIGINT   NOT NULL IDENTITY,
    [FirstOrderID]  VARCHAR (13)  CONSTRAINT [DF_GarmentTest_FirstOrderID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13)  CONSTRAINT [DF_GarmentTest_OrderID] DEFAULT ('') NULL,
    [StyleID]       VARCHAR (15)  CONSTRAINT [DF_GarmentTest_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]      VARCHAR (8)   CONSTRAINT [DF_GarmentTest_SeasonID] DEFAULT ('') NOT NULL,
    [BrandID]       VARCHAR (8)   CONSTRAINT [DF_GarmentTest_BrandID] DEFAULT ('') NOT NULL,
    [Article]       VARCHAR (8)   CONSTRAINT [DF_GarmentTest_Article] DEFAULT ('') NOT NULL,
    [MDivisionid]     VARCHAR (8)   CONSTRAINT [DF_GarmentTest_FactoryID] DEFAULT ('') NOT NULL,
    [DeadLine]      DATE          NULL,
    [SewingInline]  DATE          NULL,
    [SewingOffline] DATE          NULL,
    [Date]          DATE          NULL,
    [Result]        VARCHAR (1)   CONSTRAINT [DF_GarmentTest_Result] DEFAULT ('') NULL,
    [Remark]        NVARCHAR (MAX) CONSTRAINT [DF_GarmentTest_Remark] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_GarmentTest_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_GarmentTest_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [OldUkey] VARCHAR(10) NULL DEFAULT (''), 
    SeamBreakageResult varchar(1)  CONSTRAINT [DF_GarmentTest_SeamBreakageResult] default('') NOT NULL,
    SeamBreakageLastTestDate Date NULL,
    OdourResult varchar(1)  CONSTRAINT [DF_GarmentTest_OdourResult] default('') NOT NULL,
    WashResult varchar(1)  CONSTRAINT [DF_GarmentTest_WashResult] default('') NOT NULL,
    CONSTRAINT [PK_GarmentTest] PRIMARY KEY CLUSTERED ([MDivisionid], [BrandID], [StyleID], [SeasonID], [Article])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'首次訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'FirstOrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款示', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = 'MDivisionid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試截止日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'DeadLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing 上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'SewingInline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing 下線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'SewingOffline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後測試日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'Date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest', @level2type = N'COLUMN', @level2name = N'EditDate';
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
	@value = N'Adidas-PHX-AP0450 SeamBrakage 最後一次檢驗的結果',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'GarmentTest',
	@level2type = N'COLUMN',
	@level2name = N'SeamBreakageResult'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
	@value = N'Adidas-PHX-AP0450 SeamBrakage 最後一次檢驗的日期',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'GarmentTest',
	@level2type = N'COLUMN',
	@level2name = N'SeamBreakageLastTestDate'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
	@value = N'Adidas-PHX-AP0451 最後一次檢驗的結果',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'GarmentTest',
	@level2type = N'COLUMN',
	@level2name = N'OdourResult'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
	@value = N'Adidas-PHX-AP0701 / PHX-AP0710 最後一次檢驗的結果',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'GarmentTest',
	@level2type = N'COLUMN',
	@level2name = N'WashResult'
GO