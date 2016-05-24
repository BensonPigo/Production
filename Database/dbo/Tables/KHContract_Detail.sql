CREATE TABLE [dbo].[KHContract_Detail] (
    [ID]     VARCHAR (15)    NOT NULL,
    [NLCode] VARCHAR (5)     NOT NULL,
    [Seq]    VARCHAR (8)     CONSTRAINT [DF_KHContract_Detail_Seq] DEFAULT ('') NULL,
    [UnitID] VARCHAR (8)     CONSTRAINT [DF_KHContract_Detail_UnitID] DEFAULT ('') NULL,
    [Qty]    NUMERIC (15, 2) CONSTRAINT [DF_KHContract_Detail_Qty] DEFAULT ((0)) NULL,
    [Price]  NUMERIC (7, 2)  CONSTRAINT [DF_KHContract_Detail_Price] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_KHContract_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [NLCode] ASC)
);

