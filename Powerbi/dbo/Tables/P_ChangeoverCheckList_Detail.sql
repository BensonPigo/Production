CREATE TABLE [dbo].[P_ChangeoverCheckList_Detail] (
    [FactoryID]       VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_FactoryID_New] DEFAULT ('') NOT NULL,
    [SP]              VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_SP_New] DEFAULT ('') NOT NULL,
    [Style]           VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Style_New] DEFAULT ('') NOT NULL,
    [ComboType]       VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_ComboType_New] DEFAULT ('') NOT NULL,
    [Category]        VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Category_New] DEFAULT ('') NOT NULL,
    [ProductType]     NVARCHAR (1000) CONSTRAINT [DF_P_ChangeoverCheckList_Detail_ProductType_New] DEFAULT ('') NOT NULL,
    [Line]            VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_Line_New] DEFAULT ('') NOT NULL,
    [DaysLeft]        VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_DaysLeft_New] DEFAULT ('') NULL,
    [InlineDate]      DATETIME        NOT NULL,
    [OverDays]        INT             CONSTRAINT [DF_P_ChangeoverCheckList_Detail_OverDays_New] DEFAULT ((0)) NOT NULL,
    [ChgOverCheck]    VARCHAR (8000)  CONSTRAINT [DFP_ChangeoverCheckList_Detail_Check_New] DEFAULT ('') NOT NULL,
    [CompletionDate]  DATETIME        NULL,
    [ResponseDep]     NVARCHAR (1000) CONSTRAINT [DF_P_ChangeoverCheckList_Detail_ResponseDep_New] DEFAULT ('') NOT NULL,
    [CheckListNo]     INT             CONSTRAINT [DF_P_ChangeoverCheckList_Detail_CheckListNo_New] DEFAULT ((0)) NOT NULL,
    [CheckListItem]   VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_CheckListItem_New] DEFAULT ('') NOT NULL,
    [LateReason]      NVARCHAR (1000) CONSTRAINT [DF_P_ChangeoverCheckList_Detail_LateReason_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]     VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]    DATETIME        NULL,
    [Deadline]        DATE            NULL,
    [CompletedInTime] VARCHAR (8000)  CONSTRAINT [DF__P_Changeo__Compl__09160B73_New] DEFAULT ('') NOT NULL,
    [BIStatus]        VARCHAR (8000)  CONSTRAINT [DF_P_ChangeoverCheckList_Detail_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_ChangeoverCheckList_Detail] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [SP] ASC, [Style] ASC, [ComboType] ASC, [Category] ASC, [ProductType] ASC, [Line] ASC, [InlineDate] ASC, [CheckListNo] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'SP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'換款後款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reason table 的 ReasonTypeID = Style_Apparel_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ProductType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'剩餘天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'DaysLeft'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上線日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'InlineDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逾期天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'OverDays'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'完成該項CheckList後勾選' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ChgOverCheck'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按下Check的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'CompletionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'負責部門' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'ResponseDep'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CheckList代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'CheckListNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Check List 項目名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'CheckListItem'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'逾期原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'LateReason'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ChangeoverCheckList_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_ChangeoverCheckList_Detail', @level2type = N'COLUMN', @level2name = N'BIStatus';

