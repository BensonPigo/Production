CREATE TABLE [dbo].[LocalSupp_Bank_Detail] (
    [ID]           VARCHAR (8)    DEFAULT ('') NOT NULL,
    [Pkey]         BIGINT         NOT NULL,
    [Ukey]         BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AccountNo]    VARCHAR (30)   DEFAULT ('') NOT NULL,
    [AccountName]  NVARCHAR (100) DEFAULT ('') NULL,
    [BankName]     NVARCHAR (70)  DEFAULT ('') NOT NULL,
    [BranchCode]   VARCHAR (30)   DEFAULT ('') NOT NULL,
    [BranchName]   NVARCHAR (60)  DEFAULT ('') NOT NULL,
    [CountryID]    VARCHAR (2)    DEFAULT ('') NOT NULL,
    [City]         NVARCHAR (20)  DEFAULT ('') NULL,
    [SWIFTCode]    VARCHAR (11)   DEFAULT ('') NULL,
    [MidSWIFTCode] VARCHAR (11)   DEFAULT ('') NULL,
    [MidBankName]  NVARCHAR (70)  DEFAULT ('') NULL,
    [Remark]       NVARCHAR (MAX) DEFAULT ('') NULL,
    [IsDefault]    BIT            DEFAULT ((0)) NULL,
    [VNBankBranch] VARCHAR (50)   DEFAULT ('') NULL,
    CONSTRAINT [PK_LocalSupp_Bank_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Pkey] ASC, [Ukey] ASC)
);


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


