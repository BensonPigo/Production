CREATE TABLE [dbo].[WorkOrderForOutput_Distribute] (
    [WorkOrderForOutputUkey] INT          CONSTRAINT [DF_WorkOrderForOutput_Distribute_WorkOrderForOutputUkey] DEFAULT ((0)) NOT NULL,
    [ID]                     VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutput_Distribute_ID] DEFAULT ('') NOT NULL,
    [OrderID]                VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutput_Distribute_OrderID] DEFAULT ('') NOT NULL,
    [Article]                VARCHAR (8)  CONSTRAINT [DF_WorkOrderForOutput_Distribute_Article] DEFAULT ('') NOT NULL,
    [SizeCode]               VARCHAR (8)  CONSTRAINT [DF_WorkOrderForOutput_Distribute_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                    NUMERIC (6)  CONSTRAINT [DF_WorkOrderForOutput_Distribute_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrderForOutput_Distribute] PRIMARY KEY CLUSTERED ([WorkOrderForOutputUkey] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);

	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出裁剪工單主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_Distribute', @level2type=N'COLUMN',@level2name=N'WorkOrderForOutputUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_Distribute', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_Distribute', @level2type=N'COLUMN',@level2name=N'OrderID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_Distribute', @level2type=N'COLUMN',@level2name=N'Article'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_Distribute', @level2type=N'COLUMN',@level2name=N'SizeCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_Distribute', @level2type=N'COLUMN',@level2name=N'Qty'

	GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_Distribute_SPUkey]
    ON [dbo].[WorkOrderForOutput_Distribute]([OrderID] ASC, [WorkOrderForOutputUkey] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_Distribute_OrderID]
    ON [dbo].[WorkOrderForOutput_Distribute]([OrderID] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_Distribute_ID]
    ON [dbo].[WorkOrderForOutput_Distribute]([ID] ASC)
    INCLUDE([WorkOrderForOutputUkey], [OrderID], [Article], [SizeCode], [Qty]);
	
GO
