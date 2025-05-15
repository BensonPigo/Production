
CREATE TABLE [dbo].[WorkOrderForOutput_SpreadingFabric] (
    [Ukey]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [CutRef]          VARCHAR (10)    CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_CutRef] DEFAULT ('') NOT NULL,
    [POID]            VARCHAR (13)    CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_POID] DEFAULT ('') NOT NULL,
    [Seq1]            VARCHAR (3)     CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]            VARCHAR (2)     CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Seq2] DEFAULT ('') NOT NULL,
    [Roll]            VARCHAR (8)     CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]          VARCHAR (8)     CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Dyelot] DEFAULT ('') NOT NULL,
    [SCIRefNo]        VARCHAR (30)    CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_SCIRefNo] DEFAULT ('') NOT NULL,
    [ColorID]         VARCHAR (6)     CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_ColorID] DEFAULT ('') NOT NULL,
    [Tone]            VARCHAR (15)    CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_Tone] DEFAULT ('') NOT NULL,
    [InsertByManual]  BIT             CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_InsertByManual] DEFAULT ((0)) NOT NULL,
    [TicketYards]     NUMERIC (11, 2) CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_TicketYards] DEFAULT ((0)) NOT NULL,
    [SpreadingLayers] TINYINT         NULL,
    [MergeFabYards]   NUMERIC (11, 2) CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_MergeFabYards] DEFAULT ((0)) NOT NULL,
    [DamageYards]     NUMERIC (11, 2) CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_DamageYards] DEFAULT ((0)) NOT NULL,
    [ActCutends]      NUMERIC (11, 2) CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_ActCutends] DEFAULT ((0)) NOT NULL,
	[AddDate]      DATETIME  NULL,
	[AddName]      VARCHAR(10) CONSTRAINT [DF_WorkOrderForOutput_SpreadingFabric_AddName] DEFAULT (('')) NOT NULL,
    CONSTRAINT [PK_WorkOrderForOutput_SpreadingFabric] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為手動新增的布
0 = 不是 (代表有發料單來源)
1 = 是 (代表所以布捲資訊皆為人工維護)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput_SpreadingFabric', @level2type = N'COLUMN', @level2name = N'InsertByManual';
