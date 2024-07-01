CREATE TABLE [dbo].[WorkOrderForPlanning_SizeRatio] (
    [WorkOrderForPlanningUkey] INT          CONSTRAINT [DF_WorkOrderForPlanning_SizeRatio_WorkOrderForPlanningUkey] DEFAULT ((0)) NOT NULL,
    [ID]                       VARCHAR (13) CONSTRAINT [DF_WorkOrderForPlanning_SizeRatio_ID] DEFAULT ('') NOT NULL,
    [SizeCode]                 VARCHAR (8)  CONSTRAINT [DF_WorkOrderForPlanning_SizeRatio_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]                      NUMERIC (5)  CONSTRAINT [DF_WorkOrderForPlanning_SizeRatio_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrderForPlanning_SizeRatio] PRIMARY KEY CLUSTERED ([WorkOrderForPlanningUkey] ASC, [SizeCode] ASC)
);


	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料裁剪工單主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_SizeRatio', @level2type=N'COLUMN',@level2name=N'WorkOrderForPlanningUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_SizeRatio', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'尺寸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_SizeRatio', @level2type=N'COLUMN',@level2name=N'SizeCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning_SizeRatio', @level2type=N'COLUMN',@level2name=N'Qty'
	GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForPlanning_SizeRatio_ID]
    ON [dbo].[WorkOrderForPlanning_SizeRatio]([ID] ASC)
    INCLUDE([WorkOrderForPlanningUkey], [SizeCode], [Qty]);

