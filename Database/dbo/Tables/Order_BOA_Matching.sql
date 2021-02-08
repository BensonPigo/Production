CREATE TABLE [dbo].[Order_BOA_Matching] (
    [ID]            VARCHAR (13) NULL,
    [Order_BOAUkey] BIGINT       NOT NULL,
    [Seq]           VARCHAR (2)  NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Order_BOAUkey] ASC, [Seq] ASC)
);

