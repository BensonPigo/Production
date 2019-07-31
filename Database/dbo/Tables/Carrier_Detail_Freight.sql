CREATE TABLE [dbo].[Carrier_Detail_Freight] (
    [ID]          VARCHAR (4)    CONSTRAINT [DF_Carrier_Detail_Freight_ID] DEFAULT ('') NOT NULL,
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Payer]       VARCHAR (1)    CONSTRAINT [DF_Carrier_Detail_Freight_Payer] DEFAULT ('') NULL,
    [FromTag]     VARCHAR (1)    CONSTRAINT [DF_Carrier_Detail_Freight_FromTag] DEFAULT ('') NULL,
    [FromInclude] NVARCHAR (254) CONSTRAINT [DF_Carrier_Detail_Freight_FromInclude] DEFAULT ('') NULL,
    [FromExclude] NVARCHAR (254) CONSTRAINT [DF_Carrier_Detail_Freight_FromExclude] DEFAULT ('') NULL,
    [ToTag]       VARCHAR (1)    CONSTRAINT [DF_Carrier_Detail_Freight_ToTag] DEFAULT ('') NULL,
    [ToInclude]   NVARCHAR (254) CONSTRAINT [DF_Carrier_Detail_Freight_ToInclude] DEFAULT ('') NULL,
    [ToExclude]   NVARCHAR (254) CONSTRAINT [DF_Carrier_Detail_Freight_ToExclude] DEFAULT ('') NULL,
    [ToFty]       VARCHAR (8)    CONSTRAINT [DF_Carrier_Detail_Freight_ToFty] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_Carrier_Detail_Freight_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_Carrier_Detail_Freight_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_Carrier_Detail_Freight] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件指定工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'ToFty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件排除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'ToExclude';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件包含', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'ToInclude';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件對象', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'ToTag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄件排除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'FromExclude';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄件包含', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'FromInclude';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄件對象', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'FromTag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款對象', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'Payer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際快遞帳號套用規則檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_Detail_Freight';

