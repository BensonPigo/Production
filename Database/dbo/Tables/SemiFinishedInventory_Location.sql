CREATE TABLE [dbo].[SemiFinishedInventory_Location] (
    [POID]          VARCHAR (13) NOT NULL,
    [Roll]          VARCHAR (8)  NOT NULL,
    [Dyelot]        VARCHAR (8)  NOT NULL,
    [StockType]     CHAR (1)     CONSTRAINT [DF_SemiFinishedInventory_Location_StockType] DEFAULT ('') NOT NULL,
    [MtlLocationID] VARCHAR (20) NOT NULL,
    [Seq]           VARCHAR (6)  DEFAULT ('') NOT NULL,
    [Tone]          VARCHAR (8)  DEFAULT ('') NOT NULL
);



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory_Location', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory_Location', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory_Location', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory_Location', @level2type = N'COLUMN', @level2name = N'MtlLocationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory_Location', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品色 Tone', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory_Location', @level2type = N'COLUMN', @level2name = N'Tone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'半成品項次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SemiFinishedInventory_Location', @level2type = N'COLUMN', @level2name = N'Seq';

