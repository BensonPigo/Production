CREATE TABLE [dbo].[ZonePoDetail]
(
	[ZoneId] VARCHAR(3) NOT NULL CONSTRAINT [DF_ZonePoDetail_ZoneId] DEFAULT ('') , 
    [POID] VARCHAR(13) NOT NULL CONSTRAINT [DF_ZonePoDetail_POID] DEFAULT (''), 
    [Seq1] VARCHAR(3) NOT NULL CONSTRAINT [DF_ZonePoDetail_Seq1] DEFAULT (''), 
    [Seq2] VARCHAR(2) NOT NULL CONSTRAINT [DF_ZonePoDetail_Seq2] DEFAULT (''), 
    [TEST] BIGINT NOT NULL IDENTITY, 
    PRIMARY KEY ([Seq2], [ZoneId], [POID], [Seq1])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Zone',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ZonePoDetail',
    @level2type = N'COLUMN',
    @level2name = N'ZoneId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PO#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ZonePoDetail',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Seq1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ZonePoDetail',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Seq2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ZonePoDetail',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'