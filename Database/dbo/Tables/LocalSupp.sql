CREATE TABLE [dbo].[LocalSupp] (
    [ID]              VARCHAR (8)    CONSTRAINT [DF_LocalSupp_ID] DEFAULT ('') NOT NULL,
    [Junk]            BIT            CONSTRAINT [DF_LocalSupp_Junk] DEFAULT ((0)) NULL,
    [Abb]             NVARCHAR (12)  CONSTRAINT [DF_LocalSupp_Abb] DEFAULT ('') NOT NULL,
    [Name]            NVARCHAR (60)  CONSTRAINT [DF_LocalSupp_Name] DEFAULT ('') NOT NULL,
    [CountryID]       VARCHAR (2)    CONSTRAINT [DF_LocalSupp_CountryID] DEFAULT ('') NOT NULL,
    [Tel]             VARCHAR (30)   CONSTRAINT [DF_LocalSupp_Tel] DEFAULT ('') NULL,
    [Fax]             VARCHAR (30)   CONSTRAINT [DF_LocalSupp_Fax] DEFAULT ('') NULL,
    [Address]         NVARCHAR (MAX) CONSTRAINT [DF_LocalSupp_Address] DEFAULT ('') NULL,
    [CurrencyID]      VARCHAR (3)    CONSTRAINT [DF_LocalSupp_CurrencyID] DEFAULT ('') NULL,
    [PayTermID]       VARCHAR (6)    CONSTRAINT [DF_LocalSupp_PayTermID] DEFAULT ('') NULL,
    [WithholdingRate] TINYINT        CONSTRAINT [DF_LocalSupp_WithholdingRate] DEFAULT ((0)) NULL,
    [UseSBTS]         BIT            CONSTRAINT [DF_LocalSupp_UseSBTS] DEFAULT ((0)) NULL,
    [IsFactory]       BIT            CONSTRAINT [DF_LocalSupp_IsFactory] DEFAULT ((0)) NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_LocalSupp_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_LocalSupp_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    [IsShipping]      BIT            DEFAULT ((0)) NULL,
    [IsSubcon]        BIT            DEFAULT ((0)) NULL,
    [IsMisc]          BIT            DEFAULT ((0)) NULL,
    [IsSintexSubcon]  BIT            CONSTRAINT [DF_LocalSupp_IsSintexSubcon] DEFAULT ((0)) NULL,
    [email] VARCHAR(50) NOT NULL DEFAULT (''), 
    [Status] VARCHAR(15) NOT NULL DEFAULT (''), 
    [Remark] NVARCHAR(600) NULL, 
	IsFreightForwarder BIT			 CONSTRAINT [DF_LocalSupp_IsFreightForwarder] DEFAULT((0)) NOT NULL,
    CONSTRAINT [PK_LocalSupp] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'Abb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳真', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'Fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'Address';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'PayTermID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預扣稅 (%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'WithholdingRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否導入Subcon Bundle Tracking System', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'UseSBTS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'IsFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��Subcon������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'IsSubcon';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��Shipping������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'IsShipping';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��Misc������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp', @level2type = N'COLUMN', @level2name = N'IsMisc';




GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'判斷付費對象是否為 Forwarder',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalSupp',
    @level2type = N'COLUMN',
    @level2name = N'IsFreightForwarder'