CREATE TABLE [dbo].[BIRInvoice] (
    [ID]          INT          IDENTITY (1, 1) NOT NULL,
    [InvSerial]   VARCHAR (10) NOT NULL,
    [BrandID]     VARCHAR (8)  NOT NULL,
    [AddName]     VARCHAR (10) NULL,
    [AddDate]     DATETIME     NULL,
    [EditName]    VARCHAR (10) NULL,
    [EditDate]    DATETIME     NULL,
    [ExVoucherID] VARCHAR (16) DEFAULT ('') NULL,
    [Status]      VARCHAR (15) CONSTRAINT [DF_BIRInvoice_Status] DEFAULT ('') NULL,
    [Approve]     VARCHAR (10) CONSTRAINT [DF_BIRInvoice_Approve] DEFAULT ('') NULL,
    [ApproveDate] DATETIME     NULL,
    CONSTRAINT [PK_BIRInvoice] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BIRInvoice', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�~�b�ǲ�ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BIRInvoice', @level2type = N'COLUMN', @level2name = N'ExVoucherID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BIRInvoice', @level2type = N'COLUMN', @level2name = N'ApproveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Approve人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BIRInvoice', @level2type = N'COLUMN', @level2name = N'Approve';

