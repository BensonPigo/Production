CREATE TABLE [dbo].[FormType]
(
	[ID] VARCHAR(2) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL  CONSTRAINT [DF_FormType_Name] DEFAULT (''), 
    [Remark] NVARCHAR(500) NOT NULL  CONSTRAINT [DF_FormType_Remark] DEFAULT (''), 
    [Junk] BIT NOT NULL  CONSTRAINT [DF_FormType_Junk] DEFAULT (0), 
    [AddName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_FormType_AddName] DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_FormType_EditName] DEFAULT (''), 
    [EditDate] DATETIME NULL,
    CONSTRAINT [PK_FormType] PRIMARY KEY CLUSTERED ([ID] ASC)
)
