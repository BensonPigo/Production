CREATE TABLE [dbo].[WorkOrderForOutput_SizeRatio] (
    [WorkOrderForOutputUkey] INT          CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_WorkOrderForOutputUkey] DEFAULT ((0)) NOT NULL,
    [ID]                     VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_ID] DEFAULT ('') NOT NULL,
    [SizeCode]               VARCHAR (8)  CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                    NUMERIC (5)  CONSTRAINT [DF_WorkOrderForOutput_SizeRatio_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrderForOutput_SizeRatio] PRIMARY KEY CLUSTERED ([WorkOrderForOutputUkey] ASC, [SizeCode] ASC)
);
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出裁剪工單主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'WorkOrderForOutputUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'SizeCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SizeRatio', @level2type=N'COLUMN',@level2name=N'Qty'	
	GO

CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_SizeRatio_ID]
    ON [dbo].[WorkOrderForOutput_SizeRatio]([ID] ASC)
    INCLUDE([WorkOrderForOutputUkey], [SizeCode], [Qty]);
