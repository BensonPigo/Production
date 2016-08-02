CREATE TABLE [dbo].[FSRCpuCost] (
    [ShipperID] VARCHAR (8)  CONSTRAINT [DF_FSRCpuCost_ShipperID] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME     NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_FSRCpuCost_AddName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_FSRCpuCost_EditName] DEFAULT ('') NULL, 
    CONSTRAINT [PK_FSRCpuCost] PRIMARY KEY ([ShipperID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'First Sale Rule-CpuCost', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost', @level2type = N'COLUMN', @level2name = N'ShipperID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost', @level2type = N'COLUMN', @level2name = N'EditName';

