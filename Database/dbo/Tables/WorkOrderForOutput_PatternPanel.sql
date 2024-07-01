CREATE TABLE [dbo].[WorkOrderForOutput_PatternPanel](
		[WorkOrderForOutputUkey] [int] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_WorkOrderForOutputUkey] DEFAULT 0,
		[ID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_ID] DEFAULT '',
		[PatternPanel] [varchar] (2) NOT Null CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_PatternPanel] DEFAULT '',
		[FabricPanelCode] [varchar] (2) NOT Null CONSTRAINT [DF_WorkOrderForOutput_PatternPanel_FabricPanelCode] DEFAULT '',
		
	 CONSTRAINT [PK_WorkOrderForOutput_PatternPanel] PRIMARY KEY CLUSTERED 
	(
		[WorkOrderForOutputUkey] ASC,
		[PatternPanel] ASC,
		[FabricPanelCode] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出裁剪工單主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_PatternPanel', @level2type=N'COLUMN',@level2name=N'WorkOrderForOutputUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_PatternPanel', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_PatternPanel', @level2type=N'COLUMN',@level2name=N'PatternPanel'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_PatternPanel', @level2type=N'COLUMN',@level2name=N'FabricPanelCode'
	GO