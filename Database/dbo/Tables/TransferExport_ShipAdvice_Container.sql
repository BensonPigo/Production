CREATE TABLE [dbo].[TransferExport_ShipAdvice_Container] (
    [Ukey]                       BIGINT       NOT NULL,
    [TransferExport_Detail_Ukey] BIGINT       CONSTRAINT [DF_TransferExport_ShipAdvice_Container_TransferExport_Detail_Ukey] DEFAULT ((0)) NOT NULL,
    [ContainerType]              VARCHAR (2)  CONSTRAINT [DF_TransferExport_ShipAdvice_Container_ContainerType] DEFAULT ('') NOT NULL,
    [ContainerNo]                VARCHAR (20) CONSTRAINT [DF_TransferExport_ShipAdvice_Container_ContainerNo] DEFAULT ('') NOT NULL,
    [AddName]                    VARCHAR (10) CONSTRAINT [DF_TransferExport_ShipAdvice_Container_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                    DATETIME     NULL,
    [EditName]                   VARCHAR (10) CONSTRAINT [DF_TransferExport_ShipAdvice_Container_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                   DATETIME     NULL,
    CONSTRAINT [PK_TransferExport_ShipAdvice_Container] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


