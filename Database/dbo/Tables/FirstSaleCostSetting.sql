CREATE TABLE [dbo].[FirstSaleCostSetting] (
    [CountryID]  VARCHAR (2)  CONSTRAINT [DF_FirstSaleCostSetting_CountryID] DEFAULT ('') NOT NULL,
    [ArtWorkID]  VARCHAR (20) CONSTRAINT [DF_FirstSaleCostSetting_ArtWorkID] DEFAULT ('') NOT NULL,
    [CostTypeID] VARCHAR (13) CONSTRAINT [DF_FirstSaleCostSetting_CostTypeID] DEFAULT ('') NOT NULL,
    [BeginDate]  DATE         NOT NULL,
    [EndDate]    DATE         NOT NULL,
    [IsJunk]     BIT          CONSTRAINT [DF_FirstSaleCostSetting_IsJunk] DEFAULT ((0)) NOT NULL,
    [OrderCompanyID] NUMERIC(2,0) CONSTRAINT [DF_FirstSaleCostSetting_OrderCompanyID] DEFAULT ((0)) NOT NULL,
    [AddDate]    DATETIME     NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_FirstSaleCostSetting_AddName] DEFAULT ('') NOT NULL,
    [EditDate]   DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_FirstSaleCostSetting_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FirstSaleCostSetting] PRIMARY KEY CLUSTERED ([CountryID] ASC, [ArtWorkID] ASC, [CostTypeID] ASC, [BeginDate] ASC)
);



GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'國別代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'CountryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'ArtWorkID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成本類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'CostTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起始日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'BeginDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'迄止日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'EndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'IsJunk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderCompanyID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'OrderCompanyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'First sale Cost Setting' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FirstSaleCostSetting'
GO
