CREATE TABLE [dbo].[FtyInventory] (
    [Category]  VARCHAR (1)     CONSTRAINT [DF_FtyInventory_Category] DEFAULT ('') NULL,
    [Poid]      VARCHAR (13)    CONSTRAINT [DF_FtyInventory_Poid] DEFAULT ('') NOT NULL,
    [Seq1]      VARCHAR (3)     CONSTRAINT [DF_FtyInventory_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]      VARCHAR (2)     CONSTRAINT [DF_FtyInventory_Seq2] DEFAULT ('') NOT NULL,
    [Roll]      VARCHAR (8)     CONSTRAINT [DF_FtyInventory_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (4)     CONSTRAINT [DF_FtyInventory_Dyelot] DEFAULT ('') NULL,
    [StockType] VARCHAR (1)     CONSTRAINT [DF_FtyInventory_StockType] DEFAULT ('') NOT NULL,
    [InQty]     NUMERIC (10, 2) CONSTRAINT [DF_FtyInventory_InQty] DEFAULT ((0)) NULL,
    [OutQty]    NUMERIC (10, 2) CONSTRAINT [DF_FtyInventory_OutQty] DEFAULT ((0)) NULL,
    [AdjustQty] NUMERIC (10, 2) CONSTRAINT [DF_FtyInventory_AdjustQty] DEFAULT ((0)) NULL,
    [LockName]  VARCHAR (10)    CONSTRAINT [DF_FtyInventory_LockName] DEFAULT ('') NULL,
    [LockDate]  DATETIME        NULL,
    [Lock]      BIT             CONSTRAINT [DF_FtyInventory_Lock] DEFAULT ((0)) NULL,
    [Ukey]      BIGINT          IDENTITY (1, 1) NOT NULL,
    [ZoneID] VARCHAR(3) NULL, 
    CONSTRAINT [PK_FtyInventory] PRIMARY KEY CLUSTERED ([Poid] ASC, [Seq1] ASC, [Seq2] ASC, [Roll] ASC, [StockType] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠庫存', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Category', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Poid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'卷號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'InQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發出量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'OutQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'AdjustQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖定人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'LockName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖定日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'LockDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖定狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Lock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Zone',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FtyInventory',
    @level2type = N'COLUMN',
    @level2name = N'ZoneID'