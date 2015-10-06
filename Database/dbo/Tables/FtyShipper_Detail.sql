CREATE TABLE [dbo].[FtyShipper_Detail] (
    [BrandID]   VARCHAR (8) CONSTRAINT [DF_FtyShipper_Detail_BrandID] DEFAULT ('') NOT NULL,
    [FactoryID] VARCHAR (8) CONSTRAINT [DF_FtyShipper_Detail_FactoryID] DEFAULT ('') NOT NULL,
    [BeginDate] DATE        NOT NULL,
    [EndDate]   DATE        NULL,
    [Shipper]   VARCHAR (8) CONSTRAINT [DF_FtyShipper_Detail_Shipper] DEFAULT ('') NULL,
    CONSTRAINT [PK_FtyShipper_Detail] PRIMARY KEY CLUSTERED ([BrandID] ASC, [FactoryID] ASC, [BeginDate] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment Shipper Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper_Detail', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper_Detail', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper_Detail', @level2type = N'COLUMN', @level2name = N'BeginDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'迄止日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper_Detail', @level2type = N'COLUMN', @level2name = N'EndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyShipper_Detail', @level2type = N'COLUMN', @level2name = N'Shipper';

