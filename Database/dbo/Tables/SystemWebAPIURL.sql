CREATE TABLE [dbo].[SystemWebAPIURL] (
    [SystemName]  VARCHAR (8)   CONSTRAINT [DF_SystemWebAPIURL_SystemName] DEFAULT ('') NOT NULL,
    [CountryID]   VARCHAR (3)   CONSTRAINT [DF_SystemWebAPIURL_CountryID] DEFAULT ('') NOT NULL,
    [URL]         VARCHAR (100) CONSTRAINT [DF_SystemWebAPIURL_URL] DEFAULT ('') NOT NULL,
    [Environment] VARCHAR (20)  CONSTRAINT [DF_SystemWebAPIURL_Environment] DEFAULT ('') NOT NULL,
    [Junk]        BIT           CONSTRAINT [DF_SystemWebAPIURL_Junk] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_SystemWebAPIURL] PRIMARY KEY CLUSTERED ([SystemName] ASC, [Environment] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認該 WebAPI 是否開啟', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemWebAPIURL', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dummy, Formal', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemWebAPIURL', @level2type = N'COLUMN', @level2name = N'Environment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各區所在的國家', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemWebAPIURL', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各區系統名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SystemWebAPIURL', @level2type = N'COLUMN', @level2name = N'SystemName';

