CREATE TABLE [dbo].[P_MaterialLocationIndex] (
    [ID]           VARCHAR (8000)  CONSTRAINT [DF_P_MaterialLocationIndex_ID_New] DEFAULT ('') NOT NULL,
    [StockType]    VARCHAR (8000)  CONSTRAINT [DF_P_MaterialLocationIndex_StockType_New] DEFAULT ('') NOT NULL,
    [Junk]         BIT             CONSTRAINT [DF_P_MaterialLocationIndex_Junk_New] DEFAULT ((0)) NOT NULL,
    [Description]  NVARCHAR (1000) CONSTRAINT [DF_P_MaterialLocationIndex_Description_New] DEFAULT ('') NOT NULL,
    [IsWMS]        BIT             CONSTRAINT [DF_P_MaterialLocationIndex_IsWMS_New] DEFAULT ((0)) NOT NULL,
    [Capacity]     INT             CONSTRAINT [DF_P_MaterialLocationIndex_Capacity_New] DEFAULT ((0)) NOT NULL,
    [AddName]      VARCHAR (8000)  CONSTRAINT [DF_P_MaterialLocationIndex_AddName_New] DEFAULT ('') NOT NULL,
    [AddDate]      DATETIME        NULL,
    [EditName]     VARCHAR (8000)  CONSTRAINT [DF_P_MaterialLocationIndex_EditName_New] DEFAULT ('') NOT NULL,
    [EditDate]     DATETIME        NULL,
    [LocationType] VARCHAR (8000)  CONSTRAINT [DF_P_MaterialLocationIndex_LocationType_New] DEFAULT ('Fabric') NOT NULL,
    [BIFactoryID]  VARCHAR (8000)  CONSTRAINT [DF_P_MaterialLocationIndex_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate] DATETIME        NULL,
    [BIStatus]     VARCHAR (8000)  CONSTRAINT [DF_P_MaterialLocationIndex_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_MaterialLocationIndex] PRIMARY KEY CLUSTERED ([ID] ASC, [StockType] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'StockType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否Junk' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'Description'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為自動倉' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'IsWMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'儲位容量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'Capacity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MaterialLocationIndex', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_MaterialLocationIndex', @level2type = N'COLUMN', @level2name = N'BIStatus';

