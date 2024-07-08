CREATE TABLE [dbo].[WorkOrderForOutput_SpreadingFabric](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[CutRef] [varchar] (10) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_CutRef] DEFAULT '',
		[POID] [varchar] (13) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_POID] DEFAULT '',
		[Seq1] [varchar] (3) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Seq1] DEFAULT '',
		[Seq2] [varchar] (2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Seq2] DEFAULT '',
		[Roll] [varchar] (8) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Roll] DEFAULT '',
		[Dyelot] [varchar] (8) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Dyelot] DEFAULT '',
		[SCIRefNo] [varchar] (30) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_SCIRefNo] DEFAULT '',
		[ColorID] [varchar] (6) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_ColorID] DEFAULT '',
		[Tone] [varchar] (15) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Tone] DEFAULT '',
		[Issue_DetailUkey] [bigint] NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Issue_DetailUkey] DEFAULT 0,
		[TicketYards] [numeric] (11, 2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_TicketYards] DEFAULT 0,
		[SpreadingLayers] [numeric] (11, 2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_SpreadingLayers] DEFAULT 0,
		[MergeFabYards] [numeric] (11, 2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_MergeFabYards] DEFAULT 0,
		[DamageYards] [numeric] (11, 2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_DamageYards] DEFAULT 0,
		[ActCutends] [numeric] (11, 2) NOT NULL CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_ActCutends] DEFAULT 0,
		
	 CONSTRAINT [PK_WorkOrderForOutput_SpreadingFabric] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pkey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'Ukey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'CutRef'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'POID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'Seq1'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'Seq2'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'Roll'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'Dyelot'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'SCIRefNo'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'ColorID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tone 色差' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'Tone'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'發料單明細 Pkey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'Issue_DetailUkey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫發料單 (WH P10, P13) 已發出的布
其單據紀錄該次發料有多少碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'TicketYards'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際拉布的層數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'SpreadingLayers'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'不同捲布合併同一層時
該層有多少碼不屬於該卷布' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'MergeFabYards'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該卷布有多少碼有出現異常' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'DamageYards'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'拉布結束後，實際剩餘量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForOutput_SpreadingFabric', @level2type=N'COLUMN',@level2name=N'ActCutends'	
	GO
	