CREATE TABLE [dbo].[CfaArea] (
    [Id]          VARCHAR (3)  NOT NULL,
    [Description] VARCHAR (50) NULL,
    [Junk]        BIT          NULL,
    [AddName]     VARCHAR (10) NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) NULL,
    [EditDate]    DATETIME     NULL,
    CONSTRAINT [PK_CfaArea] PRIMARY KEY CLUSTERED ([Id] ASC)
);

