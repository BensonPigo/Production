CREATE TABLE [dbo].[MyGridStd1] (
    [FormName]         VARCHAR (80)   CONSTRAINT [DF_MyGridStd1_FormName] DEFAULT ('') NOT NULL,
    [Description]      NVARCHAR (100) CONSTRAINT [DF_MyGridStd1_Description] DEFAULT ('') NULL,
    [GridRecordSource] VARCHAR (50)   CONSTRAINT [DF_MyGridStd1_GridRecordSource] DEFAULT ('') NULL,
    [AddName]          VARCHAR (10)   CONSTRAINT [DF_MyGridStd1_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME       NULL,
    [EditName]         VARCHAR (10)   CONSTRAINT [DF_MyGridStd1_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME       NULL,
    CONSTRAINT [PK_MyGridStd1] PRIMARY KEY CLUSTERED ([FormName] ASC)
);

