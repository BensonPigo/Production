CREATE TABLE [dbo].[Export_ShipAdvice_Container](
	[Ukey] [bigint] NOT NULL,
	[Export_Detail_Ukey] [bigint] NULL,
	[ContainerType] [varchar](2) NOT NULL,
	[ContainerNo] [varchar](20) NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	CONSTRAINT [PK_Export_ShipAdvice_Container] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

	ALTER TABLE [dbo].[Export_ShipAdvice_Container] ADD  CONSTRAINT [DF_Export_ShipAdvice_ContainerType]  DEFAULT ('') FOR [ContainerType]
GO
	
	ALTER TABLE [dbo].[Export_ShipAdvice_Container] ADD  CONSTRAINT [DF_Export_ShipAdvice_Container_ContainerNo]  DEFAULT ('') FOR [ContainerNo]
GO
	
	ALTER TABLE [dbo].[Export_ShipAdvice_Container] ADD  CONSTRAINT [DF_Export_ShipAdvice_Container_AddName]  DEFAULT ('') FOR [AddName]
GO
	
	ALTER TABLE [dbo].[Export_ShipAdvice_Container] ADD  CONSTRAINT [DF_Export_ShipAdvice_Container_EditName]  DEFAULT ('') FOR [EditName]
GO