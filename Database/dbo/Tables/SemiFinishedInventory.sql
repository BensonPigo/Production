CREATE TABLE [dbo].[SemiFinishedInventory] (
    [POID]      VARCHAR (13)    NOT NULL,
    [Roll]      VARCHAR (8)     NOT NULL,
    [Dyelot]    VARCHAR (8)     NOT NULL,
    [StockType] CHAR (1)        CONSTRAINT [DF_SemiFinishedInventory_StockType] DEFAULT ('') NOT NULL,
    [InQty]     NUMERIC (11, 2) CONSTRAINT [DF_SemiFinishedInventory_InQty] DEFAULT ((0)) NOT NULL,
    [OutQty]    NUMERIC (11, 2) CONSTRAINT [DF_SemiFinishedInventory_OutQty] DEFAULT ((0)) NOT NULL,
    [AdjustQty] NUMERIC (11, 2) CONSTRAINT [DF_SemiFinishedInventory_AdjustQty] DEFAULT ((0)) NOT NULL,
    [Seq]       VARCHAR (6)     DEFAULT ('') NOT NULL,
    [Tone]      VARCHAR (8)     DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SemiFinishedInventory] PRIMARY KEY CLUSTERED ([POID] ASC, [Seq] ASC, [Roll] ASC, [Dyelot] ASC, [Tone] ASC, [StockType] ASC)
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發出量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'OutQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'InQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'AdjustQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品色 Tone', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'Tone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品項次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory', @level2type = N'COLUMN', @level2name = N'Seq';

