CREATE TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn] (
    [POID]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_POID_New] DEFAULT ('') NOT NULL,
    [SEQ]                  VARCHAR (8000)  CONSTRAINT [DF_Table_1_採購單號_New] DEFAULT ('') NOT NULL,
    [Wkno]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Wkno_New] DEFAULT ('') NOT NULL,
    [ReceivingID]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_ReceivingID_New] DEFAULT ('') NOT NULL,
    [StyleID]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_StyleID_New] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_BrandID_New] DEFAULT ('') NOT NULL,
    [Supplier]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Supplier_New] DEFAULT ('') NOT NULL,
    [Refno]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Refno_New] DEFAULT ('') NOT NULL,
    [Color]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Color_New] DEFAULT ('') NOT NULL,
    [ArriveWHDate]         DATE            NULL,
    [ArriveQty]            NUMERIC (38, 2) CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_ArriveQty_New] DEFAULT ((0)) NOT NULL,
    [WeaveTypeID]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_WeaveTypeID_New] DEFAULT ('') NOT NULL,
    [Dyelot]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Dyelot_New] DEFAULT ('') NOT NULL,
    [CutWidth]             NUMERIC (38, 2) CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_CutWidth_New] DEFAULT ((0)) NOT NULL,
    [Weight]               NUMERIC (38, 1) CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Weight_New] DEFAULT ((0)) NOT NULL,
    [Composition]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Composition_New] DEFAULT ('') NOT NULL,
    [Desc]                 NVARCHAR (MAX)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Desc_New] DEFAULT ('') NOT NULL,
    [FabricConstructionID] VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_FabricConstructionID_New] DEFAULT ('') NOT NULL,
    [Roll]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Roll_New] DEFAULT ('') NOT NULL,
    [InspDate]             DATE            NULL,
    [Result]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Result_New] DEFAULT ('') NOT NULL,
    [Grade]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Grade_New] DEFAULT ('') NOT NULL,
    [DefectCode]           VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectCode_New] DEFAULT ('') NOT NULL,
    [DefectType]           VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectType_New] DEFAULT ('') NOT NULL,
    [DefectDesc]           VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectDesc_New] DEFAULT ('') NOT NULL,
    [Points]               INT             CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Points_New] DEFAULT ((0)) NOT NULL,
    [DefectRate]           NUMERIC (38, 2) CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_DefectRate_New] DEFAULT ((0)) NOT NULL,
    [Inspector]            NVARCHAR (1000) CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_Inspector_New] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME        NULL,
    [EditDate]             DATETIME        NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricInspReport_ReceivingTransferIn] PRIMARY KEY CLUSTERED ([POID] ASC, [SEQ] ASC, [ReceivingID] ASC, [Dyelot] ASC, [Roll] ASC, [DefectCode] ASC)
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


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大項-小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'SEQ'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作底稿編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Wkno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'ReceivingID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌
	' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠商代碼-英文簡稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Supplier'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Color
	' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Color'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫收料日/單據日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'ArriveWHDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'ArriveQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'織法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'WeaveTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Dyelot'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'幅寬' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'CutWidth'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'平方米重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Weight'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Composition' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Composition'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Desc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'組成代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'FabricConstructionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捲號
	' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Roll'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'檢驗日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'InspDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'結果' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Result'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'等級' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Grade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectCode' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'瑕疵種類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'英文描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Points' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Points'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectRate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'DefectRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Inspector' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'Inspector'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricInspReport_ReceivingTransferIn', @level2type = N'COLUMN', @level2name = N'BIStatus';

