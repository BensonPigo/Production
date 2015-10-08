CREATE TABLE [dbo].[ThreadColorComb] (
    [ThreadComboID]       VARCHAR (10)   CONSTRAINT [DF_ThreadColorComb_ThreadComboID] DEFAULT ('') NULL,
    [Machinetypeid]       VARCHAR (10)   CONSTRAINT [DF_ThreadColorComb_Machinetypeid] DEFAULT ('') NULL,
    [StyleUkey]           VARCHAR (10)   CONSTRAINT [DF_ThreadColorComb_StyleUkey] DEFAULT ('') NULL,
    [Id] BIGINT   CONSTRAINT [DF_ThreadColorComb_ThreadColorCombUkey] DEFAULT ('') NOT NULL,
    [Length]              NUMERIC (8, 2) CONSTRAINT [DF_ThreadColorComb_Length] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ThreadColorComb] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Color Combination', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線布種組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb', @level2type = N'COLUMN', @level2name = N'ThreadComboID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器工段種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb', @level2type = N'COLUMN', @level2name = N'Machinetypeid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'StyleUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ThreadColorCombUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb', @level2type = N'COLUMN', @level2name = 'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線總長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb', @level2type = N'COLUMN', @level2name = N'Length';

