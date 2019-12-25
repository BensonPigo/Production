CREATE TABLE [dbo].[ChangeMemo_ContactWindow] (
    [UKey]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [MDivisionID] VARCHAR (8)  NOT NULL,
    [Pass1FtyID]  VARCHAR (10) NOT NULL,
    [AddName]     VARCHAR (10) NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) NULL,
    [EditDate]    DATETIME     NULL,
    CONSTRAINT [PK_ChangeMemo_ContactWindow] PRIMARY KEY CLUSTERED ([UKey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'聯絡窗口的工號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_ContactWindow', @level2type = N'COLUMN', @level2name = N'Pass1FtyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬廠區', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_ContactWindow', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PPIC聯絡窗口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_ContactWindow';

