CREATE TABLE [dbo].[Style_BOA_CustCD] (
    [StyleUkey]     BIGINT       CONSTRAINT [DF_Style_BOA_CustCD_StyleUkey] DEFAULT ((0)) NULL,
    [Style_BOAUkey] BIGINT       CONSTRAINT [DF_Style_BOA_CustCD_Style_BOAUkey] DEFAULT ((0)) NOT NULL,
    [CustCDID]      VARCHAR (16) CONSTRAINT [DF_Style_BOA_CustCD_CustCDID] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (36) CONSTRAINT [DF_Style_BOA_CustCD_Refno] DEFAULT ('') NULL,
    [SCIRefno]      VARCHAR (30) CONSTRAINT [DF_Style_BOA_CustCD_SCIRefno] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Style_BOA_CustCD_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Style_BOA_CustCD_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_Style_BOA_CustCD] PRIMARY KEY CLUSTERED ([Style_BOAUkey] ASC, [CustCDID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill of Accessories - For CustCd', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BOA的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'Style_BOAUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶資料代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'CustCDID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客人的料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_CustCD', @level2type = N'COLUMN', @level2name = N'EditDate';

