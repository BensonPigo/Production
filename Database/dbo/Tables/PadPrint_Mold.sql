CREATE TABLE [dbo].[PadPrint_Mold](
	[PadPrint_ukey] [bigint] NOT NULL,
	[MoldID] [varchar](10)　	CONSTRAINT [DF_PadPrint_Mold_MoldID]  DEFAULT ('') NOT NULL,
	[Refno] [varchar](36)　		CONSTRAINT [DF_PadPrint_Mold_Refno]  DEFAULT ('') NOT NULL,
	[BrandID] [varchar](8)　	CONSTRAINT [DF_PadPrint_Mold_BrandID]  DEFAULT ('')　NOT NULL,
	[Season] [varchar](10)　	CONSTRAINT [DF_PadPrint_Mold_Season]  DEFAULT ('') NOT NULL,
	[LabelFor] [varchar](1)　	CONSTRAINT [DF_PadPrint_Mold_LabelFor]  DEFAULT ('')  NOT NULL,
	[MainSize] [varchar](1)　	CONSTRAINT [DF_PadPrint_Mold_MainSize]  DEFAULT ('') NOT NULL,
	[Gender] [varchar](1)　		CONSTRAINT [DF_PadPrint_Mold_Gender]  DEFAULT ('') NOT NULL,
	[AgeGroup] [varchar](1)　	CONSTRAINT [DF_PadPrint_Mold_AgeGroup]  DEFAULT ('') NOT NULL,
	[SizeSpec] [varchar](1)　	CONSTRAINT [DF_PadPrint_Mold_SizeSpec]  DEFAULT ('') NOT NULL,
	[Part] [varchar](3)　		CONSTRAINT [DF_PadPrint_Mold_Part]  DEFAULT ('') NOT NULL,
	[MadeIn] [varchar](2)　		CONSTRAINT [DF_PadPrint_Mold_MadeIn]  DEFAULT ('') NOT NULL,
	[Region] [varchar](3)　		CONSTRAINT [DF_PadPrint_Mold_Region]  DEFAULT ('') NOT NULL,
	[AddName] [varchar](10)　	CONSTRAINT [DF_PadPrint_Mold_AddName]  DEFAULT ('') NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10)　	CONSTRAINT [DF_PadPrint_Mold_EditName]  DEFAULT ('') NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PadPrint_Mold] PRIMARY KEY CLUSTERED 
(
	[PadPrint_ukey] ASC,
	[MoldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO