CREATE TABLE [dbo].[ADIDASComplain] (
    [ID]          VARCHAR (13)  CONSTRAINT [DF_ADIDASComplain_ID] DEFAULT ('') NOT NULL,
    [StartDate]   DATE          NOT NULL,
    [EndDate]     DATE          NOT NULL,
    [AGCCode]     VARCHAR (6)   CONSTRAINT [DF_ADIDASComplain_AGCCode] DEFAULT ('') NOT NULL,
    [FactoryName] NVARCHAR (50) CONSTRAINT [DF_ADIDASComplain_FactoryName] DEFAULT ('') NOT NULL,
    [Country]     VARCHAR (2)   CONSTRAINT [DF_ADIDASComplain_Country] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_ADIDASComplain_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_ADIDASComplain_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME      NULL,
    [TPEApvName] VARCHAR(10) CONSTRAINT [DF_ADIDASComplain_TPEApvName] DEFAULT ('') NOT NULL, 
    [TPEApvDate] DATETIME NOT NULL, 
    [FtyApvName] VARCHAR(10) CONSTRAINT [DF_ADIDASComplain_FtyApvName] DEFAULT ('') NOT NULL, 
    [FtyApvDate] DATETIME NULL, 
    [Junk]  bit DEFAULT 0 NULL, 
    CONSTRAINT [PK_ADIDASComplain] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ADIDAS Complain 主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ADIDAS Complain 編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'開始日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'StartDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結束日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'EndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AGCCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'AGCCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'FactoryName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'Country';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplain', @level2type = N'COLUMN', @level2name = N'EditDate';

