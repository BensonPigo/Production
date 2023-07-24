CREATE TABLE [dbo].[TPEPass1] (
    [ID]    VARCHAR (10) CONSTRAINT [DF_TPEPASS1_ID] DEFAULT ('') NOT NULL,
    [Name]  VARCHAR (50) CONSTRAINT [DF_TPEPass1_Name] DEFAULT ('') NOT NULL,
    [ExtNo] VARCHAR (6)  CONSTRAINT [DF_TPEPass1_ExtNo] DEFAULT ('') NOT NULL,
    [EMail] VARCHAR (80) CONSTRAINT [DF_TPEPass1_EMail] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_TPEPASS1] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Taipei Pass1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TPEPass1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TPEPass1', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TPEPass1', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分機', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TPEPass1', @level2type = N'COLUMN', @level2name = N'ExtNo';

