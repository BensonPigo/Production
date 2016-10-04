CREATE TABLE [dbo].[MDivisionPoDetail] (
    [MDivisionID] VARCHAR (8)     CONSTRAINT [DF_MDivisionPoDetail_MDivisionId] DEFAULT ('') NOT NULL,
    [POID]        VARCHAR (13)    CONSTRAINT [DF_MDivisionPoDetail_POID] DEFAULT ('') NOT NULL,
    [Seq1]        VARCHAR (3)     CONSTRAINT [DF_MDivisionPoDetail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]        VARCHAR (2)     CONSTRAINT [DF_MDivisionPoDetail_Seq2] DEFAULT ('') NOT NULL,
    [InQty]       NUMERIC (10, 2) CONSTRAINT [DF_MDivisionPoDetail_InQty] DEFAULT ((0)) NULL,
    [OutQty]      NUMERIC (10, 2) CONSTRAINT [DF_MDivisionPoDetail_OutQty] DEFAULT ((0)) NULL,
    [AdjustQty]   NUMERIC (10, 2) CONSTRAINT [DF_MDivisionPoDetail_AdjustQty] DEFAULT ((0)) NULL,
    [LInvQty]     NUMERIC (10, 2) CONSTRAINT [DF_MDivisionPoDetail_LInvQty] DEFAULT ((0)) NULL,
    [LObQty]      NUMERIC (10, 2) CONSTRAINT [DF_MDivisionPoDetail_LObQty] DEFAULT ((0)) NULL,
    [ALocation]   VARCHAR (100)   CONSTRAINT [DF_MDivisionPoDetail_ALocation] DEFAULT ('') NULL,
    [BLocation]   VARCHAR (100)   CONSTRAINT [DF_MDivisionPoDetail_BLocation] DEFAULT ('') NULL,
    [Ukey]        BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK__MDivisio__07AF6F03481FB32F] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

















GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MDivision',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MDivisionPoDetail',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PO#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MDivisionPoDetail',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Seq1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MDivisionPoDetail',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Seq2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MDivisionPoDetail',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出庫數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivisionPoDetail', @level2type = N'COLUMN', @level2name = N'OutQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'入庫數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivisionPoDetail', @level2type = N'COLUMN', @level2name = N'InQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'B倉儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivisionPoDetail', @level2type = N'COLUMN', @level2name = N'BLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'A倉儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivisionPoDetail', @level2type = N'COLUMN', @level2name = N'ALocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivisionPoDetail', @level2type = N'COLUMN', @level2name = N'AdjustQty';


GO



GO



GO
CREATE NONCLUSTERED INDEX [POID]
    ON [dbo].[MDivisionPoDetail]([POID] ASC);


GO
CREATE NONCLUSTERED INDEX [MDID_POID]
    ON [dbo].[MDivisionPoDetail]([MDivisionID] ASC, [POID] ASC);


GO
CREATE NONCLUSTERED INDEX [MDID]
    ON [dbo].[MDivisionPoDetail]([MDivisionID] ASC)
    INCLUDE([POID], [Seq1], [Seq2]);

