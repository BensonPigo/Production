CREATE TABLE [dbo].[Order_BOA_Article] (
    [ID]            VARCHAR (13) NOT NULL,
    [Order_BoAUkey] BIGINT       NOT NULL,
    [Article]       VARCHAR (8)  NULL,
    [AddName]       VARCHAR (10) NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) NULL,
    [EditDate]      DATETIME     NULL,
    [Ukey]          BIGINT       NOT NULL,
    CONSTRAINT [PK_Order_BOA_Article] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

