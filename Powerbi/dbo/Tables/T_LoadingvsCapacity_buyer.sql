CREATE TABLE [dbo].[T_LoadingvsCapacity_buyer](
	[MDivisionID] [varchar](8) NULL,
	[KpiCode] [varchar](8) NULL,
	[Key] [varchar](6) NULL,
	[Half key] [varchar](8) NULL,
	[ArtworkTypeID] [varchar](20) NULL,
	[Capacity(CPU)] [numeric](38, 6) NULL,
	[Loading (CPU)] [numeric](38, 6) NULL,
	[TransferBIDate] [datetime] NULL,
	[Ukey] [bigint] NOT NULL
) ON [PRIMARY]