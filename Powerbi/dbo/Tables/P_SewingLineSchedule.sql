CREATE TABLE [dbo].[P_SewingLineSchedule] (
    [APSNo]                    INT             NULL,
    [SewingLineID]             VARCHAR (8000)  NULL,
    [SewingDay]                DATE            NULL,
    [SewingStartTime]          DATETIME        NULL,
    [SewingEndTime]            DATETIME        NULL,
    [MDivisionID]              VARCHAR (8000)  NULL,
    [FactoryID]                VARCHAR (8000)  NOT NULL,
    [PO]                       NVARCHAR (MAX)  NULL,
    [POCount]                  BIGINT          NULL,
    [SP]                       NVARCHAR (MAX)  NULL,
    [SPCount]                  INT             NULL,
    [EarliestSCIdelivery]      DATE            NULL,
    [LatestSCIdelivery]        DATE            NULL,
    [EarliestBuyerdelivery]    DATE            NULL,
    [LatestBuyerdelivery]      DATE            NULL,
    [Category]                 NVARCHAR (MAX)  NULL,
    [Colorway]                 NVARCHAR (MAX)  NULL,
    [ColorwayCount]            BIGINT          NULL,
    [CDCode]                   NVARCHAR (MAX)  NULL,
    [ProductionFamilyID]       NVARCHAR (MAX)  NULL,
    [Style]                    NVARCHAR (MAX)  NULL,
    [StyleCount]               BIGINT          NULL,
    [OrderQty]                 INT             NULL,
    [AlloQty]                  INT             NULL,
    [StardardOutputPerDay]     FLOAT (53)      NULL,
    [CPU]                      FLOAT (53)      NULL,
    [WorkHourPerDay]           FLOAT (53)      NULL,
    [StardardOutputPerHour]    FLOAT (53)      NULL,
    [Efficienycy]              NUMERIC (38, 2) NULL,
    [ScheduleEfficiency]       NUMERIC (38, 2) NULL,
    [LineEfficiency]           NUMERIC (38, 2) NULL,
    [LearningCurve]            NUMERIC (38, 2) NULL,
    [SewingInline]             DATETIME        NULL,
    [SewingOffline]            DATETIME        NULL,
    [PFRemark]                 VARCHAR (8000)  NULL,
    [MTLComplete]              VARCHAR (8000)  NULL,
    [KPILETA]                  DATE            NULL,
    [MTLETA]                   DATE            NULL,
    [ArtworkType]              NVARCHAR (MAX)  NULL,
    [InspectionDate]           DATE            NULL,
    [Remarks]                  NVARCHAR (MAX)  NULL,
    [CuttingOutput]            NUMERIC (38, 2) NULL,
    [SewingOutput]             INT             NULL,
    [ScannedQty]               INT             NULL,
    [ClogQty]                  INT             NULL,
    [Sewer]                    INT             NULL,
    [SewingCPU]                NUMERIC (38, 5) NULL,
    [BrandID]                  NVARCHAR (1000) NULL,
    [Orig_WorkHourPerDay]      FLOAT (53)      NULL,
    [New_SwitchTime]           FLOAT (53)      NULL,
    [FirststCuttingOutputDate] DATE            NULL,
    [TTL_PRINTING (PCS)]       NUMERIC (38, 6) NULL,
    [TTL_PRINTING PPU (PPU)]   NUMERIC (38, 6) NULL,
    [SubCon]                   NVARCHAR (MAX)  NULL,
    [CDCodeNew]                VARCHAR (8000)  NULL,
    [ProductType]              NVARCHAR (MAX)  NULL,
    [FabricType]               NVARCHAR (MAX)  NULL,
    [Lining]                   VARCHAR (8000)  NULL,
    [Gender]                   VARCHAR (8000)  NULL,
    [Construction]             NVARCHAR (MAX)  NULL,
    [Subcon Qty]               INT             NULL,
    [Std Qty for printing]     INT             NULL,
    [StyleName]                NVARCHAR (MAX)  CONSTRAINT [DF_P_SewingLineSchedule_StyleName_New] DEFAULT ('') NULL,
    [StdQtyEMB]                VARCHAR (8000)  NULL,
    [EMBStitch]                VARCHAR (8000)  NULL,
    [EMBStitchCnt]             INT             NULL,
    [TtlQtyEMB]                INT             NULL,
    [PrintPcs]                 INT             NULL,
    [Ukey]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [InlineCategory]           VARCHAR (8000)  NULL,
    [StyleSeason]              NVARCHAR (MAX)  CONSTRAINT [DF_P_SewingLineSchedule_StyleSeason_New] DEFAULT ('') NOT NULL,
    [AddDate]                  DATETIME        NULL,
    [EditDate]                 DATETIME        NULL,
    [LastDownloadAPSDate]      DATETIME        NULL,
    [SewingInlineCategory]     VARCHAR (8000)  CONSTRAINT [DF__P_SewingL__Sewin__3B177B5B_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]              VARCHAR (8000)  CONSTRAINT [DF_P_SewingLineSchedule_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]             DATETIME        NULL,
    [BIStatus]                 VARCHAR (8000)  CONSTRAINT [DF_P_SewingLineSchedule_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SewingLineSchedule] PRIMARY KEY CLUSTERED ([Ukey] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'StyleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingLineSchedule.AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingLineSchedule.EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sewing output InlineCategory By Style' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'SewingInlineCategory'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SewingLineSchedule', @level2type = N'COLUMN', @level2name = N'BIStatus';

