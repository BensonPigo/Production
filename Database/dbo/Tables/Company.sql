CREATE TABLE [dbo].[Company] (
    [ID]        NUMERIC(2)        CONSTRAINT [DF_Company_ID] DEFAULT ((0)) NOT NULL,
    [Title]     NVARCHAR (40)  CONSTRAINT [DF_Company_Title] DEFAULT ('') NOT NULL,
    [Abbr]      VARCHAR (2)    CONSTRAINT [DF_Company_Abbr] DEFAULT ('') NOT NULL,
    [Country]   VARCHAR (2)    CONSTRAINT [DF_Company_Country] DEFAULT ('') NOT NULL,
    [Junk]      BIT            CONSTRAINT [DF_Company_Junk] DEFAULT ((0)) NOT NULL,
    [Currency]  VARCHAR (3)    CONSTRAINT [DF_Company_Currency] DEFAULT ('') NOT NULL,
    [NameCH]    NVARCHAR (40)  CONSTRAINT [DF_Company_NameCH] DEFAULT ('') NOT NULL,
    [NameEN]    NVARCHAR (80)  CONSTRAINT [DF_Company_NameEN] DEFAULT ('') NOT NULL,
    [hasTax]    BIT            CONSTRAINT [DF_Company_hasTax] DEFAULT ((0)) NOT NULL,
    [IsDefault] BIT            CONSTRAINT [DF_Company_IsDefault] DEFAULT ((0)) NOT NULL,
    [VatNO]     VARCHAR (25)   CONSTRAINT [DF_Company_VatNO] DEFAULT ('') NOT NULL,
    [AddressCH] NVARCHAR (MAX) CONSTRAINT [DF_Company_AddressCH] DEFAULT ('') NOT NULL,
    [AddressEN] NVARCHAR (MAX) CONSTRAINT [DF_Company_AddressEN] DEFAULT ('') NOT NULL,
    [Tel]       VARCHAR (30)   CONSTRAINT [DF_Company_Tel] DEFAULT ('') NOT NULL,
    [Fax]       VARCHAR (30)   CONSTRAINT [DF_Company_Fax] DEFAULT ('') NOT NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Company_AddName] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Company_EditName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME       NULL,
    [IsOrderCompany] BIT CONSTRAINT [DF_Company_IsOrderCompany] DEFAULT ((0)) NOT NULL, 
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司資訊基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司抬頭 (對外顯示值)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Title';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司簡碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Abbr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所在國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Country';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cancel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Currency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司別名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文公司名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否有營業稅', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'hasTax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為預設值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'IsDefault';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'統一編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'VatNO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'AddressCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'AddressEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳真', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'Fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'EditName';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為訂單公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company', @level2type = N'COLUMN', @level2name = N'IsOrderCompany';
GO

