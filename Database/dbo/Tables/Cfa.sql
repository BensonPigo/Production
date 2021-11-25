CREATE TABLE [dbo].[Cfa] (
    [ID]            VARCHAR (13)  CONSTRAINT [DF_Cfa_ID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13)  CONSTRAINT [DF_Cfa_OrderID] DEFAULT ('') NULL,
    [cDate]         DATE          NULL,
    [FactoryID]     VARCHAR (8)   CONSTRAINT [DF_Cfa_FactoryID] DEFAULT ('') NULL,
    [SewingLineID]  VARCHAR (5)   CONSTRAINT [DF_Cfa_SewingLineID] DEFAULT ('') NULL,
    [InspectQty]    NUMERIC (7)   CONSTRAINT [DF_Cfa_InspectQty] DEFAULT ((0)) NULL,
    [DefectQty]     NUMERIC (7)   CONSTRAINT [DF_Cfa_DefectQty] DEFAULT ((0)) NULL,
    [Team]          VARCHAR (5)   CONSTRAINT [DF_Cfa_Team] DEFAULT ('') NULL,
    [Shift]         VARCHAR (1)   CONSTRAINT [DF_Cfa_Shift] DEFAULT ('') NULL,
    [Result]        VARCHAR (5)   CONSTRAINT [DF_Cfa_Result] DEFAULT ('') NULL,
    [GarmentOutput] NUMERIC (7)   CONSTRAINT [DF_Cfa_GarmentOutput] DEFAULT ((0)) NULL,
    [Stage]         VARCHAR (1)   CONSTRAINT [DF_Cfa_Stage] DEFAULT ('') NULL,
    [CFA]           VARCHAR (10)  CONSTRAINT [DF_Cfa_CFA] DEFAULT ('') NULL,
    [Remark]        NVARCHAR (60) CONSTRAINT [DF_Cfa_Remark] DEFAULT ('') NULL,
    [Status]        VARCHAR (15)  CONSTRAINT [DF_Cfa_Status] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_Cfa_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_Cfa_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [MDivisionid]   VARCHAR (8)   CONSTRAINT [DF_Cfa_MDivisionid] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Cfa] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA InLine Record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'cDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'InspectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'汙損數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'DefectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'Team';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'Shift';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成衣Output % 數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'GarmentOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'階段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'Stage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'CFA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-CFAOrderID]
    ON [dbo].[Cfa]([OrderID] ASC);

