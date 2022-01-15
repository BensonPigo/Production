CREATE TABLE [dbo].[IESelectCode] (
    [Type]     VARCHAR (5)  CONSTRAINT [DF_IESelectCode_Type] DEFAULT ('') NOT NULL,
    [ID]       VARCHAR (20) CONSTRAINT [DF_IESelectCode_ID] DEFAULT ('') NOT NULL,
    [Name]     VARCHAR (50) CONSTRAINT [DF_IESelectCode_Name] DEFAULT ('') NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_IESelectCode_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_IESelectCode_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_IESelectCode] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);

