CREATE TABLE [dbo].[FtyInventory] (
    [Ukey]                  BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [MDivisionPoDetailUkey] BIGINT          NULL,
    [POID]                  VARCHAR (13)    CONSTRAINT [DF_FtyInventory_POID] DEFAULT ('') NOT NULL,
    [Seq1]                  VARCHAR (3)     CONSTRAINT [DF_FtyInventory_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]                  VARCHAR (2)     CONSTRAINT [DF_FtyInventory_Seq2] DEFAULT ('') NOT NULL,
    [Roll]                  VARCHAR (8)     CONSTRAINT [DF_FtyInventory_Roll] DEFAULT ('') NULL,
    [StockType]             CHAR (1)        CONSTRAINT [DF_FtyInventory_StockType] DEFAULT ('') NOT NULL,
    [Dyelot]                VARCHAR (8)     CONSTRAINT [DF_FtyInventory_Dyelot] DEFAULT ('') NULL,
    [InQty]                 NUMERIC (11, 2) CONSTRAINT [DF_FtyInventory_InQty] DEFAULT ((0)) NULL,
    [OutQty]                NUMERIC (11, 2) CONSTRAINT [DF_FtyInventory_OutQty] DEFAULT ((0)) NULL,
    [AdjustQty]             NUMERIC (11, 2) CONSTRAINT [DF_FtyInventory_AdjustQty] DEFAULT ((0)) NULL,
    [LockName]              VARCHAR (10)    CONSTRAINT [DF_FtyInventory_LockName] DEFAULT ('') NULL,
    [LockDate]              DATETIME        NULL,
    [Lock]                  BIT             CONSTRAINT [DF_FtyInventory_Lock] DEFAULT ((0)) NULL,
    [Remark]                NVARCHAR (500)  CONSTRAINT [DF_FtyInventory_Remark] DEFAULT ('') NULL,
    [Barcode] VARCHAR(16) NULL DEFAULT (''), 
    [WMSLock] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_FtyInventory] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠庫存', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyInventory';


GO



GO



GO



GO



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
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[FtyInventory]([Roll] ASC, [StockType] ASC)
    INCLUDE([InQty], [OutQty], [AdjustQty]);


GO
CREATE NONCLUSTERED INDEX [MdID_POSeq]
    ON [dbo].[FtyInventory]([POID] ASC, [Seq1] ASC, [Seq2] ASC, [StockType] ASC)
    INCLUDE([Ukey]);










GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20180413-173045]
    ON [dbo].[FtyInventory]([MDivisionPoDetailUkey] ASC);

