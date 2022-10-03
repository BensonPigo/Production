CREATE TABLE [dbo].[ForwarderWarehouse] (
    [ID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [WhseCode] NVARCHAR (50) CONSTRAINT [DF_ForwarderWarehouse_WhseCode] DEFAULT ('') NOT NULL,
    [WhseName] NVARCHAR (50) CONSTRAINT [DF_ForwarderWarehouse_WhseName] DEFAULT ('') NOT NULL,
    [BrandID]  VARCHAR (MAX) CONSTRAINT [DF_ForwarderWarehouse_BrandID] DEFAULT ('') NOT NULL,
    [Address]  NVARCHAR(MAX) CONSTRAINT [DF_ForwarderWarehouse_Address] DEFAULT ('') NOT NULL,
    [Junk]     BIT           CONSTRAINT [DF_ForwarderWarehouse_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_ForwarderWarehouse_AddName] DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_ForwarderWarehouse_EditName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME      NULL,
    CONSTRAINT [PK_ForwarderWarehouse] PRIMARY KEY CLUSTERED ([WhseCode] ASC)
);

