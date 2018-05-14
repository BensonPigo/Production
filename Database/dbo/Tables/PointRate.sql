CREATE TABLE [dbo].[PointRate](
	[BrandID] [varchar](8) NOT NULL,
	[ID] [varchar](1) NOT NULL CONSTRAINT [DF_PointRate_OptionID]  DEFAULT ((1)),
	[Junk] [bit] NULL CONSTRAINT [DF_PointRate_Junk]  DEFAULT ((0)),
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PointRate_1] PRIMARY KEY CLUSTERED 
(
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'«~µP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PointRate', @level2type=N'COLUMN',@level2name=N'BrandID'
GO


