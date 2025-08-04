CREATE TABLE [dbo].[P_StyleChangeover] (
    [ID]           BIGINT          NOT NULL,
    [FactoryID]    VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_Factory_New] DEFAULT ('') NOT NULL,
    [SewingLine]   VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_SewingLine_New] DEFAULT ('') NOT NULL,
    [Inline]       DATETIME        NULL,
    [OldSP]        VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_OldSP_New] DEFAULT ('') NOT NULL,
    [OldStyle]     VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_OldStyle_New] DEFAULT ('') NOT NULL,
    [OldComboType] VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_OldComboType_New] DEFAULT ('') NOT NULL,
    [NewSP]        VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_NewSP_New] DEFAULT ('') NOT NULL,
    [NewStyle]     VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_NewStyle_New] DEFAULT ('') NOT NULL,
    [NewComboType] VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_NewComboType_New] DEFAULT ('') NOT NULL,
    [Category]     VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_Category_New] DEFAULT ('') NOT NULL,
    [COPT(min)]    NUMERIC (38, 2) CONSTRAINT [DF_P_StyleChangeover_COPT(min)_New] DEFAULT ((0)) NOT NULL,
    [COT(min)]     NUMERIC (38, 2) CONSTRAINT [DF_P_StyleChangeover_COT(min)_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]  VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate] DATETIME        NULL,
    [BIStatus]     VARCHAR (8000)  CONSTRAINT [DF_P_StyleChangeover_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_StyleChangeover] PRIMARY KEY CLUSTERED ([ID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ChgOver.ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'SewingLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inline date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'Inline'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一張的SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'OldSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一個款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'OldStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一張SP的Combo Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'OldComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SP#' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'NewSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'NewStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Combo Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'NewComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款難度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Changeover Process Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'COPT(min)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Changeover Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'COT(min)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StyleChangeover', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_StyleChangeover', @level2type = N'COLUMN', @level2name = N'BIStatus';

