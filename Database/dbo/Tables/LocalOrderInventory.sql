CREATE TABLE [dbo].[LocalOrderInventory]
(
	[POID]              VARCHAR(13)                                                                             NOT NULL, 
    [Seq1]              VARCHAR(3)                                                                              NOT NULL, 
    [Seq2]              VARCHAR(2)                                                                              NOT NULL, 
    [Roll]              VARCHAR(8)                                                                              NOT NULL, 
    [Dyelot]            VARCHAR(8)                                                                              NOT NULL, 
    [StockType]         CHAR(1)                                                                                 NOT NULL, 
    [InQty]             NUMERIC(11, 2)      CONSTRAINT [DF_LocalOrderInventory_InQty]           DEFAULT ((0))   NOT NULL, 
    [OutQty]            NUMERIC(11, 2)      CONSTRAINT [DF_LocalOrderInventory_OutQty]          DEFAULT ((0))   NOT NULL, 
    [AdjustQty]         NUMERIC(11, 2)      CONSTRAINT [DF_LocalOrderInventory_AdjustQty]       DEFAULT ((0))   NOT NULL,
    [Barcode]           VARCHAR(255)        CONSTRAINT [DF_LocalOrderInventory_Barcode]         DEFAULT ((''))  NOT NULL, 
    [BarcodeSeq]        VARCHAR(10)         CONSTRAINT [DF_LocalOrderInventory_BarcodeSeq]      DEFAULT ((''))  NOT NULL, 
    [ContainerCode]     NVARCHAR(100)       CONSTRAINT [DF_LocalOrderInventory_ContainerCode]   DEFAULT ((''))  NOT NULL, 
    [Tone]              VARCHAR(8)          CONSTRAINT [DF_LocalOrderInventory_Tone]            DEFAULT ((''))  NOT NULL, 
    [Ukey]              BIGINT                                                                  IDENTITY(1,1)   NOT NULL,   
    CONSTRAINT [PK_LocalOrderInventory] PRIMARY KEY ([StockType], [POID], [Seq1], [Seq2], [Roll], [Dyelot])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'捲號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'Roll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'Dyelot'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'StockType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'收料量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'InQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發出量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'OutQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'AdjustQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'主料條碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'Barcode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'主料條碼流水號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'BarcodeSeq'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'鐵框號 ( 主要針對主料 )',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'ContainerCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'色 Tone',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'Tone'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'流水號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderInventory',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'