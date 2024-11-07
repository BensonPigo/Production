CREATE TABLE [dbo].[ChgOverCheckList_Detail](
	[ID] [int] NOT NULL,
	[ChgOverCheckListBaseID] [int] NOT NULL,
	[ResponseDep] [nvarchar] (200) NOT Null CONSTRAINT [DF_ChgOverCheckList_Detail_ResponseDep] DEFAULT '',
	[LeadTime] [smallint] NOT Null CONSTRAINT [DF_ChgOverCheckList_Detail_LeadTime] DEFAULT 0,
	[AddName] [varchar] (10) NOT Null CONSTRAINT [DF_ChgOverCheckList_Detail_AddName] DEFAULT '',
	[AddDate] [datetime] Null,
	[EditName] [varchar] (10) NOT Null CONSTRAINT [DF_ChgOverCheckList_Detail_EditName] DEFAULT '',
	[EditDate] [datetime] Null,		
	CONSTRAINT [PK_ChgOverCheckList_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [ChgOverCheckListBaseID] ASC)
);
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ChgOverCheckListBase.ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ChgOverCheckListBaseID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�t�d����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ResponseDep'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ګe�X��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'LeadTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�s�W�H��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�s�W�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�̫�ק�H��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�̫�ק�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'EditDate'