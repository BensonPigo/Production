CREATE TABLE [dbo].[RFIDProcessLocation]
(
	[ID] VARCHAR(15) NOT NULL , 
    [Description] VARCHAR(50) CONSTRAINT [DF_RFIDProcessLocation_Description] DEFAULT ('') NOT NULL, 
    [Junk] BIT CONSTRAINT [DF_RFIDProcessLocation_Junk] DEFAULT (0) NOT NULL, 
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_RFIDProcessLocation_AddName] DEFAULT ('') NOT NULL, 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_RFIDProcessLocation_EditName] DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_RFIDProcessLocation] PRIMARY KEY CLUSTERED ([ID] ASC)
)
