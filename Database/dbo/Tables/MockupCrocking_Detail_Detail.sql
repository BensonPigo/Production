CREATE TABLE [dbo].[MockupCrocking_Detail_Detail] (
    [ID]            VARCHAR (13)  NOT NULL,
    [ReportNo]      VARCHAR (13)  NOT NULL,
    [Ukey]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [ArtworkTypeID] VARCHAR (20)  CONSTRAINT [DF__MockupCro__Artwo__09C1AD97] DEFAULT ('') NULL,
    [ArtworkColor]  VARCHAR (35)  CONSTRAINT [DF__MockupCro__Artwo__0AB5D1D0] DEFAULT ('') NULL,
    [FabricRefNo]   VARCHAR (30)  CONSTRAINT [DF__MockupCro__Fabri__0BA9F609] DEFAULT ('') NULL,
    [FabricColor]   VARCHAR (35)  CONSTRAINT [DF__MockupCro__Fabri__0C9E1A42] DEFAULT ('') NULL,
    [DryScale]      VARCHAR (10)  CONSTRAINT [DF__MockupCro__DrySc__0D923E7B] DEFAULT ('') NULL,
    [WetScale]      VARCHAR (10)  CONSTRAINT [DF__MockupCro__WetSc__0E8662B4] DEFAULT ('') NULL,
    [Design]        VARCHAR (100) CONSTRAINT [DF_MockupCrocking_Detail_Detail_Design] DEFAULT ('') NULL,
    [Result]        VARCHAR (4)   CONSTRAINT [DF__MockupCro__Resul__0F7A86ED] DEFAULT ('') NULL,
    [Remark]        VARCHAR (300) CONSTRAINT [DF__MockupCro__Remar__106EAB26] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF__MockupCro__AddNa__1162CF5F] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF__MockupCro__EditN__1256F398] DEFAULT ('') NULL,
    CONSTRAINT [PK__MockupCr__5F39671C69B9272A] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


