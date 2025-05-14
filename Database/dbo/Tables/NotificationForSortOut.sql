CREATE TABLE [dbo].[NotificationForSortOut] (
    [FactoryID]       Varchar(8)     NOT NULL,
    [ToAddress]       Varchar(500)   NOT NULL default(''),
    [CcAddress]       VARCHAR(500)   NOT NULL default(''),
    [StartTime]       Time           NULL,
	[EndTime]         Time           NULL,
    [Frequency]       Varchar(20)    NOT NULL default(''),
    [Description]     NVARCHAR (500) NOT NULL default(''),
    [AddDate]         datetime       NULL,
    [AddName]		  VARCHAR (10)   NOT NULL default(''),
	[EditDate]        datetime       NULL,
    [Editname]		  VARCHAR (10)   NOT NULL default(''),
	[LastExecuteTime] datetime 
    CONSTRAINT [PK_NotificationForSortOut] PRIMARY KEY CLUSTERED ([FactoryID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mail To', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'ToAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mail CC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'CcAddress';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'開始時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'StartTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結束時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'EndTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'頻率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'Frequency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新建日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NotificationForSortOut', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新建人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'Editname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後一次執行的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList', @level2type = N'COLUMN', @level2name = N'LastExecuteTime';


GO
