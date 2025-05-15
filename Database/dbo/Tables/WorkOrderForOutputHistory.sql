CREATE TABLE [dbo].[WorkOrderForOutputHistory](
		[Ukey] [int] IDENTITY(1,1) NOT NULL,
		[ID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutputHistory_ID] DEFAULT '',
		[SourceFrom] [varchar] (1) NOT NULL CONSTRAINT [DF_WorkOrderForOutputHistory_SourceFrom] DEFAULT '',
		[GroupID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutputHistory_GroupID] DEFAULT '',
		[CutRef] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutputHistory_CutRef] DEFAULT '',
		[Layer] [int] NOT NULL CONSTRAINT [DF_WorkOrderForOutputHistory_Layer] DEFAULT 0,
		[CutNo] [int] NULL,
		
	 CONSTRAINT [PK_WorkOrderForOutputHistory] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputHistory', @level2type=N'COLUMN',@level2name=N'Ukey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputHistory', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1>Cutting_P09. WorkOrder For Output
2>M360_Digital Spreading' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputHistory', @level2type=N'COLUMN',@level2name=N'SourceFrom'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改前群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputHistory', @level2type=N'COLUMN',@level2name=N'GroupID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputHistory', @level2type=N'COLUMN',@level2name=N'CutRef'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'層數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputHistory', @level2type=N'COLUMN',@level2name=N'Layer'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutputHistory', @level2type=N'COLUMN',@level2name=N'CutNo'
	GO