CREATE TABLE [dbo].[WorkOrderForOutput] (
    [Ukey]                     INT             IDENTITY (1, 1) NOT NULL,
    [ID]                       VARCHAR (13)    CONSTRAINT [DF_WorkOrderForOutput_ID] DEFAULT ('') NOT NULL,
    [FactoryID]                VARCHAR (8)     CONSTRAINT [DF_WorkOrderForOutput_FactoryID] DEFAULT ('') NOT NULL,
    [MDivisionID]              VARCHAR (8)     CONSTRAINT [DF_WorkOrderForOutput_MDivisionID] DEFAULT ('') NOT NULL,
    [Seq1]                     VARCHAR (3)     CONSTRAINT [DF_WorkOrderForOutput_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]                     VARCHAR (2)     CONSTRAINT [DF_WorkOrderForOutput_Seq2] DEFAULT ('') NOT NULL,
    [CutRef]                   VARCHAR (10)    CONSTRAINT [DF_WorkOrderForOutput_CutRef] DEFAULT ('') NOT NULL,
    [CutNo]                    INT             NULL,
    [OrderID]                  VARCHAR (13)    CONSTRAINT [DF_WorkOrderForOutput_OrderID] DEFAULT ('') NOT NULL,
    [RefNo]                    VARCHAR (36)    CONSTRAINT [DF_WorkOrderForOutput_RefNo] DEFAULT ('') NOT NULL,
    [SCIRefNo]                 VARCHAR (30)    CONSTRAINT [DF_WorkOrderForOutput_SCIRefNo] DEFAULT ('') NOT NULL,
    [ColorID]                  VARCHAR (6)     CONSTRAINT [DF_WorkOrderForOutput_ColorID] DEFAULT ('') NOT NULL,
    [Tone]                     VARCHAR (15)    CONSTRAINT [DF_WorkOrderForOutput_Tone] DEFAULT ('') NOT NULL,
    [Layer]                    INT             CONSTRAINT [DF_WorkOrderForOutput_Layer] DEFAULT ((0)) NOT NULL,
    [WKETA]                    DATE            NULL,
    [FabricCombo]              VARCHAR (2)     CONSTRAINT [DF_WorkOrderForOutput_FabricCombo] DEFAULT ('') NOT NULL,
    [FabricCode]               VARCHAR (3)     CONSTRAINT [DF_WorkOrderForOutput_FabricCode] DEFAULT ('') NOT NULL,
    [FabricPanelCode]          VARCHAR (2)     CONSTRAINT [DF_WorkOrderForOutput_FabricPanelCode] DEFAULT ('') NOT NULL,
    [EstCutDate]               DATE            NULL,
    [ConsPC]                   NUMERIC (12, 4) CONSTRAINT [DF_WorkOrderForOutput_ConsPC] DEFAULT ((0)) NOT NULL,
    [Cons]                     NUMERIC (16, 4) CONSTRAINT [DF_WorkOrderForOutput_Cons] DEFAULT ((0)) NOT NULL,
    [MarkerNo]                 VARCHAR (10)    CONSTRAINT [DF_WorkOrderForOutput_MarkerNo] DEFAULT ('') NOT NULL,
    [MarkerName]               VARCHAR (20)    CONSTRAINT [DF_WorkOrderForOutput_MarkerName] DEFAULT ('') NOT NULL,
    [MarkerLength]             VARCHAR (15)    CONSTRAINT [DF_WorkOrderForOutput_MarkerLength] DEFAULT ('') NOT NULL,
    [ActCuttingPerimeter]      NVARCHAR (15)   CONSTRAINT [DF_WorkOrderForOutput_ActCuttingPerimeter] DEFAULT ('') NOT NULL,
    [StraightLength]           VARCHAR (15)    CONSTRAINT [DF_WorkOrderForOutput_StraightLength] DEFAULT ('') NOT NULL,
    [CurvedLength]             VARCHAR (15)    CONSTRAINT [DF_WorkOrderForOutput_CurvedLength] DEFAULT ('') NOT NULL,
    [Shift]                    VARCHAR (1)     CONSTRAINT [DF_WorkOrderForOutput_Shift] DEFAULT ('') NOT NULL,
    [CutCellID]                VARCHAR (2)     CONSTRAINT [DF_WorkOrderForOutput_CutCellID] DEFAULT ('') NOT NULL,
    [SpreadingNoID]            VARCHAR (5)     CONSTRAINT [DF_WorkOrderForOutput_SpreadingNoID] DEFAULT ('') NOT NULL,
    [UnfinishedCuttingReason]  VARCHAR (50)    CONSTRAINT [DF_WorkOrderForOutput_UnfinishedCuttingReason] DEFAULT ('') NOT NULL,
    [UnfinishedCuttingRemark]  NVARCHAR (500)  CONSTRAINT [DF_WorkOrderForOutput_UnfinishedCuttingRemark] DEFAULT ('') NOT NULL,
    [IsCreateByUser]           BIT             CONSTRAINT [DF_WorkOrderForOutput_IsCreateByUser] DEFAULT ((0)) NOT NULL,
    [SpreadingStatus]          VARCHAR (10)    CONSTRAINT [DF_WorkOrderForOutput_SpreadingStatus] DEFAULT ('Ready') NOT NULL,
    [SpreadingRemark]          NVARCHAR (500)  CONSTRAINT [DF_WorkOrderForOutput_SpreadingRemark] DEFAULT ('') NOT NULL,
    [GroupID]                  VARCHAR (13)    CONSTRAINT [DF_WorkOrderForOutput_GroupID] DEFAULT ('') NOT NULL,
    [WorkOrderForPlanningUkey] INT             CONSTRAINT [DF_WorkOrderForOutput_WorkOrderForPlanningUkey] DEFAULT ((0)) NOT NULL,
    [SourceFrom]               VARCHAR (1)     CONSTRAINT [DF_WorkOrderForOutput_SourceFrom] DEFAULT ('') NOT NULL,
    [CuttingMethod]            BIT             CONSTRAINT [DF_WorkOrderForOutput_CuttingMethod] DEFAULT ((0)) NOT NULL,
    [Order_EachconsUkey]       BIGINT          CONSTRAINT [DF_WorkOrderForOutput_Order_EachconsUkey] DEFAULT ((0)) NOT NULL,
    [AddName]                  VARCHAR (10)    CONSTRAINT [DF_WorkOrderForOutput_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                  DATETIME        NULL,
    [EditName]                 VARCHAR (10)    CONSTRAINT [DF_WorkOrderForOutput_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                 DATETIME        NULL,
    [MarkerVersion]            VARCHAR (3)     CONSTRAINT [DF_WorkOrderForOutput_MarkerVersion] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_WorkOrderForOutput] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_EstCutDate_CutCellid]
    ON [dbo].[WorkOrderForOutput]([EstCutDate] ASC, [CutCellID] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_EstCutDate]
    ON [dbo].[WorkOrderForOutput]([EstCutDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_BundleESCDate]
    ON [dbo].[WorkOrderForOutput]([ID] ASC, [MDivisionID] ASC, [CutRef] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_MDivisionId]
    ON [dbo].[WorkOrderForOutput]([MDivisionID] ASC)
    INCLUDE([CutRef]);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_OrderID]
    ON [dbo].[WorkOrderForOutput]([OrderID] ASC)
    INCLUDE([CutRef], [CutNo], [EstCutDate], [Ukey]);


GO
CREATE NONCLUSTERED INDEX [IDX_WorkOrderForOutput_CutRef]
    ON [dbo].[WorkOrderForOutput]([CutRef] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'MarkerVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單用料主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Order_EachconsUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手裁 Manual (0) 還是機裁 Auto (1)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'CuttingMethod';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1>Cutting_P02. WorkOrder For Planning
2>Cutting_P09. WorkOrder For Output
3>M360_Digital Spreading', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'SourceFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrderForPlanning主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'WorkOrderForPlanningUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Digital Spreading 合併與拆分的群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'GroupID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉布人員在拉布時的備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'SpreadingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ready,Spreading,Finished
	用於M360 Digital Spreading', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'SpreadingStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否由使用者建立', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'IsCreateByUser';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'未裁剪註記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'UnfinishedCuttingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'未裁剪原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'UnfinishedCuttingReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉布桌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'SpreadingNoID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁桌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'CutCellID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Shift';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克曲線長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'CurvedLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克直徑長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'StraightLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁剪碼長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'ActCuttingPerimeter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'MarkerLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'MarkerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'EstCutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布料種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'FabricCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船表預計到達時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'WKETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tone色差', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Tone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'SCIRefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'CutNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutput', @level2type = N'COLUMN', @level2name = N'Ukey';

