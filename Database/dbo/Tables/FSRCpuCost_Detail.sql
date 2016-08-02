CREATE TABLE [dbo].[FSRCpuCost_Detail] (
    [ShipperID] VARCHAR (8)    CONSTRAINT [DF_FSRCpuCost_Detail_ShipperID] DEFAULT ('') NOT NULL,
    [BeginDate] DATE           NOT NULL,
    [EndDate]   DATE           NOT NULL,
    [CpuCost]   NUMERIC (5, 3) CONSTRAINT [DF_FSRCpuCost_Detail_CpuCost] DEFAULT ((0)) NOT NULL,
    [AddDate]   DATETIME       NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_FSRCpuCost_Detail_AddName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_FSRCpuCost_Detail_EditName] DEFAULT ('') NULL, 
    CONSTRAINT [PK_FSRCpuCost_Detail] PRIMARY KEY ([ShipperID], [BeginDate], [EndDate])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'First Sale Rule-CPUCost Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'ShipperID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起迄日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'BeginDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'迄止日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'EndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'K值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'CpuCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FSRCpuCost_Detail', @level2type = N'COLUMN', @level2name = N'EditName';

