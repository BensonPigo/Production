CREATE TABLE [dbo].[RFIDProcessLocation_Detail] (
    [ID]               VARCHAR (20) CONSTRAINT [DF_RFIDProcessLocation_Detail_ID] DEFAULT ('') NOT NULL,
    [RFIDProcessTable] VARCHAR (2)  CONSTRAINT [DF_RFIDProcessLocation_Detail_Table] DEFAULT ('') NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線桌子', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDProcessLocation_Detail', @level2type = N'COLUMN', @level2name = N'RFIDProcessTable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Location', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RFIDProcessLocation_Detail', @level2type = N'COLUMN', @level2name = N'ID';

