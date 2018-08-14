CREATE TABLE [dbo].[MockupWash_Detail_Detail] (
    [ID]            VARCHAR (13)  NOT NULL,
    [ReportNo]      VARCHAR (13)  NOT NULL,
    [Ukey]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [ArtworkTypeID] VARCHAR (20)  CONSTRAINT [DF__MockupWas__Artwo__3A64E4F2] DEFAULT ('') NULL,
    [ArtworkColor]  VARCHAR (35)  CONSTRAINT [DF__MockupWas__Artwo__3B59092B] DEFAULT ('') NULL,
    [FabricRefNo]   VARCHAR (30)  CONSTRAINT [DF__MockupWas__Fabri__3C4D2D64] DEFAULT ('') NULL,
    [FabricColor]   VARCHAR (35)  CONSTRAINT [DF__MockupWas__Fabri__3D41519D] DEFAULT ('') NULL,
    [Design]        VARCHAR (100) NULL,
    [Result]        VARCHAR (4)   CONSTRAINT [DF__MockupWas__Resul__3E3575D6] DEFAULT ('') NULL,
    [Remark]        VARCHAR (300) CONSTRAINT [DF__MockupWas__Remar__3F299A0F] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF__MockupWas__AddNa__401DBE48] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF__MockupWas__EditN__4111E281] DEFAULT ('') NULL,
    CONSTRAINT [PK__MockupWa__5F39671CD1DA547A] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


