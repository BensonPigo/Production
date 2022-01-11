CREATE TABLE [dbo].[TransferExport_ShipAdvice_Container]
(
	[Ukey] BIGINT NOT NULL, 
    [TransferExport_DetailUkey] BIGINT NULL, 
    [ContainerType] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferExport_ShipAdvice_Container_ContainerType] DEFAULT (''),  
    [ContainerNo] VARCHAR(20) NOT NULL CONSTRAINT [DF_TransferExport_ShipAdvice_Container_ContainerNo] DEFAULT (''),   
    [AddName] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_ShipAdvice_Container_AddName] DEFAULT (''),  
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_ShipAdvice_Container_EditName] DEFAULT (''),  
    [EditDate] DATETIME NULL,  
    CONSTRAINT [PK_TransferExport_ShipAdvice_Container] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)
