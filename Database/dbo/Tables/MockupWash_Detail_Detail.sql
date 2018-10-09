CREATE TABLE [dbo].[MockupWash_Detail_Detail] (
    [ID]            VARCHAR (13)  NOT NULL,
    [ReportNo]      VARCHAR (13)  NOT NULL,
    [Ukey]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [ArtworkTypeID] VARCHAR (20)  DEFAULT ('') NULL,
    [ArtworkColor]  VARCHAR (35)  DEFAULT ('') NULL,
    [FabricRefNo]   VARCHAR (30)  DEFAULT ('') NULL,
    [FabricColor]   VARCHAR (35)  DEFAULT ('') NULL,
    [Result]        VARCHAR (4)   DEFAULT ('') NULL,
    [Remark]        VARCHAR (300) DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [AddName]       VARCHAR (10)  DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [EditName]      VARCHAR (10)  DEFAULT ('') NULL,
    [Design]        VARCHAR (100) DEFAULT ('') NULL,
    [Typeofprint]   VARCHAR (5)   NULL,
    PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




