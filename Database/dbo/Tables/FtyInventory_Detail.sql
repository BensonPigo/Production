CREATE TABLE [dbo].[FtyInventory_Detail] (
    [Ukey]          BIGINT       CONSTRAINT [DF_FtyInventory_Detail_Ukey] DEFAULT ((0)) NOT NULL,
    [MtlLocationID] VARCHAR (10) CONSTRAINT [DF_FtyInventory_Detail_MtlLocationID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FtyInventory_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC, [MtlLocationID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料儲位資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory_Detail', @level2type = N'COLUMN', @level2name = N'MtlLocationID';

