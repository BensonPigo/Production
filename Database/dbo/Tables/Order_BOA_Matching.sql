CREATE TABLE [dbo].[Order_BOA_Matching] (
    [ID]            VARCHAR (13) CONSTRAINT [DF_Order_BOA_Matching_ID] DEFAULT ('') NOT NULL,
    [Order_BOAUkey] BIGINT       NOT NULL,
    [Seq]           VARCHAR (2)  NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Order_BOAUkey] ASC, [Seq] ASC)
);



