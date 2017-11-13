CREATE TABLE [dbo].[MyGridUDF2] (
    [PKey]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [FKUDF1]            BIGINT        CONSTRAINT [DF_MyGridUDF2_FKUDF1] DEFAULT ((0)) NULL,
    [FKSTD2]            BIGINT        CONSTRAINT [DF_MyGridUDF2_FKSTD2] DEFAULT ((0)) NULL,
    [FormName]          VARCHAR (80)  CONSTRAINT [DF_MyGridUDF2_FormName] DEFAULT ('') NULL,
    [Sele]              VARCHAR (1)   CONSTRAINT [DF_MyGridUDF2_Sele] DEFAULT ('') NULL,
    [Sequence]          INT           CONSTRAINT [DF_MyGridUDF2_Sequence] DEFAULT ((0)) NULL,
    [GridControlSource] VARCHAR (500) CONSTRAINT [DF_MyGridUDF2_GridControlSource] DEFAULT ('') NULL,
    [GridHeader]        NVARCHAR (20) CONSTRAINT [DF_MyGridUDF2_GridHeader] DEFAULT ('') NULL,
    [GridWidth]         INT           CONSTRAINT [DF_MyGridUDF2_GridWidth] DEFAULT ((0)) NULL,
    [GridFormat]        VARCHAR (20)  CONSTRAINT [DF_MyGridUDF2_GridFormat] DEFAULT ('') NULL,
    [GridInputmask]     VARCHAR (20)  CONSTRAINT [DF_MyGridUDF2_GridInputmask] DEFAULT ('') NULL,
    [GridColumnName]    VARCHAR (80)  NULL,
    CONSTRAINT [PK_MyGridUDF2] PRIMARY KEY CLUSTERED ([PKey] ASC)
);



