CREATE TABLE [dbo].[LocationTrans] (
    [ID]          VARCHAR (13)  CONSTRAINT [DF_LocationTrans_ID] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)   CONSTRAINT [DF_LocationTrans_MDivisionID] DEFAULT ('') NOT NULL,
	[FactoryID]   VARCHAR (8)   CONSTRAINT [DF_LocationTrans_FactoryID] DEFAULT ('') NOT NULL,
    [IssueDate]   DATE          NOT NULL,
    [Status]      VARCHAR (15)  CONSTRAINT [DF_LocationTrans_Encode] DEFAULT ((0)) NOT NULL,
    [Remark]      NVARCHAR (60) CONSTRAINT [DF_LocationTrans_Remark] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_LocationTrans_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_LocationTrans_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_LocationTrans] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位變更主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'StockType';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'MDivisionID';

