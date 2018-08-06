CREATE TABLE [dbo].[SubconOutContract]
(
	[SubConOutFty] VARCHAR(8) NOT NULL , 
    [ContractNumber] VARCHAR(50) NOT NULL, 
    [MDivisionID] VARCHAR(8) CONSTRAINT [DF_SubconOutContract_MDivisionID] DEFAULT ('') NULL,
    [Factoryid] VARCHAR(8) CONSTRAINT [DF_SubconOutContract_Factoryid] DEFAULT ('') NULL,
    [IssueDate] DATE NULL, 
    [Status] VARCHAR(15) CONSTRAINT [DF_SubconOutContract_Status] DEFAULT ('') NULL,
    [Remark] VARCHAR(500) CONSTRAINT [DF_SubconOutContract_Remark] DEFAULT ('') NULL,
    [ApvName] VARCHAR(10) CONSTRAINT [DF_SubconOutContract_ApvName] DEFAULT ('') NULL,
    [ApvDate] DATETIME NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_SubconOutContract_AddName] DEFAULT ('') NULL,
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SubconOutContract_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME NULL, 
	CONSTRAINT [PK_SubconOutContract] PRIMARY KEY CLUSTERED ([SubConOutFty], [ContractNumber])
)
