CREATE TABLE [dbo].[CompleteCFAReturn] (
    [ID]        BIGINT       NOT NULL,
    [SCICtnNo]  VARCHAR (16) NOT NULL,
    [Time]      DATETIME     NULL,
    [SCIUpdate] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompleteCFAReturn] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteCFAReturn', @level2type = N'COLUMN', @level2name = N'SCIUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WMS完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteCFAReturn', @level2type = N'COLUMN', @level2name = N'Time';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteCFAReturn', @level2type = N'COLUMN', @level2name = N'SCICtnNo';

