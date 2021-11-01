CREATE TABLE [dbo].[Adidas_FGWT] (
    [Seq]               INT             CONSTRAINT [DF_Adidas_FGWT_Seq] DEFAULT ((0)) NOT NULL,
    [TestName]          VARCHAR (15)    CONSTRAINT [DF_Adidas_FGWT_TestName] DEFAULT ('') NOT NULL,
    [Location]          VARCHAR (1)     CONSTRAINT [DF_Adidas_FGWT_Location] DEFAULT ('') NOT NULL,
    [SystemType]        VARCHAR (150)   CONSTRAINT [DF_Adidas_FGWT_SystemType] DEFAULT ('') NOT NULL,
    [ReportType]        VARCHAR (150)   CONSTRAINT [DF_Adidas_FGWT_ReportType] DEFAULT ('') NOT NULL,
    [MtlTypeID]         VARCHAR (20)    CONSTRAINT [DF_Adidas_FGWT_MtlTypeID] DEFAULT ('') NOT NULL,
    [Washing]           VARCHAR (20)    CONSTRAINT [DF_Adidas_FGWT_Washing] DEFAULT ('') NOT NULL,
    [FabricComposition] VARCHAR (30)    CONSTRAINT [DF_Adidas_FGWT_FabricComposition] DEFAULT ('') NOT NULL,
    [TestDetail]        VARCHAR (30)    CONSTRAINT [DF_Adidas_FGWT_estDetail] DEFAULT ('') NOT NULL,
    [Scale]             VARCHAR (5)     NULL,
    [Criteria]          NUMERIC (11, 2) NULL,
    [Criteria2]         NUMERIC (11, 2) NULL,
    CONSTRAINT [PK_Adidas_FGWT] PRIMARY KEY CLUSTERED ([Location] ASC, [ReportType] ASC, [MtlTypeID] ASC, [Washing] ASC, [FabricComposition] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數值判斷 Pass 區間的最大值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'Criteria2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數值判斷 Pass 區間的最小值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'Criteria';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Null (代表用數值判斷)、不為 Null (代表用分數級距判斷)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'Scale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pass 判斷方法 (級距 = [grade, pass/fail] ; 數值 = [cm, %, Range%])', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'TestDetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成分 (Above50NaturaFibres, Above50SyntheticFibres)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'FabricComposition';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'(LineDry, TumbleDry, HandWash)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'Washing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種 (KNIT, WOVEN)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'MtlTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試項目 (報表呈現)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'ReportType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試項目 (系統呈現)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'SystemType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位 (T [Top], B [Bottom], S [Top+Bottom], '''' [全部])', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試名稱
PHX-AP0701 須特別注意
要根據建立款式的 Special Mark 改為 PHX-AP0710

只有以下 15 種 Special Mark 屬於 710
MATCH TEAMWEAR
BASEBALL ON FIELD
SOFTBALL ON FIELD
TRAINING TEAMWEAR
LACROSSE ONFIELD
AMERIC. FOOT. ON-FIELD
TIRO
BASEBALL OFF FIELD
NCAA ON ICE
ON-COURT
BBALL PERFORMANCE
BRANDED BLANKS
SLD ON-FIELD
NHL ON ICE
SLD ON-COURT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'TestName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Adidas_FGWT';

