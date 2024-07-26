CREATE TABLE [dbo].[ChgOverCheckList] (
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [varchar] (1) NOT NULL CONSTRAINT [DF_ChgOverCheckList_Category] DEFAULT '',
	[StyleType] [varchar] (1) NOT Null CONSTRAINT [DF_ChgOverCheckList_StyleType] DEFAULT '',
	[SendMail] [nvarchar] (max) NOT Null CONSTRAINT [DF_ChgOverCheckList_SendMail] DEFAULT '',
	[Junk] [bit] NOT Null CONSTRAINT [DF_ChgOverCheckList_Junk] DEFAULT 0,
	[AddName] [varchar] (10) NOT Null CONSTRAINT [DF_ChgOverCheckList_AddName] DEFAULT '',
	[AddDate] [datetime] Null,
	[EditName] [varchar] (10) NOT Null CONSTRAINT [DF_ChgOverCheckList_EditName] DEFAULT '',
	[EditDate] [datetime] Null,		
	CONSTRAINT [PK_ChgOverCheckList] PRIMARY KEY CLUSTERED ([ID] ASC)	
);
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Category，詳細可參考SP：ChangeOver' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為做過的Style，分為N(New)和R(Repeat)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'StyleType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'超過LeadTime發送通知信地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'SendMail'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'停用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList', @level2type=N'COLUMN',@level2name=N'EditDate'
