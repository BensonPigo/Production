CREATE TABLE [dbo].[P_MachineMasterListByDays] (
    [Ukey]                        BIGINT          IDENTITY (1, 1) NOT NULL,
    [MachineID]                   VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_MachineID_New] DEFAULT ('') NULL,
    [M]                           VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_M_New] DEFAULT ('') NULL,
    [FTY]                         VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_FTY_New] DEFAULT ('') NULL,
    [MachineLocationID]           VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_MachineLocationID_New] DEFAULT ('') NULL,
    [MachineGroup]                NVARCHAR (1000) CONSTRAINT [DF_P_MachineMasterListByDays_MachineGroup_New] DEFAULT ('') NULL,
    [BrandID]                     VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_BrandID_New] DEFAULT ('') NULL,
    [BrandName]                   VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_BrandName_New] DEFAULT ('') NULL,
    [Model]                       VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_Model_New] DEFAULT ('') NULL,
    [SerialNo]                    VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_SerialNo_New] DEFAULT ('') NULL,
    [Condition]                   VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_Condition_New] DEFAULT ('') NULL,
    [PendingCountryMangerApvDate] DATE            NULL,
    [RepairStartDate]             DATE            NULL,
    [EstFinishRepairDate]         DATE            NULL,
    [MachineArrivalDate]          DATE            NULL,
    [TransferDate]                DATE            NULL,
    [LendTo]                      VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_LendTo_New] DEFAULT ('') NULL,
    [LendDate]                    DATE            NULL,
    [LastEstReturnDate]           DATE            NULL,
    [Remark]                      NVARCHAR (1000) CONSTRAINT [DF_P_MachineMasterListByDays_Remark_New] DEFAULT ('') NULL,
    [FAID]                        VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_FAID_New] DEFAULT ('') NULL,
    [Junk]                        VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_Junk_New] DEFAULT ('') NULL,
    [POID]                        VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_POID_New] DEFAULT ('') NULL,
    [RefNo]                       VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_RefNo_New] DEFAULT ('') NULL,
    [BIFactoryID]                 VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]                DATETIME        NULL,
    [BIStatus]                    VARCHAR (8000)  CONSTRAINT [DF_P_MachineMasterListByDays_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_MachineMasterListByDays] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機器編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'MachineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'目前位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'MachineLocationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'待Country Mgr審核日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'PendingCountryMangerApvDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機器借給誰' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'LendTo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機器借出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'LendDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計歸還日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'LastEstReturnDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'固資編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'FAID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MachineMasterListByDays', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_MachineMasterListByDays', @level2type = N'COLUMN', @level2name = N'BIStatus';

