CREATE TABLE [dbo].[WorkOrderForOutput_SizeRatio](
		[WorkOrderForOutputUkey] [int] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_WorkOrderForOutputUkey] DEFAULT 0,
		[ID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_ID] DEFAULT '',
		[SizeCode] [varchar] (8) NOT Null CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_SizeCode] DEFAULT '',
		[Qty] [numeric] (5, 0) NOT Null CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_Qty] DEFAULT 0,
		
	 CONSTRAINT [PK_WorkOrderForOutput_SizeRatio] PRIMARY KEY CLUSTERED 
	(
		[WorkOrderForOutputUkey] ASC,
		[SizeCode] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出裁剪工單主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'WorkOrderForOutputUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'SizeCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'Qty'	
	GO
	