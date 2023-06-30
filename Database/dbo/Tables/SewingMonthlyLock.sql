CREATE TABLE [dbo].[SewingMonthlyLock] (
    [FactoryID] VARCHAR (8)  DEFAULT ('') NOT NULL,
    [LockDate]  DATE         NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_SewingMonthlyLock_EditName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME     NULL,
    CONSTRAINT [PK_SewingMonthlyLock] PRIMARY KEY CLUSTERED ([FactoryID] ASC)
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingMonthlyLock',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'上鎖日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingMonthlyLock',
    @level2type = N'COLUMN',
    @level2name = N'LockDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後更新人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingMonthlyLock',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後更新日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SewingMonthlyLock',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'