CREATE TABLE [dbo].[CutInsRecord] (
    [Ukey]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [CutRef]           VARCHAR (6)    CONSTRAINT [DF_CutInsRecord_CutRef] DEFAULT ('') NOT NULL,
    [MDivisionID]      VARCHAR (8)    NOT NULL,
    [ActualWidth]      NUMERIC (5, 2) NULL,
    [Description]      NVARCHAR (300) NULL,
    [Top]              NUMERIC (5, 2) NULL,
    [Middle]           NUMERIC (5, 2) NULL,
    [Bottom]           NUMERIC (5, 2) NULL,
    [Remark]           NVARCHAR (300) NULL,
    [Machine]          NVARCHAR (50)  NULL,
    [CutCellId]        NVARCHAR (2)   NULL,
    [MarkerLength]     NUMERIC (9, 4) NULL,
    [CuttableWidth]    NUMERIC (5, 2) NULL,
    [InspRatio]        INT            NULL,
    [InspectQty]       INT            NULL,
    [RejectQty]        INT            NULL,
    [AddDate]          DATETIME       NULL,
    [AddName]          VARCHAR (10)   NULL,
    [EditDate]         DATETIME       NULL,
    [Editname]         VARCHAR (10)   NULL,
    [RepairedDatetime] DATETIME       NULL,
    [RepairedName]     VARCHAR (10)   NULL,
    CONSTRAINT [PK_CutInsRecord] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



