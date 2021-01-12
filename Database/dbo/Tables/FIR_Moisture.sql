CREATE TABLE [dbo].[FIR_Moisture] (
    [ID]          BIGINT         NOT NULL,
    [Roll]        VARCHAR (8)    NOT NULL,
    [Dyelot]      VARCHAR (8)    NOT NULL,
    [ActMoisture] NUMERIC (4, 1) NULL,
    [InspDate]    DATE           NULL,
    [Inspector]   VARCHAR (10)   CONSTRAINT [DF_FIR_Moisture_Inspector] DEFAULT ('') NOT NULL,
    [Result]      VARCHAR (5)    CONSTRAINT [DF_FIR_Moisture_Result] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_FIR_Moisture_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_FIR_Moisture_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_FIR_Moisture] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統根據實際檢驗的數值判斷檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'InspDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度實際檢測的數值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'ActMoisture';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'染缸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FIR Pkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕度檢測', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Moisture';

