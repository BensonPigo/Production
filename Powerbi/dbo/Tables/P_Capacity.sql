
	CREATE TABLE P_Capacity(
		Ukey bigint NOT NULL IDENTITY(1,1),
		ID varchar(12) NULL CONSTRAINT [DF_PBIReportData_ID] DEFAULT '',
		FTY varchar(8) NULL CONSTRAINT [DF_PBIReportData_FTY] DEFAULT '',
		MDivision varchar(8) NULL CONSTRAINT [DF_PBIReportData_MDivision] DEFAULT '',
		Year int NULL CONSTRAINT [DF_PBIReportData_Year] DEFAULT 0,

		Month int NULL CONSTRAINT [DF_PBIReportData_Month] DEFAULT 0,
		[Key] varchar(7) NULL CONSTRAINT [DF_PBIReportData_Key] DEFAULT '',		
		ArtworkType varchar(50) NULL CONSTRAINT [DF_PBIReportData_ArtworkType] DEFAULT '',
		WorkDays int NULL CONSTRAINT [DF_PBIReportData_WorkDays] DEFAULT 0,
		WorkingHourDaily numeric(5,2) NULL CONSTRAINT [DF_PBIReportData_WorkingHourDaily] DEFAULT 0,

		TotalIndirectManpower int NULL CONSTRAINT [DF_PBIReportData_TotalIndirectManpower] DEFAULT 0,		
		Noofcells numeric(6,2) NULL CONSTRAINT [DF_PBIReportData_Noofcells] DEFAULT 0,
		NoofSewerCell numeric(6,2) NULL CONSTRAINT [DF_PBIReportData_NoofSewerCell] DEFAULT 0,
		NoofSewers int NULL CONSTRAINT [DF_PBIReportData_NoofSewers] DEFAULT 0,
		AbsentRate  numeric(6,2) NULL CONSTRAINT [DF_PBIReportData_AbsentRate] DEFAULT 0,
		
		TotalAvailableSewers int NULL CONSTRAINT [DF_PBIReportData_TotalAvailableSewers] DEFAULT 0,
		AverageProductivity  numeric(5,2) NULL CONSTRAINT [DF_PBIReportData_AverageProductivity] DEFAULT 0,
		FTYCPU numeric(10,3) NULL CONSTRAINT [DF_PBIReportData_FTYCPU] DEFAULT 0,
		SubconCPU numeric(10,3) NULL CONSTRAINT [DF_PBIReportData_SubconCPU] DEFAULT 0,
		TTLCPU numeric(10,3) NULL CONSTRAINT [DF_PBIReportData_TTLCPU] DEFAULT 0,
		
		RemarkDayOffDate nvarchar(max) NULL CONSTRAINT [DF_PBIReportData_RemarkDayOffDate] DEFAULT '',
		MachineAvailableUnits int NULL CONSTRAINT [DF_PBIReportData_MachineAvailableUnits] DEFAULT 0,
		TTLPrinter int NULL CONSTRAINT [DF_PBIReportData_TTLPrinter] DEFAULT 0,
		AverageAttendance numeric(5,2) NULL CONSTRAINT [DF_PBIReportData_AverageAttendance] DEFAULT 0,
		AverageOutputPerHour  numeric(6,2) NULL CONSTRAINT [DF_PBIReportData_AverageOutputPerHour] DEFAULT 0,

		OvalMachineOutputPerDayPPU numeric(6,2) NULL CONSTRAINT [DF_PBIReportData_OvalMachineOutputPerDayPPU] DEFAULT 0,
		AverageStitchesPerHour1000Stiches numeric(6,2) NULL CONSTRAINT [DF_PBIReportData_AverageStitchesPerHour1000Stiches] DEFAULT 0,
		SubconOut1000StichesMins  numeric(10,3) NULL CONSTRAINT [DF_PBIReportData_SubconOut1000StichesMins] DEFAULT 0,
		SubconOutPcs numeric(10,3) NULL CONSTRAINT [DF_PBIReportData_SubconOutPcs] DEFAULT 0,
		ShiftDayandNight int NULL CONSTRAINT [DF_PBIReportData_ShiftDayandNight] DEFAULT 0,

		MachineCapacity  numeric(10,3) NULL CONSTRAINT [DF_PBIReportData_MachineCapacity] DEFAULT 0,
		Unit varchar(8) NULL CONSTRAINT [DF_PBIReportData_Unit] DEFAULT '',		
		ApprovedDate  varchar(10) NULL CONSTRAINT [DF_PBIReportData_ApprovedDate] DEFAULT '',
		 [AverageEfficiency] NUMERIC(5, 2) NULL DEFAULT 0, 
    CONSTRAINT [PK_P_Capacity] PRIMARY KEY CLUSTERED 
		(
			Ukey ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
GO



EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'ID';
GO


EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠別'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'FTY';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年+月'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'Key';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artwork類型'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'ArtworkType';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總工作日'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'Workdays';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'一天工作幾小時'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'WorkingHourDaily';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'直接人力'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'TotalIndirectManpower';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'多少cell'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'Noofcells';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'一個cell有幾個車縫工'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'NoofSewerCell';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'共多少車縫工'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'NoofSewers';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缺席率'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'AbsentRate';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'共多少可用車縫工'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'TotalAvailableSewers';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均生產力'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'AverageProductivity';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠CPU'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'FTYCPU';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發CPU'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'SubconCPU';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總共CPU'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'TTLCPU';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註(當月休假日期)'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'RemarkDayOffDate';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器可用單位'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'MachineAvailableUnits';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'共多少打印機'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'TTLPrinter';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均出席率'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'AverageAttendance';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均每小時多少產出'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'AverageOutputPerHour';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PPU'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'OvalMachineOutputPerDayPPU';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均每小時多少縫線'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'AverageStitchesPerHour1000Stiches';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發(1000 stiches) (mins) '
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'SubconOut1000StichesMins';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發(pcs)'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'SubconOutPcs';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'早、晚班'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'ShiftDayandNight';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Machine產能'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'MachineCapacity';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'Unit';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核日'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'ApprovedDate';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均效率'
, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Capacity'
, @level2type = N'COLUMN', @level2name = N'AverageEfficiency';
GO
