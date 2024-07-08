CREATE TABLE [dbo].[WorkOrderForOutput](
		[Ukey] [int] IDENTITY(1,1) NOT NULL,
		[ID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_ID] DEFAULT '',
		[FactoryID] [varchar] (8) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_FactoryID] DEFAULT '',
		[MDivisionID] [varchar] (8) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_MDivisionID] DEFAULT '',
		[Seq1] [varchar] (3) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_Seq1] DEFAULT '',
		[Seq2] [varchar] (2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_Seq2] DEFAULT '',
		[CutRef] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_CutRef] DEFAULT '',
		[CutNo] [int] NULL,
		[OrderID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_OrderID] DEFAULT '',
		[RefNo] [varchar] (36) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_RefNo] DEFAULT '',
		[SCIRefNo] [varchar] (30) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SCIRefNo] DEFAULT '',
		[ColorID] [varchar] (6) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_ColorID] DEFAULT '',
		[Tone] [varchar] (15) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_Tone] DEFAULT '',
		[Layer] [int] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_Layer] DEFAULT 0,
		[WKETA] [date] NULL,
		[FabricCombo] [varchar] (2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_FabricCombo] DEFAULT '',
		[FabricCode] [varchar] (3) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_FabricCode] DEFAULT '',
		[FabricPanelCode] [varchar] (2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_FabricPanelCode] DEFAULT '',
		[EstCutDate] [date] NULL,
		[ConsPC] [numeric] (12, 4) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_ConsPC] DEFAULT 0,
		[Cons] [numeric] (16, 4) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_Cons] DEFAULT 0,
		[MarkerNo] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_MarkerNo] DEFAULT '',
		[MarkerName] [varchar] (20) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_MarkerName] DEFAULT '',
		[MarkerLength] [varchar] (15) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_MarkerLength] DEFAULT '',
		[MarkerVersion] [varchar] (3) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_MarkerVersion] DEFAULT '',
		[ActCuttingPerimeter] [nvarchar] (15) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_ActCuttingPerimeter] DEFAULT '',
		[StraightLength] [varchar] (15) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_StraightLength] DEFAULT '',
		[CurvedLength] [varchar] (15) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_CurvedLength] DEFAULT '',
		[Shift] [varchar] (1) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_Shift] DEFAULT '',
		[CutCellID] [varchar] (2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_CutCellID] DEFAULT '',
		[SpreadingNoID] [varchar] (5) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingNoID] DEFAULT '',
		[UnfinishedCuttingReason] [varchar] (50) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_UnfinishedCuttingReason] DEFAULT '',
		[UnfinishedCuttingRemark] [nvarchar] (500) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_UnfinishedCuttingRemark] DEFAULT '',
		[IsCreateByUser] [bit] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_IsCreateByUser] DEFAULT 0,
		[SpreadingStatus] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingStatus] DEFAULT 'Ready',
		[SpreadingRemark] [nvarchar] (500) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingRemark] DEFAULT '',
		[GroupID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_GroupID] DEFAULT '',
		[WorkOrderForPlanningUkey] [int] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_WorkOrderForPlanningUkey] DEFAULT 0,
		[SourceFrom] [varchar] (1) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SourceFrom] DEFAULT '',
		[CuttingMethod] [bit] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_CuttingMethod] DEFAULT 0,
		[Order_EachconsUkey] [bigint] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_Order_EachconsUkey] DEFAULT 0,
		[AddName] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_AddName] DEFAULT '',
		[AddDate] [datetime] NULL,
		[EditName] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_EditName] DEFAULT '',
		[EditDate] [datetime] NULL,
		
	 CONSTRAINT [PK_WorkOrderForOutput] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Ukey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'FactoryID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'MDivisionID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Seq1'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Seq2'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'CutRef'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'CutNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'OrderID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'RefNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'SCIRefNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'ColorID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tone色差' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Tone'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'層數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Layer'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'船表預計到達時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'WKETA'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'FabricCombo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'FabricCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'FabricPanelCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'EstCutDate'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單件用量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'ConsPC'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Cons'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'MarkerNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'MarkerName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'MarkerLength'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'MarkerVersion'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際裁剪碼長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'ActCuttingPerimeter'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克直徑長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'StraightLength'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克曲線長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'CurvedLength'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Shift'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁桌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'CutCellID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拉布桌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'SpreadingNoID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未裁剪原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'UnfinishedCuttingReason'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'未裁剪註記' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'UnfinishedCuttingRemark'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否由使用者建立' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'IsCreateByUser'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ready,Spreading,Finished
	用於M360 Digital Spreading' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'SpreadingStatus'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拉布人員在拉布時的備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'SpreadingRemark'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Digital Spreading 合併與拆分的群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'GroupID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'WorkOrderForPlanning主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'WorkOrderForPlanningUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1>Cutting_P02. WorkOrder For Planning
2>Cutting_P09. WorkOrder For Output
3>M360_Digital Spreading' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'SourceFrom'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手裁 Manual (0) 還是機裁 Auto (1)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'CuttingMethod'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單用料主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'Order_EachconsUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'AddName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'AddDate'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'EditName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput', @level2type=N'COLUMN',@level2name=N'EditDate'
	GO
	
