CREATE TABLE [dbo].[SpreadingNo] (
    [ID]          VARCHAR (3)   NOT NULL,
    [MDivisionid] VARCHAR (8)   NULL,
    [Description] NVARCHAR (60) NULL,
    [Junk]        BIT           CONSTRAINT [DF_SpreadingNo_Junk] DEFAULT ((0)) NULL,
    [CutCellID]   VARCHAR (2)   NULL,
    [AddName]     VARCHAR (10)  NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_SpreadingNo] PRIMARY KEY CLUSTERED ([ID] ASC)
);

