CREATE TABLE [dbo].[MockupWash] (
    [ID]           VARCHAR (13)  NOT NULL,
    [BrandID]      VARCHAR (8)   NULL,
    [StyleID]      VARCHAR (15)  NULL,
    [SeasonID]     VARCHAR (8)   NULL,
    [Article]      VARCHAR (8)   NULL,
    [ReceivedDate] DATETIME      NULL,
    [ReleasedDate] DATETIME      NULL,
    [T1Subcon]     VARCHAR (8)   NULL,
    [T2Supplier]   VARCHAR (8)   NULL,
    [Remark]       VARCHAR (300) DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [AddName]      VARCHAR (10)  DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    [EditName]     VARCHAR (10)  DEFAULT ('') NULL,
    [Type]         VARCHAR (1)   CONSTRAINT [DF_MockupWash_Type] DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


