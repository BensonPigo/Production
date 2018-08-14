CREATE TABLE [dbo].[SubconOutContract_Detail]
(
	[SubConOutFty] VARCHAR(8) NOT NULL , 
    [ContractNumber] VARCHAR(50) NOT NULL, 
	[OrderId] VARCHAR(13) NOT NULL, 
    [ComboType] VARCHAR NOT NULL, 
    [Article] VARCHAR(8) NOT NULL, 
    [OutputQty] INT CONSTRAINT [DF_SubconOutContract_Detail_OutputQty] DEFAULT (0) NULL,
    [UnitPrice] NUMERIC(16, 4) CONSTRAINT [DF_SubconOutContract_Detail_UnitPrice] DEFAULT (0) NULL,
    CONSTRAINT [PK_SubconOutContract_Detail] PRIMARY KEY CLUSTERED ([SubConOutFty], [ContractNumber], [OrderId], [ComboType], [Article])
)
