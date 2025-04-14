CREATE TABLE [dbo].[Buyer] (
    [ID]         VARCHAR (8)    CONSTRAINT [DF_Buyer_ID] DEFAULT ('') NOT NULL,
    [CountryID]  VARCHAR (2)    CONSTRAINT [DF_Buyer_CountryID] DEFAULT ('') NOT NULL,
    [NameCH]     NVARCHAR (30)  CONSTRAINT [DF_Buyer_NameCH] DEFAULT ('') NOT NULL,
    [NameEN]     NVARCHAR (80)  CONSTRAINT [DF_Buyer_NameEN] DEFAULT ('') NOT NULL,
    [Tel]        VARCHAR (20)   CONSTRAINT [DF_Buyer_Tel] DEFAULT ('') NOT NULL,
    [Fax]        VARCHAR (20)   CONSTRAINT [DF_Buyer_Fax] DEFAULT ('') NOT NULL,
    [Contact1]   VARCHAR (20)   CONSTRAINT [DF_Buyer_Contact1] DEFAULT ('') NOT NULL,
    [Contact2]   VARCHAR (20)   CONSTRAINT [DF_Buyer_Contact2] DEFAULT ('') NOT NULL,
    [AddressCH]  NVARCHAR (50)  CONSTRAINT [DF_Buyer_AddressCH] DEFAULT ('') NOT NULL,
    [AddressEN]  NVARCHAR (MAX) CONSTRAINT [DF_Buyer_AddressEN] DEFAULT ('') NOT NULL,
    [BillTo1]    NVARCHAR (40)  CONSTRAINT [DF_Buyer_BillTo1] DEFAULT ('') NOT NULL,
    [BillTo2]    NVARCHAR (40)  CONSTRAINT [DF_Buyer_BillTo2] DEFAULT ('') NOT NULL,
    [BillTo3]    NVARCHAR (40)  CONSTRAINT [DF_Buyer_BillTo3] DEFAULT ('') NOT NULL,
    [BillTo4]    NVARCHAR (40)  CONSTRAINT [DF_Buyer_BillTo4] DEFAULT ('') NOT NULL,
    [BillTo5]    NVARCHAR (40)  CONSTRAINT [DF_Buyer_BillTo5] DEFAULT ('') NOT NULL,
    [CurrencyID] VARCHAR (3)    CONSTRAINT [DF_Buyer_CurrencyID] DEFAULT ('') NOT NULL,
    [Remark]     NVARCHAR (MAX) CONSTRAINT [DF_Buyer_Remark] DEFAULT ('') NOT NULL,
    [ZipCode]    VARCHAR (6)    CONSTRAINT [DF_Buyer_ZipCode] DEFAULT ('') NOT NULL,
    [Email]      NVARCHAR (50)  CONSTRAINT [DF_Buyer_Email] DEFAULT ('') NOT NULL,
    [MrTeam]     VARCHAR (5)    CONSTRAINT [DF_Buyer_MrTeam] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10)   CONSTRAINT [DF_Buyer_AddName] DEFAULT ('') NOT NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   CONSTRAINT [DF_Buyer_EditName] DEFAULT ('') NOT NULL,
    [EditDate]   DATETIME       NULL,
    [Junk]       BIT            CONSTRAINT [DF_Buyer_Junk] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Buyer] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Buyer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳真', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'Fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'聯絡人1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'Contact1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'聯絡人2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'Contact2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'AddressCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'AddressEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill to', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'BillTo1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill to', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'BillTo2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill to', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'BillTo3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill to', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'BillTo4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill to', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'BillTo5';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'郵遞區號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'ZipCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'E_Mail Address', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'Email';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'業務組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'MrTeam';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Buyer', @level2type = N'COLUMN', @level2name = N'Junk';

