CREATE TABLE [dbo].[ShippingReason] (
    [Type]        VARCHAR (2)   CONSTRAINT [DF_ShippingReason_Type] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (5)   CONSTRAINT [DF_ShippingReason_ID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60) CONSTRAINT [DF_ShippingReason_Description] DEFAULT ('') NOT NULL,
    [Junk]        BIT           CONSTRAINT [DF_ShippingReason_Junk] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_ShippingReason_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_ShippingReason_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    [IsFtyWK]     BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ShippingReason] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);

