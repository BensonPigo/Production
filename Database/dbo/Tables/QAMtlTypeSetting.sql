CREATE TABLE [dbo].[QAMtlTypeSetting] (
    [ID]                   VARCHAR (20)   CONSTRAINT [DF_QAMtlTypeSetting_ID] DEFAULT ('') NOT NULL,
    [FullName]             NVARCHAR (100) CONSTRAINT [DF_QAMtlTypeSetting_FullName] DEFAULT ('') NULL,
    [Type]                 VARCHAR (1)    CONSTRAINT [DF_QAMtlTypeSetting_Type] DEFAULT ('') NULL,
    [Junk]                 BIT            CONSTRAINT [DF_QAMtlTypeSetting_Junk] DEFAULT ((0)) NOT NULL,
    [AOS_InspQtyOption]    TINYINT        CONSTRAINT [DF_QAMtlTypeSetting_AOS_InspQty] DEFAULT ((0)) NOT NULL,
    [InspectedPercentage]  INT            CONSTRAINT [DF_QAMtlTypeSetting_InspectedPercentage] DEFAULT ((0)) NOT NULL,
    [AQL_InspectionLevels] VARCHAR (2)    CONSTRAINT [DF_QAMtlTypeSetting_AQL_InspectionLevels] DEFAULT ((1)) NOT NULL,
    [AddName]              VARCHAR (10)   CONSTRAINT [DF_QAMtlTypeSetting_AddName] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME       NULL,
    [EditName]             VARCHAR (10)   CONSTRAINT [DF_QAMtlTypeSetting_EditName] DEFAULT ('') NOT NULL,
    [EditDate]             DATETIME       NULL,
    CONSTRAINT [PK_QAMtlTypeSetting] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AQL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'AQL_InspectionLevels';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'抽驗百分比', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'InspectedPercentage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'輔料檢驗抽樣規則', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'AOS_InspQtyOption';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'FullName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主副料類別設定基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'QAMtlTypeSetting';

