CREATE TABLE [dbo].[P_SewingDailyOutputStatusRecord] (
    [SewingLineID]         VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SewingLineID_New] DEFAULT ('') NOT NULL,
    [SewingOutputDate]     DATE            NOT NULL,
    [MDivisionID]          VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FactoryID_New] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BrandID_New] DEFAULT ('') NOT NULL,
    [StyleID]              VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_StyleID_New] DEFAULT ('') NOT NULL,
    [SPNo]                 VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SPNo_New] DEFAULT ('') NOT NULL,
    [SeasonID]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SeasonID_New] DEFAULT ('') NOT NULL,
    [CDCodeNew]            VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CDCodeNew_New] DEFAULT ('') NOT NULL,
    [Article]              VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Article_New] DEFAULT ('') NOT NULL,
    [POID]                 VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_POID_New] DEFAULT ('') NOT NULL,
    [Category]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Category_New] DEFAULT ('') NOT NULL,
    [SCIDelivery]          DATE            NULL,
    [BuyerDelivery]        DATE            NULL,
    [OrderQty]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_OrderQty_New] DEFAULT ((0)) NOT NULL,
    [AlloQty]              INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AlloQty_New] DEFAULT ((0)) NOT NULL,
    [Artwork]              VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Artwork_New] DEFAULT ('') NOT NULL,
    [JITDate]              DATE            NULL,
    [BCSDate]              DATE            NULL,
    [SewingInLine]         DATE            NULL,
    [ReadyDate]            DATE            NULL,
    [SewingOffLine]        DATE            NULL,
    [StardardOutputPerDay] INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_StardardOutputPerDay_New] DEFAULT ((0)) NOT NULL,
    [WorkHourPerDay]       NUMERIC (38, 6) CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_WorkHourPerDay_New] DEFAULT ((0)) NOT NULL,
    [CuttingOutput]        INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CuttingOutput_New] DEFAULT ((0)) NOT NULL,
    [CuttingRemark]        NVARCHAR (1000) CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CuttingRemark_New] DEFAULT ('') NOT NULL,
    [Consumption]          NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Consumption_New] DEFAULT ((0)) NOT NULL,
    [ActConsOutput]        NUMERIC (38, 4) CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ActConsOutput_New] DEFAULT ((0)) NOT NULL,
    [LoadingOutput]        INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingOutput_New] DEFAULT ((0)) NOT NULL,
    [LoadingRemark]        VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingRemark_New] DEFAULT ('') NOT NULL,
    [LoadingExclusion]     BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingExclusion_New] DEFAULT ((0)) NOT NULL,
    [ATOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATOutput_New] DEFAULT ((0)) NOT NULL,
    [ATRemark]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATRemark_New] DEFAULT ('') NOT NULL,
    [ATExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATExclusion_New] DEFAULT ((0)) NOT NULL,
    [AUTOutput]            INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTOutput_New] DEFAULT ((0)) NOT NULL,
    [AUTRemark]            VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTRemark_New] DEFAULT ('') NOT NULL,
    [AUTExclusion]         BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTExclusion_New] DEFAULT ((0)) NOT NULL,
    [HTOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTOutput_New] DEFAULT ((0)) NOT NULL,
    [HTRemark]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTRemark_New] DEFAULT ('') NOT NULL,
    [HTExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTExclusion_New] DEFAULT ((0)) NOT NULL,
    [BOOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BOOutput_New] DEFAULT ((0)) NOT NULL,
    [BORemark]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BORemark_New] DEFAULT ('') NOT NULL,
    [BOExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BOExclusion_New] DEFAULT ((0)) NOT NULL,
    [FMOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMOutput_New] DEFAULT ((0)) NOT NULL,
    [FMRemark]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMRemark_New] DEFAULT ('') NOT NULL,
    [FMExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMExclusion_New] DEFAULT ((0)) NOT NULL,
    [PRTOutput]            INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTOutput_New] DEFAULT ((0)) NOT NULL,
    [PRTRemark]            VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTRemark_New] DEFAULT ('') NOT NULL,
    [PRTExclusion]         BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTExclusion_New] DEFAULT ((0)) NOT NULL,
    [PADPRTOutput]         INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PADPRTOutput_New] DEFAULT ((0)) NOT NULL,
    [PADPRTRemark]         VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PADPRTRemark_New] DEFAULT ('') NOT NULL,
    [PADPRTExclusion]      BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PADPRTExclusion_New] DEFAULT ((0)) NOT NULL,
    [EMBOutput]            INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_EMBOutput_New] DEFAULT ((0)) NOT NULL,
    [EMBRemark]            VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_EMBRemark_New] DEFAULT ('') NOT NULL,
    [EMBExclusion]         BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_EMBExclusion_New] DEFAULT ((0)) NOT NULL,
    [FIOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FIOutput_New] DEFAULT ((0)) NOT NULL,
    [FIRemark]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FIRemark_New] DEFAULT ('') NOT NULL,
    [FIExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FIExclusion_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SewingDailyOutputStatusRecord] PRIMARY KEY CLUSTERED ([SewingLineID] ASC, [SewingOutputDate] ASC, [SPNo] ASC, [FactoryID] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產線ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingOutputDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SPNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季節' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CDCodeNew' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'CDCodeNew'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'色組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Article'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'母單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'POID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Category'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SCIDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'買家交期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BuyerDelivery'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單件量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'OrderQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計畫分配生產數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AlloQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Artwork' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Artwork'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing InLine -14天(不含假日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'JITDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing InLine -2天(不含假日)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BCSDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫進線日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingInLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出貨預備日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ReadyDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫下線日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'SewingOffLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準產出/日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'StardardOutputPerDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工時/日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'WorkHourPerDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁床' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'CuttingOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁床備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'CuttingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'耗量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'Consumption'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實耗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ActConsOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loading不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'LoadingExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'ATExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AUT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AUT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AUT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'AUTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'HT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'HTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BOOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BO備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BORemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'BO不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BOExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FM備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FM不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FMExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRT' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRT備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRT不計' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PRTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT產出數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段PAD-PRT完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'PADPRTExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB產出數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段EMB完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'EMBExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI產出數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI供應量不足原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIRemark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工段FI完成率計算排除欄位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'FIExclusion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingDailyOutputStatusRecord', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BIStatus';

