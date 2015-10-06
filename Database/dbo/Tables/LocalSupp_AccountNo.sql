CREATE TABLE [dbo].[LocalSupp_AccountNo] (
    [ID]            VARCHAR (8)  CONSTRAINT [DF_LocalSupp_AccountNo_ID] DEFAULT ('') NOT NULL,
    [ArtworkTypeID] VARCHAR (20) CONSTRAINT [DF_LocalSupp_AccountNo_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [AccountNo]     VARCHAR (8)  CONSTRAINT [DF_LocalSupp_AccountNo_AccountNo] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_LocalSupp_AccountNo_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_LocalSupp_AccountNo_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_LocalSupp_AccountNo] PRIMARY KEY CLUSTERED ([ID] ASC, [ArtworkTypeID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商-Accounting Chart No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artwork Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo', @level2type = N'COLUMN', @level2name = N'AccountNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_AccountNo', @level2type = N'COLUMN', @level2name = N'EditDate';

