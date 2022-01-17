CREATE TABLE [dbo].[P_MachineMasterList]
(
	[Ukey] BIGINT NOT NULL IDENTITY , 
    [Month] VARCHAR(6) NULL CONSTRAINT [DF_P_MachineMasterList_Month] DEFAULT (''), 
    [MachineID] VARCHAR(16) NULL CONSTRAINT [DF_P_MachineMasterList_MachineID] DEFAULT (''), 
    [M] VARCHAR(8) NULL CONSTRAINT [DF_P_MachineMasterList_M] DEFAULT (''),  
    [FTY] VARCHAR(8) NULL CONSTRAINT [DF_P_MachineMasterList_FTY] DEFAULT (''), 
    [MachineLocationID] VARCHAR(10) NULL CONSTRAINT [DF_P_MachineMasterList_MachineLocationID] DEFAULT (''),  
    [MachineGroup] NVARCHAR(200) NULL CONSTRAINT [DF_P_MachineMasterList_MachineGroup] DEFAULT (''),  
    [BrandID] VARCHAR(10) NULL CONSTRAINT [DF_P_MachineMasterList_BrandID] DEFAULT (''),  
    [BrandName] VARCHAR(50) NULL CONSTRAINT [DF_P_MachineMasterList_BrandName] DEFAULT (''),  
    [Model] VARCHAR(50) NULL CONSTRAINT [DF_P_MachineMasterList_Model] DEFAULT (''), 
    [SerialNo] VARCHAR(20) NULL CONSTRAINT [DF_P_MachineMasterList_SerialNo] DEFAULT (''),  
    [Condition] VARCHAR(10) NULL CONSTRAINT [DF_P_MachineMasterList_Condition] DEFAULT (''),  
    [PendingCountryMangerApvDate] DATE NULL, 
    [RepairStartDate] DATE NULL, 
    [EstFinishRepairDate] DATE NULL, 
    [MachineArrivalDate] DATE NULL, 
    [TransferDate] DATE NULL, 
    [LendTo] VARCHAR(25) NULL CONSTRAINT [DF_P_MachineMasterList_LendTo] DEFAULT (''),   
    [LendDate] DATE NULL, 
    [LastEstReturnDate] DATE NULL, 
    [Remark] NVARCHAR(100) NULL CONSTRAINT [DF_P_MachineMasterList_Remark] DEFAULT (''),  
    [FAID] VARCHAR(21) NULL CONSTRAINT [DF_P_MachineMasterList_FAID] DEFAULT (''),  
    CONSTRAINT [PK_P_MachineMasterList] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'報表月份',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'Month'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'機器編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'MachineID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'目前位置',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'MachineLocationID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'待Country Mgr審核日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'PendingCountryMangerApvDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'機器借給誰',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'LendTo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'機器借出日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'LendDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預計歸還日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'LastEstReturnDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'固資編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MachineMasterList',
    @level2type = N'COLUMN',
    @level2name = N'FAID'
GO

CREATE INDEX [IX_P_MachineMasterList_MachineIDMonth] ON [dbo].[P_MachineMasterList] ([MachineID], [Month])
