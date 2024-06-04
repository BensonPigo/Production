CREATE TABLE [dbo].[WorkOrderForPlanning_Distribute](
		[WorkOrderForPlanningUkey] [int] NOT NULL CONSTRAINT [DF_WorkOrderForPlanning_Distribute_WorkOrderForPlanningUkey] DEFAULT 0,
		[ID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForPlanning_Distribute_ID] DEFAULT '',
		[OrderID] [varchar] (13) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Distribute_OrderID] DEFAULT '',
		[Article] [varchar] (8) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Distribute_Article] DEFAULT '',
		[SizeCode] [varchar] (8) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Distribute_SizeCode] DEFAULT '',
		[Qty] [numeric] (6, 0) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Distribute_Qty] DEFAULT 0,
		
	 CONSTRAINT [PK_WorkOrderForPlanning_Distribute] PRIMARY KEY CLUSTERED 
	(
		[WorkOrderForPlanningUkey] ASC,
		[OrderID] ASC,
		[Article] ASC,
		[SizeCode] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料裁剪工單主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_Distribute', @level2type=N'COLUMN',@level2name=N'WorkOrderForPlanningUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_Distribute', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_Distribute', @level2type=N'COLUMN',@level2name=N'OrderID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_Distribute', @level2type=N'COLUMN',@level2name=N'Article'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_Distribute', @level2type=N'COLUMN',@level2name=N'SizeCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_Distribute', @level2type=N'COLUMN',@level2name=N'Qty'
	GO