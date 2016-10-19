CREATE TABLE [dbo].[CuttingTape_Detail] (
    [MDivisionID] VARCHAR (8)  NOT NULL,
    [POID]        VARCHAR (13) NOT NULL,
    [Seq1]        VARCHAR (3)  NOT NULL,
    [Seq2]        VARCHAR (2)  NOT NULL,
    [TapeInline]  DATE         NULL,
    [TapeOffline] DATE         NULL,
    CONSTRAINT [PK_CuttingTape_Detail] PRIMARY KEY CLUSTERED ([MDivisionID] ASC, [POID] ASC, [Seq1] ASC, [Seq2] ASC) 
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外裁明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape_Detail';

