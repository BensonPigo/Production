CREATE TABLE [dbo].[Order_BOA_Expend_Keyword] (
    [Id]                   VARCHAR (13)  NOT NULL,
    [Order_BOA_ExpendUkey] BIGINT        NOT NULL,
    [KeywordField]         VARCHAR (30)  NOT NULL,
    [KeywordValue]         VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_Order_BOA_Expend_Keyword] PRIMARY KEY CLUSTERED ([Order_BOA_ExpendUkey] ASC, [KeywordField] ASC)
);

