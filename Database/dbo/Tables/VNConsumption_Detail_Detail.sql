CREATE TABLE [dbo].[VNConsumption_Detail_Detail] (
    [ID]            VARCHAR (13)    NOT NULL,
    [NLCode]        VARCHAR (9)     CONSTRAINT [DF_VNConsumption_Detail _Detail_NLCode] DEFAULT ('') NOT NULL,
    [SCIRefno]      VARCHAR (30)    CONSTRAINT [DF_VNConsumption_Detail _Detail_SCIRefno] DEFAULT ('') NOT NULL,
    [RefNo]         VARCHAR (36)    CONSTRAINT [DF_VNConsumption_Detail _Detail_RefNo] DEFAULT ('') NOT NULL,
    [Qty]           NUMERIC (12, 4) CONSTRAINT [DF_VNConsumption_Detail _Detail_Qty] DEFAULT ((0)) NOT NULL,
    [LocalItem]     BIT             CONSTRAINT [DF_VNConsumption_Detail _Detail_LocalItem] DEFAULT ((0)) NULL,
    [FabricBrandID] VARCHAR (8)     DEFAULT ('') NULL,
    [FabricType]    VARCHAR (4)     DEFAULT ('') NULL,
    [UKey]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [OldFabricUkey] VARCHAR(10) NULL DEFAULT (''), 
    [OldFabricVer] VARCHAR(2) NULL DEFAULT (''), 
    [SystemQty] NUMERIC(12, 4) CONSTRAINT [DF_VNConsumption_Detail_Detail_SystemQty] DEFAULT ((0)) NOT NULL, 
	[StockQty] NUMERIC(12, 4) CONSTRAINT [DF_VNConsumption_Detail_Detail_StockQty] DEFAULT ((0)) NOT NULL, 
	[StockUnit] VARCHAR(8) CONSTRAINT [DF_VNConsumption_Detail_Detail_StockUnit] DEFAULT (('')) NOT NULL,
	[UserCreate] BIT             CONSTRAINT [DF_VNConsumption_Detail_Detail_UserCreate] DEFAULT ((0)) NULL,
	[HSCode]     VARCHAR (11)    CONSTRAINT [DF_VNConsumption_Detail_Detail_HSCode] DEFAULT ('') NULL,
	[UnitID]     VARCHAR (8)     CONSTRAINT [DF_VNConsumption_Detail_Detail_UnitID] DEFAULT ('') NULL,
	[UsageQty] NUMERIC(12, 4) CONSTRAINT [DF_VNConsumption_Detail_Detail_UsageQty] DEFAULT ((0)) NOT NULL, 
	[UsageUnit] VARCHAR(8) CONSTRAINT [DF_VNConsumption_Detail_Detail_UsageUnit] DEFAULT (('')) NOT NULL,
    CONSTRAINT [PK_VNConsumption_Detail_Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);



