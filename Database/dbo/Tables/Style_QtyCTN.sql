CREATE TABLE [dbo].[Style_QtyCTN] (
    [StyleUkey] BIGINT       CONSTRAINT [DF_Style_QtyCTN_StyleUkey] DEFAULT ((0)) NULL,
    [CustCDID]  VARCHAR (16) CONSTRAINT [DF_Style_QtyCTN_CustCDID] DEFAULT ('') NULL,
    [Qty]       SMALLINT     CONSTRAINT [DF_Style_QtyCTN_Qty] DEFAULT ((0)) NULL,
    [CountryID] VARCHAR (2)  CONSTRAINT [DF_Style_QtyCTN_CountryID] DEFAULT ('') NULL,
    [Continent] VARCHAR (2)  CONSTRAINT [DF_Style_QtyCTN_Continent] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_Style_QtyCTN_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_Style_QtyCTN_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    [UKey]      BIGINT       CONSTRAINT [DF_Style_QtyCTN_UKey] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Style_QtyCTN] PRIMARY KEY CLUSTERED ([UKey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty/Carton by CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cust CD Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'CustCDID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝箱件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'洲別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'Continent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_QtyCTN', @level2type = N'COLUMN', @level2name = N'UKey';

