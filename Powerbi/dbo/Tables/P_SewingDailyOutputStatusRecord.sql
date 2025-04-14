CREATE TABLE [dbo].[P_SewingDailyOutputStatusRecord] (
    [SewingLineID]         VARCHAR (5)     CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SewingLineID] DEFAULT ('') NOT NULL,
    [SewingOutputDate]     DATE            NOT NULL,
    [MDivisionID]          VARCHAR (8)     CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]            VARCHAR (8)     CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FactoryID] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8)     CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BrandID] DEFAULT ('') NOT NULL,
    [StyleID]              VARCHAR (15)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_StyleID] DEFAULT ('') NOT NULL,
    [SPNo]                 VARCHAR (13)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SPNo] DEFAULT ('') NOT NULL,
    [SeasonID]             VARCHAR (10)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_SeasonID] DEFAULT ('') NOT NULL,
    [CDCodeNew]            VARCHAR (5)     CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CDCodeNew] DEFAULT ('') NOT NULL,
    [Article]              VARCHAR (200)   CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Article] DEFAULT ('') NOT NULL,
    [POID]                 VARCHAR (13)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_POID] DEFAULT ('') NOT NULL,
    [Category]             VARCHAR (1)     CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Category] DEFAULT ('') NOT NULL,
    [SCIDelivery]          DATE            NULL,
    [BuyerDelivery]        DATE            NULL,
    [OrderQty]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_OrderQty] DEFAULT ((0)) NOT NULL,
    [AlloQty]              INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AlloQty] DEFAULT ((0)) NOT NULL,
    [Artwork]              VARCHAR (200)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Artwork] DEFAULT ('') NOT NULL,
    [JITDate]              DATE            NULL,
    [BCSDate]              DATE            NULL,
    [SewingInLine]         DATE            NULL,
    [ReadyDate]            DATE            NULL,
    [SewingOffLine]        DATE            NULL,
    [StardardOutputPerDay] INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_StardardOutputPerDay] DEFAULT ((0)) NOT NULL,
    [WorkHourPerDay]       NUMERIC (11, 6) CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_WorkHourPerDay] DEFAULT ((0)) NOT NULL,
    [CuttingOutput]        INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CuttingOutput] DEFAULT ((0)) NOT NULL,
    [CuttingRemark]        NVARCHAR (50)   CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_CuttingRemark] DEFAULT ('') NOT NULL,
    [Consumption]          NUMERIC(18, 4)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_Consumption] DEFAULT ((0)) NOT NULL,
    [ActConsOutput]        NUMERIC (18, 4)  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ActConsOutput] DEFAULT ((0)) NOT NULL,
    [LoadingOutput]        INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingOutput] DEFAULT ((0)) NOT NULL,
    [LoadingRemark]        VARCHAR (30)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingRemark] DEFAULT ('') NOT NULL,
    [LoadingExclusion]     BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_LoadingExclusion] DEFAULT ((0)) NOT NULL,
    [ATOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATOutput] DEFAULT ((0)) NOT NULL,
    [ATRemark]             VARCHAR (30)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATRemark] DEFAULT ('') NOT NULL,
    [ATExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_ATExclusion] DEFAULT ((0)) NOT NULL,
    [AUTOutput]            INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTOutput] DEFAULT ((0)) NOT NULL,
    [AUTRemark]            VARCHAR (30)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTRemark] DEFAULT ('') NOT NULL,
    [AUTExclusion]         BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_AUTExclusion] DEFAULT ((0)) NOT NULL,
    [HTOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTOutput] DEFAULT ((0)) NOT NULL,
    [HTRemark]             VARCHAR (30)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTRemark] DEFAULT ('') NOT NULL,
    [HTExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_HTExclusion] DEFAULT ((0)) NOT NULL,
    [BOOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BOOutput] DEFAULT ((0)) NOT NULL,
    [BORemark]             VARCHAR (30)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BORemark] DEFAULT ('') NOT NULL,
    [BOExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_BOExclusion] DEFAULT ((0)) NOT NULL,
    [FMOutput]             INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMOutput] DEFAULT ((0)) NOT NULL,
    [FMRemark]             VARCHAR (30)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMRemark] DEFAULT ('') NOT NULL,
    [FMExclusion]          BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_FMExclusion] DEFAULT ((0)) NOT NULL,
    [PRTOutput]            INT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTOutput] DEFAULT ((0)) NOT NULL,
    [PRTRemark]            VARCHAR (30)    CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTRemark] DEFAULT ('') NOT NULL,
    [PRTExclusion]         BIT             CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_PRTExclusion] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([SewingLineID] ASC, [SewingOutputDate] ASC, [SPNo] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PRT不計', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'PRTExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PRT備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'PRTRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PRT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'PRTOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FM不計', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'FMExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FM備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'FMRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'FMOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BO不計', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BOExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BO備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BORemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BOOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HT不計', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'HTExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HT備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'HTRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'HTOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AUT不計', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'AUTExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AUT備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'AUTRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AUT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'AUTOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AT不計', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'ATExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AT備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'ATRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'ATOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loading不計', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'LoadingExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loading備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'LoadingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loading', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'LoadingOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實耗', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'ActConsOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'耗量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'Consumption';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁床備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'CuttingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁床', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'CuttingOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工時/日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'WorkHourPerDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標準產出/日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'StardardOutputPerDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫下線日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingOffLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨預備日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'ReadyDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫進線日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingInLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing InLine -2天(不含假日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BCSDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing InLine -14天(不含假日)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'JITDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artwork', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'Artwork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'計畫分配生產數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'AlloQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單件量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'OrderQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'買家交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SCIDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CDCodeNew', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'CDCodeNew';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SPNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產出日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingOutputDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingLineID';

