CREATE TABLE [dbo].[SubProInsRecord] (
    [Ukey]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [SubProcessID]     VARCHAR (10)   CONSTRAINT [DF_SubProInsRecord_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [BundleNo]         VARCHAR (10)   NULL,
    [RejectQty]        INT            NULL,
    [Machine]          VARCHAR (50)   NULL,
    [Remark]           NVARCHAR (300) NULL,
    [AddDate]          DATETIME       NULL,
    [AddName]          VARCHAR (10)   NULL,
    [EditDate]         DATETIME       NULL,
    [Editname]         VARCHAR (10)   NULL,
    [RepairedDatetime] DATETIME       NULL,
    [RepairedName]     VARCHAR (10)   NULL,
    [CustomColumn1]    VARCHAR (300)   NULL,
    [Shift] VARCHAR(5) CONSTRAINT [DF_SubProInsRecord_Shift] DEFAULT ('') NOT NULL,
	[FactoryID] VARCHAR(8) CONSTRAINT [DF_SubProInsRecord_FactoryID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SubProInsRecord] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);





