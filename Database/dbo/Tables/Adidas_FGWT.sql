CREATE TABLE [dbo].[Adidas_FGWT]
(
	[Seq] int NOT NULL CONSTRAINT [DF_Adidas_FGWT_Seq] DEFAULT(0), 
    [TestName] VARCHAR(15) NOT NULL CONSTRAINT [DF_Adidas_FGWT_TestName] DEFAULT(''), 
    [Location] VARCHAR NOT NULL CONSTRAINT [DF_Adidas_FGWT_Location] DEFAULT(''), 
    [SystemType] VARCHAR(150) NOT NULL CONSTRAINT [DF_Adidas_FGWT_SystemType] DEFAULT(''), 
    [ReportType] VARCHAR(150) NOT NULL CONSTRAINT [DF_Adidas_FGWT_ReportType] DEFAULT(''), 
    [MtlTypeID] VARCHAR(20) NOT NULL CONSTRAINT [DF_Adidas_FGWT_MtlTypeID] DEFAULT(''), 
    [Washing] VARCHAR(20) NOT NULL CONSTRAINT [DF_Adidas_FGWT_Washing] DEFAULT(''), 
    [FabricComposition] VARCHAR(30) NOT NULL CONSTRAINT [DF_Adidas_FGWT_FabricComposition] DEFAULT(''), 
    [TestDetail] VARCHAR(30) NOT NULL CONSTRAINT [DF_Adidas_FGWT_estDetail] DEFAULT(''), 
    [Scale] VARCHAR(5) NULL, 
    [Criteria] NUMERIC(11, 2) NULL, 
    [Criteria2] NUMERIC(11, 2) NULL,
    CONSTRAINT [PK_Adidas_FGWT] PRIMARY KEY CLUSTERED ( Location, ReportType, MtlTypeID, Washing, FabricComposition)
)
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用於確認 FGWT 各測試項目的排序、呈現名稱（分成 System 與 Report）以及各項檢驗的標準
※ 請參考附件 (ISP20201727) FGWT.xlsx 建立基本資料
建立時請注意第 79 項會依照不同部位有不同標準
因此寫入資料時 Location 不再是空白
而是相對應的 Location (T, B, S)', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序順序', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'Seq';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試名稱', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'TestName';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位 (T [Top], B [Bottom], S [Top+Bottom], '' [全部])', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'Location';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試項目 (系統呈現)', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'SystemType';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試項目 (報表呈現)', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'ReportType';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種 (KNIT, WOVEN)', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'MtlTypeID';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'(LineDry, TumbleDry, HandWash)', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'Washing';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成分 (Above50NaturaFibres, Above50SyntheticFibres)', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'FabricComposition';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pass 判斷方法 (級距 = [grade, pass/fail] ; 數值 = [cm, %, Range%])', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'TestDetail';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Null (代表用數值判斷)、不為 Null (代表用分數級距判斷)', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'Scale';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數值判斷 Pass 區間的最小值', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'Criteria';
go

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數值判斷 Pass 區間的最大值', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'Adidas_FGWT'
, @level2type = N'COLUMN', @level2name = N'Criteria2';
go