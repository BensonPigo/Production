Create Table [dbo].[P_StationHourlyOutput](
		[FactoryID] [varchar] (8) Not Null Constraint [DF_P_StationHourlyOutput_FactoryID] Default '',
		[Date] [date] Null,
		[Shift] [varchar] (5) Not Null Constraint [DF_P_StationHourlyOutput_Shift] Default '',
		[Team] [varchar] (10) Not Null Constraint [DF_P_StationHourlyOutput_Team] Default '',
		[Line] [varchar] (5) Not Null Constraint [DF_P_StationHourlyOutput_Line] Default '',
		[Station] [varchar] (2) Not Null Constraint [DF_P_StationHourlyOutput_Station] Default '',
		[Capacity] [int] Not Null Constraint [DF_P_StationHourlyOutput_Capacity] Default 0,
		[Target] [int] Not Null Constraint [DF_P_StationHourlyOutput_Target] Default 0,
		[TotalOutput] [int] Not Null Constraint [DF_P_StationHourlyOutput_TotalOutput] Default 0,
		[ProblemsEncounter] [nvarchar] (500) Not Null Constraint [DF_P_StationHourlyOutput_ProblemsEncounter] Default '', 
		[ActionsTaken] [nvarchar] (500) Not Null Constraint [DF_P_StationHourlyOutput_ActionsTaken] Default '',
		[Problems4MS] [varchar] (50) Not Null Constraint [DF_P_StationHourlyOutput_Problems4MS] Default '',
		[Ukey] [int] Not Null Constraint [DF_P_StationHourlyOutput_Ukey] Default '', 
		[StyleID] [varchar] (200) Not Null Constraint [DF_P_StationHourlyOutput_StyleID] Default '',
		[OrderID] [varchar] (MAX) Not Null Constraint [DF_P_StationHourlyOutput_OrderID] Default '', 
	[Problems4MSDesc] VARCHAR(MAX) NOT NULL DEFAULT '', 
    CONSTRAINT [PK_P_StationHourlyOutput] PRIMARY KEY CLUSTERED 
		(
			[FactoryID] ASC,
			[Ukey] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'FactoryID'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Date'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Shift'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Team' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Team'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Line'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Station' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Station'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每小時最大能產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Capacity'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每小時目標數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Target'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總產量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'TotalOutput'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'問題說明紀錄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'ProblemsEncounter'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採取何種改善紀錄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'ActionsTaken'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Problems 4MS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Problems4MS'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Ukey'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StyleID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'StyleID'
Go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'OrderID'
Go