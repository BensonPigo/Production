CREATE TABLE [dbo].[ShipPlan] (
    [ID]          VARCHAR (13)   CONSTRAINT [DF_ShipPlan_ID] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR (8)    CONSTRAINT [DF_ShipPlan_MDivisionID] DEFAULT ('') NOT NULL,
    [CDate]       DATE           NOT NULL,
    [Remark]      NVARCHAR (MAX) CONSTRAINT [DF_ShipPlan_Remark] DEFAULT ('') NULL,
    [CFMDate]     DATE           NULL,
    [Status]      VARCHAR (15)   CONSTRAINT [DF_ShipPlan_Status] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_ShipPlan_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_ShipPlan_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [OrderCompanyID] NUMERIC(2, 0)      CONSTRAINT [DF_ShipPlan_OrderCompanyID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ShipPlan] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ship Plan', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'CDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Confirm Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'CFMDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'MDivisionID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipPlan', @level2type = N'COLUMN', @level2name = N'OrderCompanyID';


GO