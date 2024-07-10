CREATE TABLE [dbo].[P_RTLStatusByDay] (
	TransferDate            DATE          NOT  NULL,
	FactoryID               VARCHAR (8)   CONSTRAINT [DF_P_RTLStatusByDay_FactoryID] DEFAULT ('') NOT NULL,
	CurrentWIPDays          numeric(5,2)  CONSTRAINT [DF_P_RTLStatusByDay_CurrentWIPDays] DEFAULT ((0)) NOT NULL,
	CONSTRAINT [PK_RTLStatusByDay] PRIMARY KEY (TransferDate,FactoryID,CurrentWIPDays)
)
GO