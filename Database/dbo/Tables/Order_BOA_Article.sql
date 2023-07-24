CREATE TABLE [dbo].[Order_BOA_Article] (
    [ID]            VARCHAR (13) NOT NULL,
    [Order_BoAUkey] BIGINT       NOT NULL,
    [Article]       VARCHAR (8)  CONSTRAINT [DF_Order_BOA_Article_Article] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Order_BOA_Article_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Order_BOA_Article_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME     NULL,
    [Ukey]          BIGINT       NOT NULL,
    CONSTRAINT [PK_Order_BOA_Article] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
CREATE NONCLUSTERED INDEX [ForGetBOAExpend]
    ON [dbo].[Order_BOA_Article]([Order_BoAUkey] ASC);

