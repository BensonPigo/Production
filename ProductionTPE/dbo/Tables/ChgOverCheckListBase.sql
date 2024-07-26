CREATE TABLE [dbo].[ChgOverCheckListBase](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[No] [int] NOT NULL CONSTRAINT [DF_ChgOverCheckListBase_No] DEFAULT 0,
		[CheckList] [varchar] (200) NOT Null CONSTRAINT [DF_ChgOverCheckListBase_CheckList] DEFAULT '',
		[Junk] [bit] NOT Null CONSTRAINT [DF_ChgOverCheckListBase_Junk] DEFAULT 0,
		[AddName] [varchar] (10) NOT Null CONSTRAINT [DF_ChgOverCheckListBase_AddName] DEFAULT '',
		[AddDate] [datetime] Null,
		[EditName] [varchar] (10) NOT Null CONSTRAINT [DF_ChgOverCheckListBase_EditName] DEFAULT '',
		[EditDate] [datetime] Null,
		
	 CONSTRAINT [PK_ChgOverCheckListBase] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check List 代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'No'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check List 項目名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'CheckList'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'停用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'Junk'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'AddName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'AddDate'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'EditName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChgOverCheckListBase', @level2type=N'COLUMN',@level2name=N'EditDate'
