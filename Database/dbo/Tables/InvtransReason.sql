CREATE TABLE [dbo].[InvtransReason] (
    [ID]           VARCHAR(5)       NOT NULL DEFAULT (''),
    [ReasonEN]     NVARCHAR (MAX) NOT NULL DEFAULT (''),
    [ReasonCH]     NVARCHAR (MAX) NULL DEFAULT (''),
    [IsDefault]    BIT            NULL DEFAULT ((0)),
    [Junk]         BIT            NULL DEFAULT ((0)),
    [AdjustFields] NVARCHAR (200) NULL DEFAULT (''),
    [AdjustDesc]   NVARCHAR (200) NULL DEFAULT (''),
    [AddName]      VARCHAR(10)      NULL DEFAULT (''),
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR(10)      NULL DEFAULT (''),
    [EditDate]     DATETIME       NULL, 
    CONSTRAINT [PK_InvtransReason] PRIMARY KEY ([ID])
);

