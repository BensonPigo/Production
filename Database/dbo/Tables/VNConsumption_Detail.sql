CREATE TABLE [dbo].[VNConsumption_Detail] (
    [ID]         VARCHAR (13)    NOT NULL,
    [NLCode]     VARCHAR (5)     CONSTRAINT [DF_VNConsumption_Detail_NLCode] DEFAULT ('') NOT NULL,
    [HSCode]     VARCHAR (11)    CONSTRAINT [DF_VNConsumption_Detail_HSCode] DEFAULT ('') NULL,
    [UnitID]     VARCHAR (8)     CONSTRAINT [DF_VNConsumption_Detail_UnitID] DEFAULT ('') NULL,
    [Qty]        NUMERIC (17, 6) CONSTRAINT [DF_VNConsumption_Detail_Qty] DEFAULT ((0)) NULL,
    [UserCreate] BIT             CONSTRAINT [DF_VNConsumption_Detail_UserCreate] DEFAULT ((0)) NULL,
    [SystemQty]  NUMERIC (14, 3) CONSTRAINT [DF__VNConsump__Syste__0FE30493] DEFAULT ((0)) NULL,
    [Waste] NUMERIC(5, 3) NOT NULL, 
    CONSTRAINT [PK_VNConsumption_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [NLCode] ASC)
);





