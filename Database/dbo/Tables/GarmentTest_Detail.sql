CREATE TABLE [dbo].[GarmentTest_Detail] (
    [ID]          BIGINT         CONSTRAINT [DF_GarmentTest_Detail_ID] DEFAULT ((0)) NOT NULL,
    [No]          INT            CONSTRAINT [DF_GarmentTest_Detail_No] DEFAULT ('') NOT NULL,
    [Result]      VARCHAR (1)    CONSTRAINT [DF_GarmentTest_Detail_Result] DEFAULT ('') NULL,
    [inspdate]    DATE           NULL,
    [inspector]   VARCHAR (10)   CONSTRAINT [DF_GarmentTest_Detail_inspector] DEFAULT ('') NULL,
    [Remark]      NVARCHAR (MAX) CONSTRAINT [DF_GarmentTest_Detail_Remark] DEFAULT ('') NULL,
    [Sender]      VARCHAR (10)   CONSTRAINT [DF_GarmentTest_Detail_Sender] DEFAULT ('') NULL,
    [SendDate]    DATETIME       NULL,
    [Receiver]    VARCHAR (10)   CONSTRAINT [DF_GarmentTest_Detail_Receiver] DEFAULT ('') NULL,
    [ReceiveDate] DATETIME       NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_GarmentTest_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_GarmentTest_Detail_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [OldUkey]     VARCHAR (10)   CONSTRAINT [DF__tmp_ms_xx__OldUk__560B68B9] DEFAULT ('') NULL,
    [LOtoFactory] VARCHAR(100) NOT NULL  CONSTRAINT DF_GarmentTest_Detail_LOtoFactory DEFAULT('') , 
    [FGWTMtlTypeID] VARCHAR(20) NOT NULL  CONSTRAINT DF_GarmentTest_Detail_FGWTMtlTypeID DEFAULT('') , 
    [Above50NaturalFibres] BIT NOT NULL CONSTRAINT [DF_GarmentTest_Detail_Above50NaturalFibres] DEFAULT 0, 
    [Above50SyntheticFibres] BIT NOT NULL CONSTRAINT [DF_GarmentTest_Detail_Above50SyntheticFibres] DEFAULT 0, 
    CONSTRAINT [PK_GarmentTest_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'No';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'inspdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄出人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Sender';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄出時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'SendDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'Receiver';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';

GO

	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'All basic Fabrics ≥ 50% natural fibres ; 布料 50% (含) 以上是天然纖維', @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'GarmentTest_Detail'
	, @level2type = N'COLUMN', @level2name = N'Above50NaturalFibres';
	;
GO
	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'All basic Fabrics ≥ 50% synthetic fibres (ex. polyester) ; 布料 50% (含)以上是合成纖維 (e.x. 聚酯纖維)', @level0type = N'SCHEMA', @level0name = N'dbo'
	, @level1type = N'TABLE', @level1name = N'GarmentTest_Detail'
	, @level2type = N'COLUMN', @level2name = N'Above50SyntheticFibres';
	;
GO