CREATE TABLE [dbo].[CutPlan_DetailDeletedHistory]
(
	[ID]                       VARCHAR(13)     CONSTRAINT [DF_CutPlan_DetailDeletedHistory_ID]                            DEFAULT ((''))      NOT NULL, 
    [Sewinglineid]             VARCHAR(5)      CONSTRAINT [DF_CutPlan_DetailDeletedHistory_Sewinglineid]                  DEFAULT ((''))      NOT NULL, 
    [CutRef]                   VARCHAR(10)     CONSTRAINT [DF_CutPlan_DetailDeletedHistory_CutRef]                        DEFAULT ((''))      NOT NULL, 
    [CutNo]                    NUMERIC(6)      CONSTRAINT [DF_CutPlan_DetailDeletedHistory_CutNo]                         DEFAULT ((0))       NOT NULL, 
    [OrderID]                  VARCHAR(13)     CONSTRAINT [DF_CutPlan_DetailDeletedHistory_OrderID]                       DEFAULT ((''))      NOT NULL, 
    [StyleID]                  VARCHAR(15)     CONSTRAINT [DF_CutPlan_DetailDeletedHistory_StyleID]                       DEFAULT ((''))      NOT NULL, 
    [Colorid]                  VARCHAR(6)      CONSTRAINT [DF_CutPlan_DetailDeletedHistory_Colorid]                       DEFAULT ((''))      NOT NULL, 
    [Cons]                     NUMERIC(8, 2)   CONSTRAINT [DF_CutPlan_DetailDeletedHistory_Cons]                          DEFAULT ((0))       NOT NULL, 
    [WorkOrderForPlanningUkey] BIGINT          CONSTRAINT [DF_CutPlan_DetailDeletedHistory_WorkOrderForPlanningUkey]      DEFAULT ((0))       NOT NULL, 
    [Remark]                   NVARCHAR(MAX)   CONSTRAINT [DF_CutPlan_DetailDeletedHistory_Remark]                        DEFAULT ((''))      NOT NULL, 
    [POID]                     VARCHAR(13)     CONSTRAINT [DF_CutPlan_DetailDeletedHistory_POID]                          DEFAULT ((''))      NOT NULL, 
    [Adddate]                  DATETIME        CONSTRAINT [DF_CutPlan_DetailDeletedHistory_dddate]                                                NULL
    CONSTRAINT [PK_CutPlan_DetailDeletedHistory] PRIMARY KEY ([ID],[WorkOrderForPlanningUkey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車縫產線號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'Sewinglineid'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Cut Refno',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'CutRef'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'裁次',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'CutNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'款示',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'StyleID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'顏色',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'Colorid'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'用量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'Cons'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'採購單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'刪除日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'Adddate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Planning裁剪工單主鍵',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CutPlan_DetailDeletedHistory',
    @level2type = N'COLUMN',
    @level2name = N'WorkOrderForPlanningUkey'
GO