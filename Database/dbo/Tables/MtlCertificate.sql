CREATE TABLE [dbo].[MtlCertificate] (
    [ID]          VARCHAR (13)  NOT NULL,
    [Consignee]   VARCHAR (8)   CONSTRAINT [DF_MtlCertificate_Consignee] DEFAULT ('') NOT NULL,
    [CartonNo]    VARCHAR (500) CONSTRAINT [DF_MtlCertificate_CartonNo] DEFAULT ('') NOT NULL,
    [ExportID]    VARCHAR (13)  CONSTRAINT [DF_MtlCertificate_ExportID] DEFAULT ('') NOT NULL,
    [Handle]      VARCHAR (10)  CONSTRAINT [DF_MtlCertificate_Handle] DEFAULT ('') NOT NULL,
    [Mailed]      BIT           CONSTRAINT [DF_MtlCertificate_Mailed] DEFAULT ((0)) NOT NULL,
    [Junk]        BIT           CONSTRAINT [DF_MtlCertificate_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_MtlCertificate_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_MtlCertificate_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME      NULL,
    [TPEEditName] VARCHAR (10)  CONSTRAINT [DF_MtlCertificate_TPEEditName] DEFAULT ('') NOT NULL,
    [TPEEditDate] DATETIME      NULL,
    CONSTRAINT [PK_MtlCertificate] PRIMARY KEY CLUSTERED ([ID] ASC)
);


