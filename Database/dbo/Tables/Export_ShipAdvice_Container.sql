CREATE TABLE [dbo].[Export_ShipAdvice_Container] (
    [Ukey]              BIGINT       NOT NULL,
    [ContainerType]     VARCHAR (2)  CONSTRAINT [DF_Export_ShipAdvice_ContainerType] DEFAULT ('') NOT NULL,
    [ContainerNo]       VARCHAR (20) CONSTRAINT [DF_Export_ShipAdvice_Container_ContainerNo] DEFAULT ('') NOT NULL,
    [AddName]           VARCHAR (10) CONSTRAINT [DF_Export_ShipAdvice_Container_AddName] DEFAULT ('') NOT NULL,
    [AddDate]           DATETIME     NULL,
    [EditName]          VARCHAR (10) CONSTRAINT [DF_Export_ShipAdvice_Container_EditName] DEFAULT ('') NOT NULL,
    [EditDate]          DATETIME     NULL,
    [Export_DetailUkey] BIGINT       CONSTRAINT [DF_Export_ShipAdvice_Container_Export_DetailUkey] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Export_ShipAdvice_Container] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO

	
GO
	
	
GO
	
	
GO
	
	
GO
CREATE NONCLUSTERED INDEX [IDX_Export_ShipAdvice_Container_Export_DetailUkey] ON [dbo].[Export_ShipAdvice_Container]
(
	[Export_DetailUkey] ASC
)
go