CREATE TABLE [dbo].[SubconReason]
(
	[Type] VARCHAR(2) NOT NULL, 
    [ID] VARCHAR(5) NOT NULL, 
    [Reason] VARCHAR(50) NOT NULL, 
    [Responsible] VARCHAR(50) NULL, 
    [Junk] BIT NULL  CONSTRAINT [DF_SubconReason_Junk] DEFAULT (0) ,  
    [AddDate] DATETIME NULL, 
    [AddName] VARCHAR(10) NULL CONSTRAINT [DF_SubconReason_AddName] DEFAULT ('') , 
    [EditDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL CONSTRAINT [DF_SubconReason_EditName] DEFAULT ('') , 
    CONSTRAINT [PK_SubconReason] PRIMARY KEY CLUSTERED ([Type],[ID] ASC)
)
