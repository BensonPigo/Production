CREATE TABLE [dbo].[WorkOrderForOutputDelete](
		[Ukey] [int] IDENTITY(1,1) NOT NULL,
		[ID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutputDelete_ID] DEFAULT '',
		[GroupID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutputDelete_GroupID] DEFAULT '',
		[CutRef] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutputDelete_CutRef] DEFAULT '',
		[Layer] [int] NOT NULL CONSTRAINT [DF_WorkOrderForOutputDelete_Layer] DEFAULT 0,
		
	 CONSTRAINT [PK_WorkOrderForOutputDelete] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputDelete', @level2type=N'COLUMN',@level2name=N'Ukey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputDelete', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'刪除前群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputDelete', @level2type=N'COLUMN',@level2name=N'GroupID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputDelete', @level2type=N'COLUMN',@level2name=N'CutRef'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'層數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputDelete', @level2type=N'COLUMN',@level2name=N'Layer'
	GO
	