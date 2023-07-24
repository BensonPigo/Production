CREATE TABLE [dbo].[Order_BOA_Matching_Detail] (
    [ID]            VARCHAR (13) CONSTRAINT [DF_Order_BOA_Matching_Detail_ID] DEFAULT ('') NOT NULL,
    [Order_BOAUkey] BIGINT       NOT NULL,
    [Seq]           VARCHAR (2)  NOT NULL,
    [SizeCode]      VARCHAR (8)  NOT NULL,
    [MatchingRatio] DECIMAL (6)  CONSTRAINT [DF_Order_BOA_Matching_Detail_MatchingRatio] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_Detail_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Order_BOA_Matching_Detail_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME     NULL,
    PRIMARY KEY CLUSTERED ([Order_BOAUkey] ASC, [Seq] ASC, [SizeCode] ASC)
);



