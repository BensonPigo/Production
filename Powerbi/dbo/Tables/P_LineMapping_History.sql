CREATE TABLE [dbo].[P_LineMapping_History]
(
	[Ukey] bigint NOT NULL IDENTITY(1,1), 
    [Factory] VARCHAR(8) NOT NULL DEFAULT (''), 
    [StyleUKey] BIGINT NOT NULL DEFAULT ((0)), 
    [ComboType] VARCHAR NOT NULL DEFAULT (''), 
    [Version] TINYINT NOT NULL DEFAULT (''), 
    [Phase] VARCHAR(7) NOT NULL DEFAULT (''), 
    [SewingLine] VARCHAR(8) NOT NULL DEFAULT (''), 
    [IsFrom] VARCHAR(6) NOT NULL DEFAULT (''), 
    [Team] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL, 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_LineMapping_History] PRIMARY KEY ([Ukey])

)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'Factory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'串Style.Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'StyleUKey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'套裝部位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'ComboType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM版號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'Version'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ALM階段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'Phase'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'生產線',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'SewingLine'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'資料來源為IE P03或IE P06',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'IsFrom'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'生產組別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'Team'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LineMapping_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'