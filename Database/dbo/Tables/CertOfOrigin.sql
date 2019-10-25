CREATE TABLE [dbo].[CertOfOrigin] (
    [SuppID]      VARCHAR (6)    CONSTRAINT [DF_CertOfOrigin_SuppID] DEFAULT ('') NOT NULL,
    [FormXPayINV] NVARCHAR (300) CONSTRAINT [DF_CertOfOrigin_FormXPayINV] DEFAULT ('') NOT NULL,
    [COName]      VARCHAR (15)   CONSTRAINT [DF_CertOfOrigin_COName] DEFAULT ('') NOT NULL,
    [ReceiveDate] DATE           NOT NULL,
    [SendDate]    DATE           NULL,
    [Carrier]     VARCHAR (30)   CONSTRAINT [DF_CertOfOrigin_Carrier] DEFAULT ('') NOT NULL,
    [AWBNo]       VARCHAR (20)   CONSTRAINT [DF_CertOfOrigin_AWBNo] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_CertOfOrigin_AddName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_CertOfOrigin_EditName] DEFAULT ('') NULL,
    CONSTRAINT [PK_CertOfOrigin] PRIMARY KEY CLUSTERED ([SuppID] ASC, [FormXPayINV] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'航空運單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'AWBNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨運公司(廠商付款，故無基本檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'Carrier';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'送出文件日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'SendDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收到文件日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'專案類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'COName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSP Record的payment invoice#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'FormXPayINV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CertOfOrigin', @level2type = N'COLUMN', @level2name = N'SuppID';

