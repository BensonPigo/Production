CREATE TABLE [dbo].[Order_BOA_Matching_Detail] (
    [ID]            VARCHAR (13) NULL,
    [Order_BOAUkey] BIGINT       NOT NULL,
    [Seq]           VARCHAR (2)  NOT NULL,
    [SizeCode]      VARCHAR (8)  NOT NULL,
    [MatchingRatio] NUMERIC (6)  CONSTRAINT [DF_Order_BOA_Matching_Detail_MatchingRatio] DEFAULT ((0)) NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_Detail_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Order_BOAUkey] ASC, [Seq] ASC, [SizeCode] ASC)
);

