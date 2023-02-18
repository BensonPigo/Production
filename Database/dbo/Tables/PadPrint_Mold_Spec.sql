CREATE TABLE [dbo].[PadPrint_Mold_Spec](
	[PadPrint_ukey] [bigint] NOT NULL,
	[MoldID] [varchar](10)　		CONSTRAINT [DF_PadPrint_Mold_Spec_MoldID]  DEFAULT ('') NOT NULL,
	[Side] [varchar](1)				CONSTRAINT [DF_PadPrint_Mold_Spec_Side]  DEFAULT ('')　NOT NULL,
	[SizePage] [varchar](10)　		CONSTRAINT [DF_PadPrint_Mold_Spec_SizePage]  DEFAULT ('') NOT NULL,
	[SourceSize] [varchar](20)　	CONSTRAINT [DF_PadPrint_Mold_Spec_SourceSize]  DEFAULT ('') NOT NULL,
	[CustomerSize] [varchar](20)　	CONSTRAINT [DF_PadPrint_Mold_Spec_CustomerSize]  DEFAULT ('') NOT NULL,
	[MoldRef] [varchar](500)　		CONSTRAINT [DF_PadPrint_Mold_Spec_MoldRef]  DEFAULT ('')  NOT NULL,
	[Version] [varchar](3)　		CONSTRAINT [DF_PadPrint_Mold_Spec_Version]  DEFAULT ('') NOT NULL,
	[ReversionMold] [varchar](10)　	CONSTRAINT [DF_PadPrint_Mold_Spec_ReversionMold]  DEFAULT ('') NOT NULL,
	[Junk] [bit]　					CONSTRAINT [DF_PadPrint_Mold_Spec_Junk]  DEFAULT ((0)) NOT NULL,
	[Reason] [varchar](500)　		CONSTRAINT [DF_PadPrint_Mold_Spec_Reason]  DEFAULT ('') NOT NULL,
	[AddName] [varchar](10)　		CONSTRAINT [DF_PadPrint_Mold_Spec_AddName]  DEFAULT ('') NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10)　		CONSTRAINT [DF_PadPrint_Mold_Spec_EditName]  DEFAULT ('') NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PadPrint_Mold_Spec] PRIMARY KEY CLUSTERED 
(
	[PadPrint_ukey] ASC,
	[MoldID] ASC,
	[Side] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
