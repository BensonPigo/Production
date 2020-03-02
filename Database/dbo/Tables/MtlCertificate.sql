CREATE TABLE [dbo].[MtlCertificate]
(
	[ID] VARCHAR(13) NOT NULL , 
    [Consignee] VARCHAR(8) NOT NULL  CONSTRAINT [DF_MtlCertificate_Consignee] DEFAULT (''), 
    [CartonNo] VARCHAR(500) NOT NULL  CONSTRAINT [DF_MtlCertificate_CartonNo] DEFAULT (''), 
    [ExportID] VARCHAR(13) NOT NULL  CONSTRAINT [DF_MtlCertificate_ExportID] DEFAULT (''), 
    [Handle] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_Handle] DEFAULT (''), 
    [Mailed] BIT NULL  CONSTRAINT [DF_MtlCertificate_Mailed] DEFAULT (0),
    [Junk] BIT NULL  CONSTRAINT [DF_MtlCertificate_Junk] DEFAULT (0),
    [AddName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_AddName] DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_EditName] DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [TPEEditName] VARCHAR(10) NOT NULL  CONSTRAINT [DF_MtlCertificate_TPEEditName] DEFAULT (''), 
    [TPEEditDate] DATETIME NULL,
    CONSTRAINT [PK_MtlCertificate] PRIMARY KEY CLUSTERED ([ID] ASC)
)
