CREATE TABLE [dbo].[ChangeKPILETARequest] (
    [ID]                  VARCHAR (13)   DEFAULT ('') NOT NULL,
    [MdivisionID]         VARCHAR (8)    DEFAULT ('') NOT NULL,
    [OrderID]             VARCHAR (13)   DEFAULT ('') NOT NULL,
    [OldKPILETA]          DATE           NOT NULL,
    [NewKPILETA]          DATE           NOT NULL,
    [Status]              VARCHAR (15)   CONSTRAINT [DF_ChangeKPILETARequest_Status] DEFAULT ('') NOT NULL,
    [FactoryRemark]       NVARCHAR (500) CONSTRAINT [DF_ChangeKPILETARequest_FactoryRemark] DEFAULT ('') NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_ChangeKPILETARequest_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_ChangeKPILETARequest_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [ConfirmedNewKPILETA] DATE           NULL,
    [ApproveName]         VARCHAR (10)   CONSTRAINT [DF_ChangeKPILETARequest_ApproveName] DEFAULT ('') NOT NULL,
    [ApproveDate]         DATETIME       NULL,
    [ConfirmName]         VARCHAR (10)   CONSTRAINT [DF_ChangeKPILETARequest_ConfirmName] DEFAULT ('') NOT NULL,
    [ConfirmDate]         DATETIME       NULL,
    [TPERemark]           NVARCHAR (500) CONSTRAINT [DF_ChangeKPILETARequest_TPERemark] DEFAULT ('') NOT NULL,
    [TPEEditName]         VARCHAR (10)   CONSTRAINT [DF_ChangeKPILETARequest_TPEEditName] DEFAULT ('') NOT NULL,
    [TPEEditDate]         DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'申請單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'所屬M',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'MdivisionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'OrderID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'原始orders.KPILETA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'OldKPILETA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'欲改成orders.KPILETA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'NewKPILETA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠註記',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'FactoryRemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北確認人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'ConfirmName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北確認日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'ConfirmDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北註記',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'TPERemark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北修改日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChangeKPILETARequest',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditDate'