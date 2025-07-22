CREATE TABLE [dbo].[P_ChangeoverCheckList_Detail_Histroy] (
    [HistoryUkey]  BIGINT          IDENTITY (1, 1) NOT NULL,
    [FactoryID]    VARCHAR (8000)  NOT NULL,
    [SP]           VARCHAR (8000)  NOT NULL,
    [Style]        VARCHAR (8000)  NOT NULL,
    [ComboType]    VARCHAR (8000)  NOT NULL,
    [Category]     VARCHAR (8000)  NOT NULL,
    [ProductType]  NVARCHAR (1000) NOT NULL,
    [Line]         VARCHAR (8000)  NOT NULL,
    [InlineDate]   DATETIME        NOT NULL,
    [CheckListNo]  INT             NOT NULL,
    [BIFactoryID]  VARCHAR (8000)  NOT NULL,
    [BIInsertDate] DATETIME        NOT NULL,
    [BIStatus]     VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Histroy_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ChangeoverCheckList_Detail_Histroy] PRIMARY KEY CLUSTERED ([HistoryUkey] ASC)
);



GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reason table 的 ReasonTypeID = Style_Apparel_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'InlineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CheckList代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'CheckListNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail_Histroy', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ChangeoverCheckList_Detail_Histroy', @level2type = N'COLUMN', @level2name = N'BIStatus';

