CREATE TABLE [dbo].[InvtransReason] (
    [ID]           VARCHAR (5)    DEFAULT ('') NOT NULL,
    [ReasonEN]     NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [ReasonCH]     NVARCHAR (MAX) CONSTRAINT [DF_InvtransReason_ReasonCH] DEFAULT ('') NOT NULL,
    [IsDefault]    BIT            CONSTRAINT [DF_InvtransReason_IsDefault] DEFAULT ((0)) NOT NULL,
    [Junk]         BIT            CONSTRAINT [DF_InvtransReason_Junk] DEFAULT ((0)) NOT NULL,
    [AdjustFields] NVARCHAR (200) CONSTRAINT [DF_InvtransReason_AdjustFields] DEFAULT ('') NOT NULL,
    [AdjustDesc]   NVARCHAR (200) CONSTRAINT [DF_InvtransReason_AdjustDesc] DEFAULT ('') NOT NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_InvtransReason_AddName] DEFAULT ('') NOT NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_InvtransReason_EditName] DEFAULT ('') NOT NULL,
    [EditDate]     DATETIME       NULL,
    CONSTRAINT [PK_InvtransReason] PRIMARY KEY CLUSTERED ([ID] ASC)
);



