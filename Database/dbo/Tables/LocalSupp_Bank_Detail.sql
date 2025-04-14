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
    [SWIFTCode]    VARCHAR (20)   DEFAULT ('') NULL,
    [MidSWIFTCode] VARCHAR (20)   DEFAULT ('') NULL,
    [MidBankName]  NVARCHAR (70)  DEFAULT ('') NULL,
    [Remark]       NVARCHAR (MAX) DEFAULT ('') NULL,
    [IsDefault]    BIT            DEFAULT ((0)) NULL,
    [VNBankBranch] VARCHAR (50)   DEFAULT ('') NULL,
    [BankNameTPB] varchar(100) CONSTRAINT [DF_LocalSupp_Bank_Detail_BankNameTPB]  DEFAULT ('') NOT NULL,
    [BankCodeTPB] varchar(20)  CONSTRAINT [DF_LocalSupp_Bank_Detail_BankCodeTPB] DEFAULT ('') NOT NULL,
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


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用於供應商付款BankName', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank_Detail', @level2type = N'COLUMN', @level2name = N'BankNameTPB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用於供應商付款BankCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalSupp_Bank_Detail', @level2type = N'COLUMN', @level2name = N'BankCodeTPB';
