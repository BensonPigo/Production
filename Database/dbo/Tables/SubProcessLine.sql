CREATE TABLE [dbo].[SubProcessLine] (
    [Type]        VARCHAR (10)   DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (10)   DEFAULT ('') NOT NULL,
    [GroupID]     VARCHAR (10)   DEFAULT ('') NULL,
    [Description] NVARCHAR (100) DEFAULT ('') NULL,
    [Manpower]    NUMERIC (5, 2) DEFAULT ((0)) NULL,
    [Remark]      NVARCHAR (100) DEFAULT ('') NULL,
    [Junk]        BIT            DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)   DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [MdivisionID] VARCHAR (8)    NULL,
    PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);


