CREATE TABLE [dbo].[P_DailyRTLStatusByLineByStyle] (
    [TransferDate] DATE            NOT NULL,
    [MDivisionID]  VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_MDivisionID_New] DEFAULT ('') NOT NULL,
    [FactoryID]    VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_FactoryID_New] DEFAULT ('') NOT NULL,
    [APSNo]        INT             CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_APSNo_New] DEFAULT ((0)) NOT NULL,
    [SewingLineID] VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_SewingLineID_New] DEFAULT ('') NOT NULL,
    [BrandID]      VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_BrandID_New] DEFAULT ('') NOT NULL,
    [SeasonID]     VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_SeasonID_New] DEFAULT ('') NOT NULL,
    [StyleID]      VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_StyleID_New] DEFAULT ('') NOT NULL,
    [CurrentWIP]   INT             CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_CurrentWIP_New] DEFAULT ((0)) NOT NULL,
    [StdQty]       INT             CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_StdQty_New] DEFAULT ((0)) NOT NULL,
    [WIP]          NUMERIC (38, 2) CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_WIP_New] DEFAULT ((0)) NOT NULL,
    [nWIP]         NUMERIC (38, 2) CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_nWIP_New] DEFAULT ((0)) NOT NULL,
    [InLine]       DATE            NOT NULL,
    [OffLine]      DATE            NOT NULL,
    [NewCdCode]    VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_NewCdCode_New] DEFAULT ('') NOT NULL,
    [ProductType]  NVARCHAR (1000) CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_ProductType_New] DEFAULT ('') NOT NULL,
    [FabricType]   NVARCHAR (1000) CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_FabricType_New] DEFAULT ('') NOT NULL,
    [AlloQty]      INT             CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_AlloQty_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]  VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate] DATETIME        NULL,
    [BIStatus]     VARCHAR (8000)  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_DailyRTLStatusByLineByStyle] PRIMARY KEY CLUSTERED ([TransferDate] ASC, [FactoryID] ASC, [APSNo] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyRTLStatusByLineByStyle', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyRTLStatusByLineByStyle', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_DailyRTLStatusByLineByStyle', @level2type = N'COLUMN', @level2name = N'BIStatus';

