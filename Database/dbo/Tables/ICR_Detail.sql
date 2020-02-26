CREATE TABLE [dbo].[ICR_Detail] (
    [ID]        VARCHAR (13)    DEFAULT ('') NOT NULL,
    [MtltypeID] VARCHAR (20)    DEFAULT ('') NOT NULL,
    [Seq1]      VARCHAR (3)     DEFAULT ('') NOT NULL,
    [Seq2]      VARCHAR (2)     DEFAULT ('') NOT NULL,
    [ICRQty]    NUMERIC (8, 2)  DEFAULT ((0)) NOT NULL,
    [ICRFoc]    NUMERIC (8, 2)  DEFAULT ((0)) NOT NULL,
    [Price]     NUMERIC (16, 4) DEFAULT ((0)) NOT NULL,
    [PriceUSD]  NUMERIC (16, 2) DEFAULT ((0)) NOT NULL,
    [AddName]   VARCHAR (10)    DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME        NULL,
    [EditName]  VARCHAR (10)    DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME        NULL,
    CONSTRAINT [PK_ICR_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Seq1] ASC, [Seq2] ASC)
);

