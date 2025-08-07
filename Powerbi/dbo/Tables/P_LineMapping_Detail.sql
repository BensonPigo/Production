CREATE TABLE [dbo].[P_LineMapping_Detail] (
    [ID]                 BIGINT          NOT NULL,
    [Ukey]               BIGINT          NOT NULL,
    [IsFrom]             VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_IsFrom_New] DEFAULT ('') NOT NULL,
    [No]                 VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_No_New] DEFAULT ('') NOT NULL,
    [Seq]                SMALLINT        CONSTRAINT [DF_P_LineMapping_Detail_Seq_New] DEFAULT ((0)) NOT NULL,
    [Location]           VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_Location_New] DEFAULT ('') NOT NULL,
    [ST/MC Type]         VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_ST/MC Type_New] DEFAULT ('') NOT NULL,
    [MC Group]           VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_MC Group_New] DEFAULT ('') NOT NULL,
    [OperationID]        VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_OperationID_New] DEFAULT ('') NOT NULL,
    [Operation]          NVARCHAR (1000) CONSTRAINT [DF_P_LineMapping_Detail_Operation_New] DEFAULT ('') NOT NULL,
    [Annotation]         NVARCHAR (1000) CONSTRAINT [DF_P_LineMapping_Detail_Annotation_New] DEFAULT ('') NOT NULL,
    [Attachment]         VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_Attachment_New] DEFAULT ('') NOT NULL,
    [PartID]             VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_PartID_New] DEFAULT ('') NOT NULL,
    [Template]           VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_Template_New] DEFAULT ('') NOT NULL,
    [GSD Time]           NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Detail_GSD Time_New] DEFAULT ((0)) NOT NULL,
    [Cycle Time]         NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Detail_Cycle Time_New] DEFAULT ((0)) NOT NULL,
    [%]                  NUMERIC (38, 2) CONSTRAINT [DF_P_LineMapping_Detail_%_New] DEFAULT ((0)) NOT NULL,
    [Div. Sewer]         NUMERIC (38, 4) CONSTRAINT [DF__P_LineMap__Div. __0F18FD8C_New] DEFAULT ((0)) NOT NULL,
    [Ori. Sewer]         NUMERIC (38, 4) CONSTRAINT [DF_P_LineMapping_Detail_Ori. Sewer_New] DEFAULT ((0)) NOT NULL,
    [Thread Combination] VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_Thread Combination_New] DEFAULT ('') NOT NULL,
    [Notice]             NVARCHAR (1000) CONSTRAINT [DF_P_LineMapping_Detail_Notice_New] DEFAULT ('') NOT NULL,
    [OperatorID]         VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_OperatorID_New] DEFAULT ('') NOT NULL,
    [OperatorName]       NVARCHAR (1000) CONSTRAINT [DF_P_LineMapping_Detail_OperatorName_New] DEFAULT ('') NOT NULL,
    [Skill]              NVARCHAR (1000) CONSTRAINT [DF_P_LineMapping_Detail_Skill_New] DEFAULT ('') NOT NULL,
    [FactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_FactoryID_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]       DATETIME        NULL,
    [BIStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_LineMapping_Detail_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_LineMapping_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC, [IsFrom] ASC)
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


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串表頭ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源為IE P03或IE P06' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'IsFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站位編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'No'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Location'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫機器代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'ST/MC Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫機器分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'MC Group'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'OperationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Operation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工段註解' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Annotation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模具附屬物代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Attachment'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'輔具規格' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'PartID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'模具模版代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Template'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'General Sewing Data時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cycle秒數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車工差異百分比' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'%'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'配分車工數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Div. Sewer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始車工數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Ori. Sewer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'線組合代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Thread Combination'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'注意事項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Notice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工人代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'OperatorID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'	工人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'OperatorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'熟悉的做工' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'Skill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'BIStatus';

