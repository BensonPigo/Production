CREATE TABLE [dbo].[MtlCertificate_Detail]
(
	[Ukey] BIGINT NOT NULL , 
    [ID] VARCHAR(13) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_ID] DEFAULT (''), 
    [SuppID] VARCHAR(6) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_SuppID] DEFAULT (''), 
    [InvoiceNo] VARCHAR(25) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_InvoiceNo] DEFAULT (''), 
    [FormType] VARCHAR(2) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_FormType] DEFAULT (''), 
    [FormNo] VARCHAR(50) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_FormNo] DEFAULT (''), 
    [TPEReceiveDate] DATETIME NULL, 
    [TPERemark] VARCHAR(500) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_TPERemark] DEFAULT (''), 
    [Junk] BIT NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_Junk] DEFAULT (0),
    [FtyReceiveDate] DATETIME NULL, 
    [FtyReceiveName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_FtyReceiveName] DEFAULT (''), 
    [FtyRemark] VARCHAR(500) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_FtyRemark] DEFAULT (''), 
    [TPEAddName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_TPEAddName] DEFAULT (''), 
    [TPEAddDate] DATETIME NULL, 
    [TPEEditName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_TPEEditName] DEFAULT (''), 
    [TPEEditDate] DATETIME NULL, 
    [FtyEditName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_Detail_FtyEditName] DEFAULT (''), 
    [FtyEditDate] DATETIME NULL,
    CONSTRAINT [PK_MtlCertificate_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
)
