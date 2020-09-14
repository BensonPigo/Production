CREATE TABLE [dbo].[ForecastLoadingBI_Summary](
	[MDivisionID] [varchar](8) NULL,
	[KpiCode] [varchar](8) NULL,
	[Key] [varchar](6) NULL,
	[ArtworkTypeID] [varchar](20) NULL,
	[Capacity(CPU)] [numeric](38, 6) NULL,
	[Loading (CPU)] [numeric](38, 6) NULL
) ON [PRIMARY]
GO