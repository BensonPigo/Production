CREATE TABLE [dbo].[PadPrint_Mold] (
    [PadPrint_ukey] BIGINT       NOT NULL,
    [MoldID]        VARCHAR (10) CONSTRAINT [DF_PadPrint_Mold_MoldID] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (36) CONSTRAINT [DF_PadPrint_Mold_Refno] DEFAULT ('') NULL,
    [BrandID]       VARCHAR (8)  CONSTRAINT [DF_PadPrint_Mold_BrandID] DEFAULT ('') NOT NULL,
    [Season]        VARCHAR (10) CONSTRAINT [DF_PadPrint_Mold_Season] DEFAULT ('') NOT NULL,
    [LabelFor]      VARCHAR (1)  CONSTRAINT [DF_PadPrint_Mold_LabelFor] DEFAULT ('') NOT NULL,
    [MainSize]      VARCHAR (1)  CONSTRAINT [DF_PadPrint_Mold_MainSize] DEFAULT ('') NOT NULL,
    [Gender]        VARCHAR (1)  CONSTRAINT [DF_PadPrint_Mold_Gender] DEFAULT ('') NOT NULL,
    [AgeGroup]      VARCHAR (1)  CONSTRAINT [DF_PadPrint_Mold_AgeGroup] DEFAULT ('') NOT NULL,
    [SizeSpec]      VARCHAR (1)  CONSTRAINT [DF_PadPrint_Mold_SizeSpec] DEFAULT ('') NOT NULL,
    [Part]          VARCHAR (3)  CONSTRAINT [DF_PadPrint_Mold_Part] DEFAULT ('') NOT NULL,
    [MadeIn]        VARCHAR (2)  CONSTRAINT [DF_PadPrint_Mold_MadeIn] DEFAULT ('') NOT NULL,
    [Region]        VARCHAR (3)  CONSTRAINT [DF_PadPrint_Mold_Region] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_PadPrint_Mold_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_PadPrint_Mold_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_PadPrint_Mold] PRIMARY KEY CLUSTERED ([PadPrint_ukey] ASC, [MoldID] ASC)
);

