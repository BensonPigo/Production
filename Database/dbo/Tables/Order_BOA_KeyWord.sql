﻿CREATE TABLE [dbo].[Order_BOA_KeyWord] (
    [ID]            VARCHAR (13)  CONSTRAINT [DF_Order_BOA_KeyWord_ID] DEFAULT ('') NOT NULL,
    [Ukey]          BIGINT        CONSTRAINT [DF_Order_BOA_KeyWord_Ukey] DEFAULT ((0)) NOT NULL,
    [Order_BOAUkey] BIGINT        CONSTRAINT [DF_Order_BOA_KeyWord_Order_BOAUkey] DEFAULT ((0)) NOT NULL,
    [Prefix]        NVARCHAR (60) CONSTRAINT [DF_Order_BOA_KeyWord_Prefix] DEFAULT ('') NOT NULL,
    [KeyWordID]     VARCHAR (30)  CONSTRAINT [DF_Order_BOA_KeyWord_KeyWordID] DEFAULT ('') NOT NULL,
    [Postfix]       NVARCHAR (60) CONSTRAINT [DF_Order_BOA_KeyWord_Postfix] DEFAULT ('') NOT NULL,
    [Code]          VARCHAR (3)   CONSTRAINT [DF_Order_BOA_KeyWord_Code] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_Order_BOA_KeyWord_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_Order_BOA_KeyWord_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME      NULL,
    [Relation]      NVARCHAR (10) CONSTRAINT [DF_Order_BOA_KeyWord_Relation] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Order_BOA_KeyWord] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bill of Accessories- Key word', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BOA的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Order_BOAUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'前置固定的顯示字樣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Prefix';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'對應欄位名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'KeyWordID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結束固定的顯示字樣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Postfix';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組/尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOA_KeyWord', @level2type = N'COLUMN', @level2name = N'EditDate';

