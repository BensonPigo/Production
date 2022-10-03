CREATE TABLE [dbo].[ForwarderWarehouse_Detail] (
    [ID]          BIGINT         NOT NULL,
    [ShipModeID]  VARCHAR (10)   CONSTRAINT [DF_ForwarderWarehouse_Detail_ShipModeID] DEFAULT ('') NOT NULL,
    [LoadingType] NVARCHAR (200) CONSTRAINT [DF_ForwarderWarehouse_Detail_LoadingType] DEFAULT ('') NOT NULL,
    [UKey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ForwarderWarehouse_Detail] PRIMARY KEY CLUSTERED ([ID] ASC)
);

