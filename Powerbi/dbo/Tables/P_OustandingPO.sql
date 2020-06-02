CREATE TABLE [dbo].[P_OustandingPO] (
    [FactoryID]         VARCHAR (8)  CONSTRAINT [DF_P_OustandingPO_FactoryID] DEFAULT ('') NOT NULL,
    [OrderID]           VARCHAR (13) CONSTRAINT [DF_P_OustandingPO_OrderID] DEFAULT ('') NOT NULL,
    [CustPONo]          VARCHAR (30) CONSTRAINT [DF_P_OustandingPO_CustPONo] DEFAULT ('') NOT NULL,
    [StyleID]           VARCHAR (15) CONSTRAINT [DF_P_OustandingPO_StyleID] DEFAULT ('') NOT NULL,
    [BrandID]           VARCHAR (8)  CONSTRAINT [DF_P_OustandingPO_BrandID] DEFAULT ('') NOT NULL,
    [BuyerDelivery]     DATE         NULL,
    [Seq]               VARCHAR (2)  CONSTRAINT [DF_P_OustandingPO_Seq] DEFAULT ('') NOT NULL,
    [ShipModeID]        VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_ShipModeID] DEFAULT ('') NOT NULL,
    [Category]          VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_Category] DEFAULT ('') NOT NULL,
    [PartialShipment]   VARCHAR (1)  CONSTRAINT [DF_P_OustandingPO_PartialShipment] DEFAULT ('') NOT NULL,
    [Junk]              VARCHAR (1)  CONSTRAINT [DF_P_OustandingPO_Junk] DEFAULT ('') NOT NULL,
    [OrderQty]          INT          CONSTRAINT [DF_P_OustandingPO_OrderQty] DEFAULT ((0)) NOT NULL,
    [PackingCtn]        INT          CONSTRAINT [DF_P_OustandingPO_PackingCtn] DEFAULT ((0)) NOT NULL,
    [PackingQty]        INT          CONSTRAINT [DF_P_OustandingPO_PackingQty] DEFAULT ((0)) NOT NULL,
    [ClogRcvCtn]        INT          CONSTRAINT [DF_P_OustandingPO_ClogRcvCtn] DEFAULT ((0)) NOT NULL,
    [ClogRcvQty]        VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_ClogRcvQty] DEFAULT ((0)) NOT NULL,
    [LastCMPOutputDate] DATE         NULL,
    [CMPQty]            VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_CMPQty] DEFAULT ((0)) NOT NULL,
    [LastDQSOutputDate] DATE         NULL,
    [DQSQty]            VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_DQSQty] DEFAULT ('') NOT NULL,
    [OSTPackingQty]     VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_OSTPackingQty] DEFAULT ('') NOT NULL,
    [OSTCMPQty]         VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_OSTCMPQty] DEFAULT ('') NOT NULL,
    [OSTDQSQty]         VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_OSTDQSQty] DEFAULT ('') NOT NULL,
    [OSTClogQty]        VARCHAR (10) CONSTRAINT [DF_P_OustandingPO_OSTClogQty] DEFAULT ('') NOT NULL,
    [OSTClogCtn]        INT          CONSTRAINT [DF_P_OustandingPO_OSTClogCtn] DEFAULT ((0)) NOT NULL,
    [PulloutComplete]   VARCHAR (1)  CONSTRAINT [DF_P_OustandingPO_PulloutCompleted] DEFAULT ('') NOT NULL,
    [Dest]              VARCHAR (30) NULL,
    CONSTRAINT [PK_P_OustandingPO] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [OrderID] ASC, [Seq] ASC)
);



