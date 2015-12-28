﻿CREATE TABLE [dbo].[WorkOrder] (
    [ID]            VARCHAR (13)   CONSTRAINT [DF_WorkOrder_ID] DEFAULT ('') NOT NULL,
    [FactoryID]     VARCHAR (8)    CONSTRAINT [DF_WorkOrder_FactoryID] DEFAULT ('') NOT NULL,
    [SEQ1]          VARCHAR (3)    CONSTRAINT [DF_WorkOrder_SEQ] DEFAULT ('') NOT NULL,
    [SEQ2]          VARCHAR (2)    NOT NULL,
    [CutRef]        VARCHAR (6)    CONSTRAINT [DF_WorkOrder_CutRef] DEFAULT ('') NULL,
    [OrderID]       VARCHAR (13)   CONSTRAINT [DF_WorkOrder_OrderID] DEFAULT ('') NULL,
    [CutplanID]     VARCHAR (13)   CONSTRAINT [DF_WorkOrder_CutplanID] DEFAULT ('') NULL,
    [Cutno]         NUMERIC (3)    CONSTRAINT [DF_WorkOrder_Cutno] DEFAULT ((0)) NULL,
    [Layer]         NUMERIC (5)    CONSTRAINT [DF_WorkOrder_Layer] DEFAULT ((0)) NULL,
    [Colorid]       VARCHAR (6)    CONSTRAINT [DF_WorkOrder_Colorid] DEFAULT ('') NULL,
    [Markername]    VARCHAR (5)    CONSTRAINT [DF_WorkOrder_Markername] DEFAULT ('') NULL,
    [EstCutDate]    DATE           NULL,
    [Cell]          VARCHAR (2)    CONSTRAINT [DF_WorkOrder_Cell] DEFAULT ('') NULL,
    [MarkerLength]  VARCHAR (13)   CONSTRAINT [DF_WorkOrder_MarkerLength] DEFAULT ('') NULL,
    [ConsPC]        NUMERIC (6, 4) CONSTRAINT [DF_WorkOrder_ConsPC] DEFAULT ((0)) NULL,
    [Cons]          NUMERIC (9, 4) CONSTRAINT [DF_WorkOrder_Cons] DEFAULT ((0)) NULL,
    [Refno]         VARCHAR (20)   CONSTRAINT [DF_WorkOrder_Refno] DEFAULT ('') NULL,
    [SCIRefno]      VARCHAR (26)   CONSTRAINT [DF_WorkOrder_SCIRefno] DEFAULT ('') NULL,
    [MarkerNo]      VARCHAR (10)   CONSTRAINT [DF_WorkOrder_MarkerNo] DEFAULT ('') NULL,
    [MarkerVersion] VARCHAR (3)    CONSTRAINT [DF_WorkOrder_MarkerVersion] DEFAULT ('') NULL,
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Type]          VARCHAR (1)    CONSTRAINT [DF_WorkOrder_Type] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_WorkOrder_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_WorkOrder_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME       NULL,
    [FabricCombo]   VARCHAR (2)    CONSTRAINT [DF_WorkOrder_FabricCombo] DEFAULT ('') NULL,
    [MDivisionId]   VARCHAR (8)    CONSTRAINT [DF_WorkOrder_MDivisionId] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cut Ref', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁簡計劃單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'CutplanID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Cutno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Colorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Markername';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計裁剪日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'EstCutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Cell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'MarkerLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Cons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI 物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'MarkerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'MarkerVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrder 來源方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'FabricCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrder', @level2type = N'COLUMN', @level2name = N'SEQ1';

