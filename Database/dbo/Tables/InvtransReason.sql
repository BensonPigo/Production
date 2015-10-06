CREATE TABLE [dbo].[InvtransReason] (
    [ID]           CHAR (5)       NOT NULL,
    [ReasonEN]     NVARCHAR (MAX) NOT NULL,
    [ReasonCH]     NVARCHAR (MAX) NULL,
    [IsDefault]    BIT            NULL,
    [Junk]         BIT            NULL,
    [AdjustFields] NVARCHAR (200) NULL,
    [AdjustDesc]   NVARCHAR (200) NULL,
    [AddName]      CHAR (10)      NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     CHAR (10)      NULL,
    [EditDate]     DATETIME       NULL
);

