CREATE TABLE [dbo].[LocalItem_Quot] (
    [RefNo]        VARCHAR (21)    CONSTRAINT [DF_LocalItem_Quot_RefNo] DEFAULT ('') NOT NULL,
    [IssueDate]    DATE            NULL,
    [LocalSuppID1] VARCHAR (8)     CONSTRAINT [DF_LocalItem_Quot_LocalSuppID1] DEFAULT ('') NULL,
    [LocalSuppID2] VARCHAR (8)     CONSTRAINT [DF_LocalItem_Quot_LocalSuppID2] DEFAULT ('') NULL,
    [LocalSuppID3] VARCHAR (8)     CONSTRAINT [DF_LocalItem_Quot_LocalSuppID3] DEFAULT ('') NULL,
    [LocalSuppID4] VARCHAR (8)     CONSTRAINT [DF_LocalItem_Quot_LocalSuppID4] DEFAULT ('') NULL,
    [CurrencyID1]  VARCHAR (3)     CONSTRAINT [DF_LocalItem_Quot_CurrencyID1] DEFAULT ('') NULL,
    [CurrencyID2]  VARCHAR (3)     CONSTRAINT [DF_LocalItem_Quot_CurrencyID2] DEFAULT ('') NULL,
    [CurrencyID3]  VARCHAR (3)     CONSTRAINT [DF_LocalItem_Quot_CurrencyID3] DEFAULT ('') NULL,
    [CurrencyID4]  VARCHAR (3)     CONSTRAINT [DF_LocalItem_Quot_CurrencyID4] DEFAULT ('') NULL,
    [Price1]       NUMERIC (12, 4) CONSTRAINT [DF_LocalItem_Quot_Price1] DEFAULT ((0)) NULL,
    [Price2]       NUMERIC (12, 4) CONSTRAINT [DF_LocalItem_Quot_Price2] DEFAULT ((0)) NULL,
    [Price3]       NUMERIC (12, 4) CONSTRAINT [DF_LocalItem_Quot_Price3] DEFAULT ((0)) NULL,
    [Price4]       NUMERIC (12, 4) CONSTRAINT [DF_LocalItem_Quot_Price4] DEFAULT ((0)) NULL,
    [Status]       VARCHAR (15)    CONSTRAINT [DF_LocalItem_Quot_Status] DEFAULT ('') NULL,
    [ChooseSupp]   NUMERIC (1)     CONSTRAINT [DF_LocalItem_Quot_ChooseSupp] DEFAULT ((0)) NULL,
    [AddName]      VARCHAR (10)    CONSTRAINT [DF_LocalItem_Quot_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME        NULL,
    [EditName]     VARCHAR (10)    CONSTRAINT [DF_LocalItem_Quot_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME        NULL,
    [Ukey]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [QuotDate1] DATE NULL, 
    [QuotDate2] DATE NULL, 
    [QuotDate3] DATE NULL, 
    [QuotDate4] DATE NULL, 
    CONSTRAINT [PK_LocalItem_Quot] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Purchase Item Canvass Record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'LocalSuppID1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'LocalSuppID2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'LocalSuppID3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'LocalSuppID4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'CurrencyID1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'CurrencyID2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'CurrencyID3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'CurrencyID4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商報價金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'Price1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商報價金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'Price2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商報價金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'Price3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商報價金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'Price4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'ChooseSupp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem_Quot', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'報價日期1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalItem_Quot',
    @level2type = N'COLUMN',
    @level2name = N'QuotDate1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'報價日期2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalItem_Quot',
    @level2type = N'COLUMN',
    @level2name = N'QuotDate2'
GO

GO

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'報價日期3',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalItem_Quot',
    @level2type = N'COLUMN',
    @level2name = N'QuotDate3'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'報價日期4',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalItem_Quot',
    @level2type = N'COLUMN',
    @level2name = N'QuotDate4'