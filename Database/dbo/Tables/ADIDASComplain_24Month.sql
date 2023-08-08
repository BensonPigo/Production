CREATE TABLE [dbo].[ADIDASComplain_24Month]
(
	[Year] VARCHAR(4) NOT NULL DEFAULT (('')), 
    [Month] VARCHAR(2) NOT NULL DEFAULT (('')), 
    [BrandFtyCode] VARCHAR(10) NOT NULL DEFAULT (('')), 
    [FactoryName] VARCHAR(40) NOT NULL DEFAULT (('')), 
    [KPILO] VARCHAR(30) NOT NULL DEFAULT (('')), 
    [PV_RAW_24Month] NUMERIC(18, 4) NOT NULL DEFAULT ((0)), 
    [SV_RAW_24Month] NUMERIC(18, 4) NOT NULL DEFAULT ((0)), 
    [WHC] NUMERIC(18, 4) NOT NULL DEFAULT ((0)), 
    [Defective_Return] NUMERIC(18, 4) NOT NULL DEFAULT ((0)), 
    [AddName] VARCHAR(10) NOT NULL DEFAULT (('')), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL DEFAULT (('')), 
    [EditDate] DATETIME NULL
)
