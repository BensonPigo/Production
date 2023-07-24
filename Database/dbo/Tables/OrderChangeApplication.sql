CREATE TABLE [dbo].[OrderChangeApplication] (
    [ID]                   VARCHAR (13)   NOT NULL,
    [ReasonID]             VARCHAR (5)    CONSTRAINT [DF_OrderChangeApplication_ReasonID] DEFAULT ('') NOT NULL,
    [OrderID]              VARCHAR (13)   CONSTRAINT [DF_OrderChangeApplication_OrderID] DEFAULT ('') NOT NULL,
    [Status]               VARCHAR (15)   CONSTRAINT [DF_OrderChangeApplication_Status] DEFAULT ('') NOT NULL,
    [SentName]             VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_SentName] DEFAULT ('') NOT NULL,
    [SentDate]             DATETIME       NULL,
    [ApprovedName]         VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_ApprovedName] DEFAULT ('') NOT NULL,
    [ApprovedDate]         DATETIME       NULL,
    [ConfirmedName]        VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_ConfirmedName] DEFAULT ('') NOT NULL,
    [ConfirmedDate]        DATETIME       NULL,
    [RejectName]           VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_RejectName] DEFAULT ('') NOT NULL,
    [RejectDate]           DATETIME       NULL,
    [ClosedName]           VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_ClosedName] DEFAULT ('') NOT NULL,
    [ClosedDate]           DATETIME       NULL,
    [JunkName]             VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_JunkName] DEFAULT ('') NOT NULL,
    [JunkDate]             DATETIME       NULL,
    [AddName]              VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_AddName] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME       NULL,
    [EditName]             VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_EditName] DEFAULT ('') NOT NULL,
    [EditDate]             DATETIME       NULL,
    [ToOrderID]            VARCHAR (13)   CONSTRAINT [DF_OrderChangeApplication_ToOrderID] DEFAULT ('') NOT NULL,
    [NeedProduction]       BIT            CONSTRAINT [DF_OrderChangeApplication_NeedProduction] DEFAULT ((0)) NOT NULL,
    [OldQty]               NUMERIC (6)    CONSTRAINT [DF_OrderChangeApplication_OldQty] DEFAULT ((0)) NOT NULL,
    [RatioFty]             NUMERIC (5, 2) CONSTRAINT [DF_OrderChangeApplication_RatioFty] DEFAULT ((0)) NOT NULL,
    [RatioSubcon]          NUMERIC (5, 2) CONSTRAINT [DF_OrderChangeApplication_RatioSubcon] DEFAULT ((0)) NOT NULL,
    [RatioSCI]             NUMERIC (5, 2) CONSTRAINT [DF_OrderChangeApplication_RatioSCI] DEFAULT ((0)) NOT NULL,
    [RatioSupp]            NUMERIC (5, 2) CONSTRAINT [DF_OrderChangeApplication_RatioSupp] DEFAULT ((0)) NOT NULL,
    [RatioBuyer]           NUMERIC (5, 2) CONSTRAINT [DF_OrderChangeApplication_RatioBuyer] DEFAULT ((0)) NOT NULL,
    [ResponsibleFty]       BIT            CONSTRAINT [DF_OrderChangeApplication_ResponsibleFty] DEFAULT ((0)) NOT NULL,
    [ResponsibleSubcon]    BIT            CONSTRAINT [DF_OrderChangeApplication_ResponsibleSubcon] DEFAULT ((0)) NOT NULL,
    [ResponsibleSCI]       BIT            CONSTRAINT [DF_OrderChangeApplication_ResponsibleSCI] DEFAULT ((0)) NOT NULL,
    [ResponsibleSupp]      BIT            CONSTRAINT [DF_OrderChangeApplication_ResponsibleSupp] DEFAULT ((0)) NOT NULL,
    [ResponsibleBuyer]     BIT            CONSTRAINT [DF_OrderChangeApplication_ResponsibleBuyer] DEFAULT ((0)) NOT NULL,
    [FactoryICRDepartment] NVARCHAR (40)  CONSTRAINT [DF_OrderChangeApplication_FactoryICRDepartment] DEFAULT ('') NOT NULL,
    [FactoryICRNo]         NVARCHAR (13)  CONSTRAINT [DF_OrderChangeApplication_FactoryICRNo] DEFAULT ('') NOT NULL,
    [FactoryICRRemark]     NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_FactoryICRRemark] DEFAULT ('') NOT NULL,
    [SubconDBCNo]          NVARCHAR (13)  CONSTRAINT [DF_OrderChangeApplication_SubconDBCNo] DEFAULT ('') NOT NULL,
    [SubconDBCRemark]      NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_SubconDBCRemark] DEFAULT ('') NOT NULL,
    [SubConName]           NVARCHAR (20)  CONSTRAINT [DF_OrderChangeApplication_SubConName] DEFAULT ('') NOT NULL,
    [SCIICRDepartment]     NVARCHAR (40)  CONSTRAINT [DF_OrderChangeApplication_SCIICRDepartment] DEFAULT ('') NOT NULL,
    [SCIICRNo]             NVARCHAR (13)  CONSTRAINT [DF_OrderChangeApplication_SCIICRNo] DEFAULT ('') NOT NULL,
    [SCIICRRemark]         NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_SCIICRRemark] DEFAULT ('') NOT NULL,
    [SuppDBCNo]            NVARCHAR (13)  CONSTRAINT [DF_OrderChangeApplication_SuppDBCNo] DEFAULT ('') NOT NULL,
    [SuppDBCRemark]        NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_SuppDBCRemark] DEFAULT ('') NOT NULL,
    [BuyerDBCDepartment]   NVARCHAR (40)  CONSTRAINT [DF_OrderChangeApplication_BuyerDBCDepartment] DEFAULT ('') NOT NULL,
    [BuyerDBCNo]           NVARCHAR (13)  CONSTRAINT [DF_OrderChangeApplication_BuyerDBCNo] DEFAULT ('') NOT NULL,
    [BuyerDBCRemark]       NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_BuyerDBCRemark] DEFAULT ('') NOT NULL,
    [BuyerICRNo]           NVARCHAR (13)  CONSTRAINT [DF_OrderChangeApplication_BuyerICRNo] DEFAULT ('') NOT NULL,
    [BuyerICRRemark]       NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_BuyerICRRemark] DEFAULT ('') NOT NULL,
    [MRComment]            NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_MRComment] DEFAULT ('') NOT NULL,
    [Remark]               NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_Remark] DEFAULT ('') NOT NULL,
    [BuyerRemark]          NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_BuyerRemark] DEFAULT ('') NOT NULL,
    [FTYComments]          NVARCHAR (MAX) CONSTRAINT [DF_OrderChangeApplication_FTYComments] DEFAULT ('') NOT NULL,
    [FactoryID]            VARCHAR (8)    DEFAULT ('') NOT NULL,
    [TPEEditName]          VARCHAR (10)   CONSTRAINT [DF_OrderChangeApplication_TPEEditName] DEFAULT ('') NOT NULL,
    [TPEEditDate]          DATETIME       NULL,
    [KeepPanels]           BIT            CONSTRAINT [DF_OrderChangeApplication_KeepPanels] DEFAULT ((0)) NOT NULL,
    [GMCheck]              BIT            CONSTRAINT [DF_OrderChangeApplication_GMCheck] DEFAULT ((0)) NOT NULL,
    [OCTaskSchedulerDate]  DATE           NULL,
    CONSTRAINT [PK_OrderChangeApplication] PRIMARY KEY CLUSTERED ([ID] ASC)
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

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北最後編輯人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北最後編輯日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'OrderChangeApplication',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditDate'