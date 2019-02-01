CREATE TABLE [dbo].[FIR_Weight] (
    [ID]              BIGINT         CONSTRAINT [DF_FIR_Weight_ID] DEFAULT ((0)) NOT NULL,
    [Roll]            VARCHAR (8)    CONSTRAINT [DF_FIR_Weight_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]          VARCHAR (8)    CONSTRAINT [DF_FIR_Weight_Dyelot] DEFAULT ('') NOT NULL,
    [WeightM2]        NUMERIC (4, 1) CONSTRAINT [DF_FIR_Weight_WeightM2] DEFAULT ((0)) NOT NULL,
    [AverageWeightM2] NUMERIC (4, 1) CONSTRAINT [DF_FIR_Weight_AverageWeightM2] DEFAULT ((0)) NOT NULL,
    [Difference]      NUMERIC (5, 2) CONSTRAINT [DF_FIR_Weight_Difference] DEFAULT ((0)) NOT NULL,
    [Inspdate]        DATE           NULL,
    [Inspector]       VARCHAR (10)   CONSTRAINT [DF_FIR_Weight_Inspector] DEFAULT ('') NULL,
    [Result]          VARCHAR (5)    CONSTRAINT [DF_FIR_Weight_Result] DEFAULT ('') NULL,
    [Remark]          NVARCHAR (100) CONSTRAINT [DF_FIR_Weight_Remark] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_FIR_Weight_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_FIR_Weight_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    [SubmitDate]      DATE           NULL,
    CONSTRAINT [PK_FIR_Weight] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Inspection - Weight Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'登記每平方尺克重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'WeightM2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均每平方尺克重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'AverageWeightM2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'差異值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'Difference';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'Inspdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Weight', @level2type = N'COLUMN', @level2name = N'EditDate';

