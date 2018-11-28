CREATE TABLE [dbo].[BIRInvoice] (
    [ID]        INT          IDENTITY (1, 1) NOT NULL,
    [InvSerial] VARCHAR (10) NOT NULL,
    [BrandID]   VARCHAR (8)  NOT NULL,
    [AddName]   VARCHAR (10) NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) NULL,
    [EditDate]  DATETIME     NULL
);

