CREATE TABLE [dbo].[P_ChangeoverCheckList] (
    [FactoryID]             VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_FactoryID_New] DEFAULT ('') NOT NULL,
    [InlineDate]            DATETIME       NOT NULL,
    [Ready]                 VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_Ready_New] DEFAULT ('') NOT NULL,
    [Line]                  VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_Line_New] DEFAULT ('') NOT NULL,
    [OldSP]                 VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_OldSP_New] DEFAULT ('') NOT NULL,
    [OldStyle]              VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_OldStyle_New] DEFAULT ('') NOT NULL,
    [OldComboType]          VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_OldComboType_New] DEFAULT ('') NOT NULL,
    [NewSP]                 VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_NewSP_New] DEFAULT ('') NOT NULL,
    [NewStyle]              VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_NewStyle_New] DEFAULT ('') NOT NULL,
    [NewComboType]          VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_NewComboType_New] DEFAULT ('') NOT NULL,
    [StyleType]             VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_StyleType_New] DEFAULT ('') NOT NULL,
    [Category]              VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_Category_New] DEFAULT ('') NOT NULL,
    [FirstSewingOutputDate] DATETIME       NULL,
    [BIFactoryID]           VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]          DATETIME       NULL,
    [BIStatus]              VARCHAR (8000) CONSTRAINT [DF_P_ChangeoverCheckList_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ChangeoverCheckList] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [InlineDate] ASC, [Line] ASC, [OldSP] ASC, [OldStyle] ASC, [OldComboType] ASC, [NewSP] ASC, [NewStyle] ASC, [NewComboType] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'InlineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否全部CheckList都已Checked' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'Ready'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'OldSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'OldStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款前套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'OldComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'NewSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'NewStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'NewComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'N顯示New，R顯示Repeat' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'StyleType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後第一次產出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'FirstSewingOutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ChangeoverCheckList', @level2type = N'COLUMN', @level2name = N'BIStatus';

