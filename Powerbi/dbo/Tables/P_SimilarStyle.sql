CREATE TABLE [dbo].[P_SimilarStyle] (
    [OutputDate]         DATE            CONSTRAINT [DF_P_SimilarStyle_OutputDate_New] DEFAULT ('') NOT NULL,
    [FactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_SimilarStyle_FactoryID_New] DEFAULT ('') NOT NULL,
    [StyleID]            VARCHAR (8000)  CONSTRAINT [DF_P_SimilarStyle_StyleID_New] DEFAULT ('') NOT NULL,
    [BrandID]            VARCHAR (8000)  CONSTRAINT [DF_P_SimilarStyle_BrandID_New] DEFAULT ('') NOT NULL,
    [Remark]             NVARCHAR (1000) CONSTRAINT [DF_P_SimilarStyle_Remark_New] DEFAULT ('') NOT NULL,
    [RemarkSimilarStyle] NVARCHAR (2000) CONSTRAINT [DF_P_SimilarStyle_RemarkSimilarStyle_New] DEFAULT ('') NOT NULL,
    [Type]               VARCHAR (8000)  CONSTRAINT [DF_P_SimilarStyle_Type_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_SimilarStyle_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]       DATETIME        NULL,
    [BIStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_SimilarStyle_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SimilarStyle] PRIMARY KEY CLUSTERED ([OutputDate] ASC, [FactoryID] ASC, [StyleID] ASC, [BrandID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'OutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顯示該款示三個月內最近一次產出之日期&最小的產線名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顯示該款示之相似母款, 以及最近三個月內最後一次產出之日期&最小的產線名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'RemarkSimilarStyle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'如果Remark一欄有值 & 且最後產出日期落在當日往前算三個月內 (動態計算), 或是Remark(Similar style) 一欄有值 & 且最後產出日期落在當日往前算三個月內 (動態計算),  顯示"Repeat", 否則顯示"New Style"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SimilarStyle', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SimilarStyle', @level2type = N'COLUMN', @level2name = N'BIStatus';

