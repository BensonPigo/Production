CREATE TABLE [dbo].[WorkOrderForPlanning](
		[Ukey] [int] IDENTITY(1,1) NOT NULL,
		[Type] [varchar] (1) NOT NULL CONSTRAINT [DF_WorkOrderForPlanning_Type] DEFAULT '',
		[ID] [varchar] (13) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_ID] DEFAULT '',
		[FactoryID] [varchar] (8) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_FactoryID] DEFAULT '',
		[MDivisionID] [varchar] (8) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_MDivisionID] DEFAULT '',
		[Seq1] [varchar] (3) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Seq1] DEFAULT '',
		[Seq2] [varchar] (2) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Seq2] DEFAULT '',
		[CutRef] [varchar] (10) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_CutRef] DEFAULT '',
		[CutPlanID] [varchar] (13) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_CutPlanID] DEFAULT '',
		[Layer] [int] NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Layer] DEFAULT 0,
		[WKETA] [date] NULL,
		[EstCutDate] [date] Null,
		[FabricCombo] [varchar] (2) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_FabricCombo] DEFAULT '',
		[FabricCode] [varchar] (3) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_FabricCode] DEFAULT '',
		[FabricPanelCode] [varchar] (2) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_FabricPanelCode] DEFAULT '',
		[RefNo] [varchar] (36) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_RefNo] DEFAULT '',
		[SCIRefNo] [varchar] (30) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_SCIRefNo] DEFAULT '',
		[ColorID] [varchar] (6) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_ColorID] DEFAULT '',
		[ConsPC] [numeric] (12, 4) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_ConsPC] DEFAULT 0,
		[Cons] [numeric] (16, 4) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Cons] DEFAULT 0,
		[Tone] [varchar] (15) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Tone] DEFAULT '',
		[Remark] [nvarchar] (MAX) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Remark] DEFAULT '',
		[IsCreateByUser] [bit] NOT Null CONSTRAINT [DF_WorkOrderForPlanning_IsCreateByUser] DEFAULT 0,
		[MarkerName] [varchar] (20) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_MarkerName] DEFAULT '',
		[MarkerNo] [varchar] (10) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_MarkerNo] DEFAULT '',
		[MarkerVersion] [varchar] (3) NOT NULL CONSTRAINT [DF_WorkOrderForPlanning_MarkerVersion] DEFAULT '',
		[MarkerLength] [varchar] (15) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_MarkerLength] DEFAULT '',
		[Order_EachconsUkey] [bigint] NOT Null CONSTRAINT [DF_WorkOrderForPlanning_Order_EachconsUkey] DEFAULT 0,
		[AddName] [varchar] (10) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_AddName] DEFAULT '',
		[AddDate] [datetime] Null,
		[EditName] [varchar] (10) NOT Null CONSTRAINT [DF_WorkOrderForPlanning_EditName] DEFAULT '',
		[EditDate] [datetime] Null,
		
	 CONSTRAINT [PK_WorkOrderForPlanning] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	Go

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Ukey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用料轉置方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Type'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'FactoryID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'MDivisionID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Seq1'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Seq2'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'CutRef'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪計畫單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'CutPlanID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'層數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Layer'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'船表預計到達時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'WKETA'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'EstCutDate'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'FabricCombo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'布料種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'FabricCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部位別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'FabricPanelCode'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'RefNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'SCIRefNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'ColorID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單件用量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'ConsPC'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Cons'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色差' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Tone'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Remark'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否由使用者建立' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'IsCreateByUser'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'MarkerName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'MarkerNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'MarkerVersion'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'MarkerLength'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單用料主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Order_EachconsUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'AddName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'AddDate'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'EditName'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'EditDate'
	GO