CREATE TABLE [dbo].[Order_BOA_Expend_Spec] (
    [Id]                   VARCHAR (13) NOT NULL,
    [Order_BOA_ExpendUkey] BIGINT       NOT NULL,
    [SpecColumnID]         VARCHAR (50) NOT NULL,
    [SpecValue]            VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Order_BOA_Expend_Spec] PRIMARY KEY CLUSTERED ([Order_BOA_ExpendUkey] ASC, [SpecColumnID] ASC)
);

