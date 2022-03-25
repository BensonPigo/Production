CREATE TABLE [dbo].[WHBarcodeTransaction] (
    [Function]                    VARCHAR (4)   NOT NULL,
    [TransactionID]               VARCHAR (13)  NOT NULL,
    [TransactionUkey]             BIGINT        NOT NULL,
    [Action]                      VARCHAR (10)  NOT NULL,
    [FromFabric_FtyInventoryUkey] BIGINT        NULL,
    [From_OldBarcode]             VARCHAR (255) NOT NULL,
    [From_OldBarcodeSeq]          VARCHAR (2)   NOT NULL,
    [From_NewBarcode]             VARCHAR (255) NOT NULL,
    [From_NewBarcodeSeq]          VARCHAR (2)   NOT NULL,
    [ToFabric_FtyInventoryUkey]   BIGINT        NULL,
    [To_OldBarcode]               VARCHAR (255) NOT NULL,
    [To_OldBarcodeSeq]            VARCHAR (2)   NOT NULL,
    [To_NewBarcode]               VARCHAR (255) NOT NULL,
    [To_NewBarcodeSeq]            VARCHAR (2)   NOT NULL,
    [CommitTime]                  DATETIME      NULL,
    CONSTRAINT [PK_WHBarcodeTransaction] PRIMARY KEY CLUSTERED ([Function] ASC, [TransactionID] ASC, [TransactionUkey] ASC, [Action] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'執行日期及時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'CommitTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉入] 物料 [新] 的 Barcode Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'To_NewBarcodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉入] 物料 [新] 的 Barcode Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'To_NewBarcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉入] 物料 [舊] 的 Barcode Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'To_OldBarcodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉入] 物料 [舊] 的 Barcode Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'To_OldBarcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉入] 物料的 FtyInventoryUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'ToFabric_FtyInventoryUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉出] 物料 [新] 的 Barcode Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'From_NewBarcodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉出] 物料 [新] 的 Barcode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'From_NewBarcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉出] 物料 [舊] 的 Barcode Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'From_OldBarcodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉出] 物料 [舊] 的 Barcode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'From_OldBarcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'[轉出] 物料的 FtyInventoryUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'FromFabric_FtyInventoryUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Confirm, Delete, UnConfirm, Send, Recall', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'Action';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易紀錄明細的 Ukey
用 Ukey 主要是有些交易同一張單據會轉出同一個物料 2 次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'TransactionUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'TransactionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能代號e.g. P07, P08', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHBarcodeTransaction', @level2type = N'COLUMN', @level2name = N'Function';

