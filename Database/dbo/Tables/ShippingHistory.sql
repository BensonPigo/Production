CREATE TABLE [dbo].[ShippingHistory]
(
	[Ukey] bigint NOT NULL IDENTITY, 
    [ID] VARCHAR(13) CONSTRAINT [DF_ShippingHistory_ID] DEFAULT ('') NOT NULL,
    [MDivisionID] VARCHAR(8) CONSTRAINT [DF_ShippingHistory_MDivisionID] DEFAULT ('') NOT NULL,
    [Type] VARCHAR(60) CONSTRAINT [DF_ShippingHistory_Type] DEFAULT ('') NOT NULL,
    [ReasonTypeID] VARCHAR(50) CONSTRAINT [DF_ShippingHistory_ReasonTypeID] DEFAULT ('') NOT NULL,
    [ReasonID] VARCHAR(5) CONSTRAINT [DF_ShippingHistory_ReasonID] DEFAULT ('') NOT NULL,
    [Remark] VARCHAR(300) CONSTRAINT [DF_ShippingHistory_Remark] DEFAULT ('') NOT NULL,
    [AddName] VARCHAR(10) CONSTRAINT [DF_ShippingHistory_AddName] DEFAULT ('') NOT NULL,
    [AddDate] DATETIME NULL,
    CONSTRAINT [PK_ShippingHistory] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)
