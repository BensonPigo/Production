Create Table [dbo].[P_StationHourlyOutput_Detail](
		[Ukey] [int] Not Null Constraint [DF_P_StationHourlyOutput_Detail_Ukey] default 0,
		[FactoryID] [varchar] (8) Not Null Constraint [DF_P_StationHourlyOutput_Detail_FactoryID] default '',
		[StationHourlyOutputUkey] [bigint] Not Null Constraint [DF_P_StationHourlyOutput_Detail_StationHourlyOutputUkey] default 0,
		[Oclock] [tinyint] Not Null Constraint [DF_P_StationHourlyOutput_Detail_Oclock] default 0,
		[Qty] [int] Not Null Constraint [DF_P_StationHourlyOutput_Detail_Qty] default 0,
	CONSTRAINT [PK_P_StationHourlyOutput_Detail] PRIMARY KEY CLUSTERED 
		(
			[FactoryID] ASC,
			[Ukey] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'FactoryID'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Ukey'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StationHourlyOutputUkey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'StationHourlyOutputUkey'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間區間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Oclock'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣件數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Qty'
Go
