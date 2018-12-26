CREATE TABLE [dbo].[ArtworkPO] (
    [ID]             VARCHAR (13)    CONSTRAINT [DF_ArtworkPO_ID] DEFAULT ('') NOT NULL,
    [MDivisionID]    VARCHAR (8)     CONSTRAINT [DF_ArtworkPO_MDivisionID] DEFAULT ('') NULL,
    [FactoryId]      VARCHAR (8)     CONSTRAINT [DF_ArtworkPO_FactoryId] DEFAULT ('') NOT NULL,
    [LocalSuppID]    VARCHAR (8)     CONSTRAINT [DF_ArtworkPO_LocalSuppID] DEFAULT ('') NOT NULL,
    [IssueDate]      DATE            NOT NULL,
    [Delivery]       DATE            NULL,
    [ArtworkTypeID]  VARCHAR (20)    CONSTRAINT [DF_ArtworkPO_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [CurrencyId]     VARCHAR (3)     CONSTRAINT [DF_ArtworkPO_CurrencyId] DEFAULT ('') NULL,
    [Amount]         NUMERIC (12, 2) CONSTRAINT [DF_ArtworkPO_Amount] DEFAULT ((0)) NULL,
    [VatRate]        NUMERIC (3, 1)  CONSTRAINT [DF_ArtworkPO_VatRate] DEFAULT ((0)) NULL,
    [Vat]            NUMERIC (11, 2) CONSTRAINT [DF_ArtworkPO_Vat] DEFAULT ((0)) NULL,
    [Remark]         NVARCHAR (60)   CONSTRAINT [DF_ArtworkPO_Remark] DEFAULT ('') NULL,
    [InternalRemark] NVARCHAR (60)   CONSTRAINT [DF_ArtworkPO_InternalRemark] DEFAULT ('') NULL,
    [Closed]         BIT             CONSTRAINT [DF_ArtworkPO_Closed] DEFAULT ((0)) NULL,
    [ApvName]        VARCHAR (10)    CONSTRAINT [DF_ArtworkPO_ApvName] DEFAULT ('') NULL,
    [ApvDate]        DATE            NULL,
    [Handle]         VARCHAR (10)    CONSTRAINT [DF_ArtworkPO_Handle] DEFAULT ('') NULL,
    [POType]         VARCHAR (1)     CONSTRAINT [DF_ArtworkPO_POType] DEFAULT ('') NULL,
    [Exceed]         BIT             CONSTRAINT [DF_ArtworkPO_Exceed] DEFAULT ((0)) NULL,
    [AddName]        VARCHAR (10)    CONSTRAINT [DF_ArtworkPO_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME        NULL,
    [EditName]       VARCHAR (10)    CONSTRAINT [DF_ArtworkPO_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME        NULL,
    [Status]         VARCHAR (15)    CONSTRAINT [DF_ArtworkPO_Status] DEFAULT ('') NULL,
    [LockName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [LockDate] DATE NULL, 
    [CloseName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [CloseDate] DATE NULL, 
    CONSTRAINT [PK_ArtworkPO] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工單主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Delivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'CurrencyId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'VatRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Vat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠內備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'InternalRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Closed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核單人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'POType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'可超出', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Exceed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ArtworkPO', @level2type = N'COLUMN', @level2name = N'MDivisionID';

