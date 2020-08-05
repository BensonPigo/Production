CREATE TABLE [dbo].[SubProDefectCode] (
    [ArtworkTypeID] VARCHAR (20)   NOT NULL,
    [DefectCode]    VARCHAR (50)   NOT NULL,
    [Junk]          BIT            CONSTRAINT [DF_SubProDefectCode_Junk] DEFAULT ((0)) NOT NULL,
    [Description]   NVARCHAR (500) NULL,
    [AddDate]       DATETIME       NULL,
    [AddName]       VARCHAR (10)   NULL,
    [EditDate]      DATETIME       NULL,
    [Editname]      VARCHAR (10)   NULL,
    CONSTRAINT [PK_SubProDefectCode] PRIMARY KEY CLUSTERED ([ArtworkTypeID] ASC, [DefectCode] ASC)
);

