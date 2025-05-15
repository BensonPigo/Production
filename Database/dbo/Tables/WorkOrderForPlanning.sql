CREATE TABLE [dbo].[WorkOrderForPlanning] (
    [Ukey]               INT             IDENTITY (1, 1) NOT NULL,
    [Type]               VARCHAR (1)     CONSTRAINT [DF_WorkOrderForPlanning_Type] DEFAULT ('') NOT NULL,
    [ID]                 VARCHAR (13)    CONSTRAINT [DF_WorkOrderForPlanning_ID] DEFAULT ('') NOT NULL,
    [OrderID]            VARCHAR (13)    CONSTRAINT [DF_WorkOrderForPlanning_OrderID] DEFAULT ('') NOT NULL,
    [FactoryID]          VARCHAR (8)     CONSTRAINT [DF_WorkOrderForPlanning_FactoryID] DEFAULT ('') NOT NULL,
    [MDivisionID]        VARCHAR (8)     CONSTRAINT [DF_WorkOrderForPlanning_MDivisionID] DEFAULT ('') NOT NULL,
    [Seq1]               VARCHAR (3)     CONSTRAINT [DF_WorkOrderForPlanning_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]               VARCHAR (2)     CONSTRAINT [DF_WorkOrderForPlanning_Seq2] DEFAULT ('') NOT NULL,
    [CutRef]             VARCHAR (10)    CONSTRAINT [DF_WorkOrderForPlanning_CutRef] DEFAULT ('') NOT NULL,
    [Seq]                INT             NULL,
    [CutPlanID]          VARCHAR (13)    CONSTRAINT [DF_WorkOrderForPlanning_CutPlanID] DEFAULT ('') NOT NULL,
    [Layer]              INT             CONSTRAINT [DF_WorkOrderForPlanning_Layer] DEFAULT ((0)) NOT NULL,
    [WKETA]              DATE            NULL,
    [EstCutDate]         DATE            NULL,
    [FabricCombo]        VARCHAR (2)     CONSTRAINT [DF_WorkOrderForPlanning_FabricCombo] DEFAULT ('') NOT NULL,
    [FabricCode]         VARCHAR (3)     CONSTRAINT [DF_WorkOrderForPlanning_FabricCode] DEFAULT ('') NOT NULL,
    [FabricPanelCode]    VARCHAR (2)     CONSTRAINT [DF_WorkOrderForPlanning_FabricPanelCode] DEFAULT ('') NOT NULL,
    [RefNo]              VARCHAR (36)    CONSTRAINT [DF_WorkOrderForPlanning_RefNo] DEFAULT ('') NOT NULL,
    [SCIRefNo]           VARCHAR (30)    CONSTRAINT [DF_WorkOrderForPlanning_SCIRefNo] DEFAULT ('') NOT NULL,
    [ColorID]            VARCHAR (6)     CONSTRAINT [DF_WorkOrderForPlanning_ColorID] DEFAULT ('') NOT NULL,
    [ConsPC]             NUMERIC (12, 4) CONSTRAINT [DF_WorkOrderForPlanning_ConsPC] DEFAULT ((0)) NOT NULL,
    [Cons]               NUMERIC (16, 4) CONSTRAINT [DF_WorkOrderForPlanning_Cons] DEFAULT ((0)) NOT NULL,
    [Tone]               VARCHAR (15)    CONSTRAINT [DF_WorkOrderForPlanning_Tone] DEFAULT ('') NOT NULL,
    [Remark]             NVARCHAR (MAX)  CONSTRAINT [DF_WorkOrderForPlanning_Remark] DEFAULT ('') NOT NULL,
    [IsCreateByUser]     BIT             CONSTRAINT [DF_WorkOrderForPlanning_IsCreateByUser] DEFAULT ((0)) NOT NULL,
    [MarkerName]         VARCHAR (20)    CONSTRAINT [DF_WorkOrderForPlanning_MarkerName] DEFAULT ('') NOT NULL,
    [MarkerNo]           VARCHAR (10)    CONSTRAINT [DF_WorkOrderForPlanning_MarkerNo] DEFAULT ('') NOT NULL,
    [MarkerLength]       VARCHAR (15)    CONSTRAINT [DF_WorkOrderForPlanning_MarkerLength] DEFAULT ('') NOT NULL,
    [Order_EachconsUkey] BIGINT          CONSTRAINT [DF_WorkOrderForPlanning_Order_EachconsUkey] DEFAULT ((0)) NOT NULL,
	[MarkerVersion]      VARCHAR (3)     CONSTRAINT [DF_WorkOrderForPlanning_MarkerVersion] DEFAULT ('') NOT NULL,
    [CutCellID] VARCHAR(2) CONSTRAINT [DF_WorkOrderForPlanning_CutCellID] DEFAULT ('') NOT NULL,
    [AddName]            VARCHAR (10)    CONSTRAINT [DF_WorkOrderForPlanning_AddName] DEFAULT ('') NOT NULL,
    [AddDate]            DATETIME        NULL,
    [EditName]           VARCHAR (10)    CONSTRAINT [DF_WorkOrderForPlanning_EditName] DEFAULT ('') NOT NULL,
    [EditDate]           DATETIME        NULL,
    CONSTRAINT [PK_WorkOrderForPlanning] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


	Go

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Ukey'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用料轉置方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Type'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'ID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'OrderID'
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
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁次號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'Seq'
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
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克長' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'MarkerLength'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'馬克版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WorkOrderForPlanning', @level2type=N'COLUMN',@level2name=N'MarkerVersion'
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
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForPlanning_OrderID]
    ON [dbo].[WorkOrderForPlanning]([OrderID] ASC)
    INCLUDE([CutRef], [EstCutDate], [Ukey]);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForPlanning_MDivisionId]
    ON [dbo].[WorkOrderForPlanning]([MDivisionID] ASC)
    INCLUDE([CutRef]);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForPlanning_EstCutDate_CutCellid]
    ON [dbo].[WorkOrderForPlanning]([EstCutDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForPlanning_EstCutDate]
    ON [dbo].[WorkOrderForPlanning]([EstCutDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForPlanning_CutRef]
    ON [dbo].[WorkOrderForPlanning]([CutRef] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForPlanning_BundleESCDate]
    ON [dbo].[WorkOrderForPlanning]([ID] ASC, [MDivisionID] ASC, [CutRef] ASC);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁桌',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'WorkOrderForPlanning',
    @level2type = N'COLUMN',
    @level2name = N'CutCellID'