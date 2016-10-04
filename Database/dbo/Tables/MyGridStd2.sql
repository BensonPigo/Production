CREATE TABLE [dbo].[MyGridStd2] (
    [PKey]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [FormName]          VARCHAR (80)  CONSTRAINT [DF_MyGridStd2_FormName] DEFAULT ('') NULL,
    [Sequence]          INT           CONSTRAINT [DF_MyGridStd2_Sequence] DEFAULT ((0)) NULL,
    [GridControlSource] VARCHAR (500) CONSTRAINT [DF_MyGridStd2_GridControlSource] DEFAULT ('') NULL,
    [GridHeader]        NVARCHAR (20) CONSTRAINT [DF_MyGridStd2_GridHeader] DEFAULT ('') NULL,
    [GridWidth]         INT           CONSTRAINT [DF_MyGridStd2_GridWidth] DEFAULT ((0)) NULL,
    [GridFormat]        VARCHAR (20)  CONSTRAINT [DF_MyGridStd2_GridFormat] DEFAULT ('') NULL,
    [GridInputMask]     VARCHAR (20)  CONSTRAINT [DF_MyGridStd2_GridInputMask] DEFAULT ('') NULL,
    [HasDefault]        BIT           CONSTRAINT [DF_MyGridStd2_HasDefault] DEFAULT ((0)) NULL,
    [SeleField]         BIT           CONSTRAINT [DF_MyGridStd2_SeleField] DEFAULT ((0)) NULL,
    [FieldType]         VARCHAR (1)   CONSTRAINT [DF_MyGridStd2_FieldType] DEFAULT ('') NULL,
    [SeleOrder]         BIT           CONSTRAINT [DF_MyGridStd2_SeleOrder] DEFAULT ((0)) NULL,
    [LocateFor]         BIT           CONSTRAINT [DF_MyGridStd2_LocateFor] DEFAULT ((0)) NULL,
    [HelpColumnWidths]  VARCHAR (20)  CONSTRAINT [DF_MyGridStd2_HelpColumnWidths] DEFAULT ('') NULL,
    [HelpRecordSource]  VARCHAR (100) CONSTRAINT [DF_MyGridStd2_HelpRecordSource] DEFAULT ('') NULL,
    [GridColumnName]    NVARCHAR (80) CONSTRAINT [DF__MyGridStd__GridC__45000CF1] DEFAULT ('') NULL,
    CONSTRAINT [PK_MyGridStd2] PRIMARY KEY CLUSTERED ([PKey] ASC)
);



