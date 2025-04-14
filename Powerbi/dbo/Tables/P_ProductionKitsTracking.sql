CREATE TABLE [dbo].[P_ProductionKitsTracking] (
    [BrandID]             VARCHAR (8)     CONSTRAINT [DF_P_ProductionKitsTracking_BrandID] DEFAULT ('') NOT NULL,
    [StyleID]             VARCHAR (15)    CONSTRAINT [DF_P_ProductionKitsTracking_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]            VARCHAR (10)    CONSTRAINT [DF_P_ProductionKitsTracking_SeasonID] DEFAULT ('') NOT NULL,
    [Article]             NVARCHAR (1000) CONSTRAINT [DF_P_ProductionKitsTracking_Article] DEFAULT ('') NOT NULL,
    [Mdivision]           VARCHAR (8)     CONSTRAINT [DF_P_ProductionKitsTracking_Mdivision] DEFAULT ('') NOT NULL,
    [FactoryID]           VARCHAR (8)     CONSTRAINT [DF_P_ProductionKitsTracking_FactoryID] DEFAULT ('') NOT NULL,
    [Doc]                 NVARCHAR (506)  CONSTRAINT [DF_P_ProductionKitsTracking_Doc] DEFAULT ('') NOT NULL,
    [TWSendDate]          DATE            NULL,
    [FtyMRRcvDate]        DATE            NULL,
    [FtySendtoQADate]     DATE            NULL,
    [QARcvDate]           DATE            NULL,
    [UnnecessaryToSend]   VARCHAR (1)     CONSTRAINT [DF_P_ProductionKitsTracking_UnnecessaryToSend] DEFAULT ('') NOT NULL,
    [ProvideDate]         DATE            NULL,
    [SPNo]                VARCHAR (13)    CONSTRAINT [DF_P_ProductionKitsTracking_SPNo] DEFAULT ('') NOT NULL,
    [SCIDelivery]         DATE            NULL,
    [BuyerDelivery]       DATE            NULL,
    [Pullforward]         VARCHAR (1)     CONSTRAINT [DF_P_ProductionKitsTracking_Pullforward] DEFAULT ('') NOT NULL,
    [Handle]              VARCHAR (61)    CONSTRAINT [DF_P_ProductionKitsTracking_Handle] DEFAULT ('') NOT NULL,
    [MRHandle]            VARCHAR (61)    CONSTRAINT [DF_P_ProductionKitsTracking_MRHandle] DEFAULT ('') NOT NULL,
    [SMR]                 VARCHAR (61)    CONSTRAINT [DF_P_ProductionKitsTracking_SMR] DEFAULT ('') NOT NULL,
    [POHandle]            VARCHAR (61)    CONSTRAINT [DF_P_ProductionKitsTracking_POHandle] DEFAULT ('') NOT NULL,
    [POSMR]               VARCHAR (61)    CONSTRAINT [DF_P_ProductionKitsTracking_POSMR] DEFAULT ('') NOT NULL,
    [FtyHandle]           VARCHAR (41)    CONSTRAINT [DF_P_ProductionKitsTracking_FtyHandle] DEFAULT ('') NOT NULL,
    [ProductionKitsGroup] VARCHAR (8)     CONSTRAINT [DF_P_ProductionKitsTracking_ProductionKitsGroup] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME        NULL,
    [EditDate]            DATETIME        NULL,
    [Reject]              VARCHAR (1)     CONSTRAINT [DF_P_ProductionKitsTracking_Reject] DEFAULT ((0)) NOT NULL,
    [AWBNO]               VARCHAR (30)    CONSTRAINT [DF_P_ProductionKitsTracking_AWBNO] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_P_ProductionKitsTracking] PRIMARY KEY CLUSTERED ([Article] ASC, [FactoryID] ASC, [Doc] ASC, [SPNo] ASC, [ProductionKitsGroup] ASC)
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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.ProductionKitsGroup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'ProductionKitsGroup'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style_ProductionKits.EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProductionKitsTracking', @level2type=N'COLUMN',@level2name=N'EditDate'
GO