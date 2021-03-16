CREATE TABLE [dbo].[FtyExport_Detail] (
    [ID]         VARCHAR (13)    CONSTRAINT [DF_FtyExport_Detail_ID] DEFAULT ('') NOT NULL,
    [POID]       VARCHAR (13)    CONSTRAINT [DF_FtyExport_Detail_POID] DEFAULT ('') NOT NULL,
    [Seq1]       VARCHAR (3)     CONSTRAINT [DF_FtyExport_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]       VARCHAR (2)     CONSTRAINT [DF_FtyExport_Detail_Seq2] DEFAULT ('') NOT NULL,
    [SCIRefno]   VARCHAR (30)    CONSTRAINT [DF_FtyExport_Detail_SCIRefno] DEFAULT ('') NOT NULL,
    [RefNo]      VARCHAR (21)    CONSTRAINT [DF_FtyExport_Detail_RefNo] DEFAULT ('') NOT NULL,
    [SuppID]     VARCHAR (8)     CONSTRAINT [DF_FtyExport_Detail_SuppNo] DEFAULT ('') NULL,
    [FabricType] VARCHAR (1)     CONSTRAINT [DF_FtyExport_Detail_Type] DEFAULT ('') NULL,
    [MtlTypeID]  VARCHAR (20)    CONSTRAINT [DF_FtyExport_Detail_MtlTypeID] DEFAULT ('') NULL,
    [Qty]        NUMERIC (8, 2)  CONSTRAINT [DF_FtyExport_Detail_Qty] DEFAULT ((0)) NULL,
    [UnitID]     VARCHAR (8)     CONSTRAINT [DF_FtyExport_Detail_UnitID] DEFAULT ('') NULL,
    [NetKg]      NUMERIC (7, 2)  CONSTRAINT [DF_FtyExport_Detail_NetKg] DEFAULT ((0)) NULL,
    [WeightKg]   NUMERIC (7, 2)  CONSTRAINT [DF_FtyExport_Detail_WeightKg] DEFAULT ((0)) NULL,
    [LocalPOID]  VARCHAR (13)    CONSTRAINT [DF_FtyExport_Detail_LocalPOID] DEFAULT ('') NOT NULL,
    [Price]      NUMERIC (12, 4) CONSTRAINT [DF_FtyExport_Detail_LocalPO_DetailUKey] DEFAULT ((0)) NULL,
    [CurrencyId] VARCHAR (3)     CONSTRAINT [DF_FtyExport_Detail_CurrencyId] DEFAULT ('') NULL,
    [OldFabricUkey] VARCHAR(10) NULL DEFAULT (''), 
    [OldFabricVer] VARCHAR(2) NULL DEFAULT (''), 
	TransactionID varchar(13) NOT NULL CONSTRAINT [DF_FtyExport_Detail_TransactionID] DEFAULT(''),
    CONSTRAINT [PK_FtyExport_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [POID] ASC, [Seq1] ASC, [Seq2] ASC, [SCIRefno] ASC, [RefNo] ASC, [LocalPOID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠之物料進出口工作底稿-表身', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Working No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'RefNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'MtlTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'本次出口數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'N.W.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'G.W.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport_Detail', @level2type = N'COLUMN', @level2name = N'WeightKg';

