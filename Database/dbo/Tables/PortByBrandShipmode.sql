	CREATE TABLE [dbo].[PortByBrandShipmode] (
    [PulloutPortID] VARCHAR (20)   NOT NULL,
    [BrandID]       VARCHAR (80)   NOT NULL,
    [Remark]        NVARCHAR (MAX) CONSTRAINT [DF_PortByBrandShipmode_Remark] DEFAULT ('') NOT NULL,
    [Junk]          BIT            CONSTRAINT [DF_PortByBrandShipmode_Junk] DEFAULT ((0)) NOT NULL,
    [AddDate]       DATETIME       NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_PortByBrandShipmode_AddName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_PortByBrandShipmode_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_PortByBrandShipmode] PRIMARY KEY CLUSTERED ([PulloutPortID] ASC, [BrandID] ASC)
);


GO