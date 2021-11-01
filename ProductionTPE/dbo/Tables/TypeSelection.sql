CREATE TABLE [dbo].[TypeSelection] (
    [VersionID] INT          NOT NULL,
    [Seq]       INT          NOT NULL,
    [Code]      VARCHAR (50) CONSTRAINT [DF_TypeSelection_Code] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_TypeSelection] PRIMARY KEY CLUSTERED ([VersionID] ASC, [Seq] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Requirement 自選項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TypeSelection', @level2type = N'COLUMN', @level2name = N'Code';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type 同版本下可選擇的清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TypeSelection', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type 選項版本號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TypeSelection', @level2type = N'COLUMN', @level2name = N'VersionID';

