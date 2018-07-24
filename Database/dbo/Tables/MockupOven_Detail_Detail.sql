CREATE TABLE [dbo].[MockupOven_Detail_Detail] (
    [ID]            VARCHAR (13)  NOT NULL,
    [ReportNo]      VARCHAR (13)  NOT NULL,
    [Ukey]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [ArtworkTypeID] VARCHAR (20)  CONSTRAINT [DF__MockupOve__Artwo__23817F9A] DEFAULT ('') NULL,
    [ArtworkColor]  VARCHAR (35)  CONSTRAINT [DF__MockupOve__Artwo__2475A3D3] DEFAULT ('') NULL,
    [FabricRefNo]   VARCHAR (30)  CONSTRAINT [DF__MockupOve__Fabri__2569C80C] DEFAULT ('') NULL,
    [FabricColor]   VARCHAR (35)  CONSTRAINT [DF__MockupOve__Fabri__265DEC45] DEFAULT ('') NULL,
    [Design]        VARCHAR (100) NULL,
    [Result]        VARCHAR (4)   CONSTRAINT [DF__MockupOve__Resul__2752107E] DEFAULT ('') NULL,
    [Remark]        VARCHAR (300) CONSTRAINT [DF__MockupOve__Remar__284634B7] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF__MockupOve__AddNa__293A58F0] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF__MockupOve__EditN__2A2E7D29] DEFAULT ('') NULL,
    CONSTRAINT [PK__MockupOv__5F39671CA904D784] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


