CREATE TABLE [dbo].[CuttingOutput_WIP] (
    [OrderID] VARCHAR (13)    CONSTRAINT [DF_CuttingOutput_WIP_OrderID] DEFAULT ('') NOT NULL,
    [Article] VARCHAR (8)     CONSTRAINT [DF_CuttingOutput_WIP_Article] DEFAULT ('') NOT NULL,
    [Size]    VARCHAR (8)     CONSTRAINT [DF_CuttingOutput_WIP_Size] DEFAULT ('') NOT NULL,
    [Qty]     NUMERIC (10, 2) CONSTRAINT [DF_CuttingOutput_WIP_Qty] DEFAULT ((0)) NULL,
    [EditDate] DATE NULL, 
    CONSTRAINT [PK_CuttingOutput_WIP] PRIMARY KEY CLUSTERED ([OrderID] ASC, [Article] ASC, [Size] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Daily Output(WIP)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_WIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_WIP', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_WIP', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_WIP', @level2type = N'COLUMN', @level2name = N'Size';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput_WIP', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WIP變更日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CuttingOutput_WIP',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'