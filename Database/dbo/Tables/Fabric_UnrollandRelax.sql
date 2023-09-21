CREATE TABLE [dbo].[Fabric_UnrollandRelax] (
    [Barcode]             VARCHAR (255)   NOT NULL,
    [POID]                VARCHAR (13)    DEFAULT ('') NULL,
    [Seq1]                VARCHAR (3)     DEFAULT ('') NULL,
    [Seq2]                VARCHAR (2)     DEFAULT ('') NULL,
    [Roll]                VARCHAR (8)     DEFAULT ('') NULL,
    [Dyelot]              VARCHAR (8)     DEFAULT ('') NULL,
    [StockType]           CHAR (1)        DEFAULT ('') NULL,
    [UnrollStatus]        VARCHAR (10)    DEFAULT ('') NULL,
    [UnrollStartTime]     DATETIME        NULL,
    [UnrollEndTime]       DATETIME        NULL,
    [RelaxationStartTime] DATETIME        NULL,
    [RelaxationEndTime]   DATETIME        NULL,
    [UnrollScanner]       VARCHAR (10)    DEFAULT ('') NULL,
    [UnrollActualQty]     NUMERIC (11, 2) DEFAULT ((0)) NULL,
    [UnrollRemark]        NVARCHAR (100)  DEFAULT ('') NULL,
    [IsAdvance] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Fabric_UnrollandRelax] PRIMARY KEY CLUSTERED ([Barcode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Unroll 階段備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'UnrollRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Unroll 階段實際收到的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'UnrollActualQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M360 Unroll 掃描 Unroll Location 的使用者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'UnrollScanner';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫鬆布 完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'RelaxationEndTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫鬆布 開始時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'RelaxationStartTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫攤開布捲 完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'UnrollEndTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫攤開布捲 開始時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'UnrollStartTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫攤開布捲狀態，可能值包括 Ongoing 或 Done', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'UnrollStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鬆布時對應的 StockType', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鬆布時對應的 Dyelot', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鬆布時對應的 Roll', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鬆布時對應的 Seq2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鬆布時對應的 Seq1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鬆布時對應的 POID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主料條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_UnrollandRelax', @level2type = N'COLUMN', @level2name = N'Barcode';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'判斷是否為提前鬆布',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Fabric_UnrollandRelax',
    @level2type = N'COLUMN',
    @level2name = N'IsAdvance'