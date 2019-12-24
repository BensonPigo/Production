CREATE TABLE [dbo].[ClogReason] (
    [Type]        VARCHAR (2)    CONSTRAINT [DF_ClogReason_Type] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (5)    CONSTRAINT [DF_ClogReason_ID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (60)  CONSTRAINT [DF_ClogReason_Description] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (100) CONSTRAINT [DF_ClogReason_Remark] DEFAULT ('') NOT NULL,
    [Junk]        BIT            CONSTRAINT [DF_ClogReason_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_ClogReason_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       CONSTRAINT [DF_ClogReason_AddDate] DEFAULT ('') NOT NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_ClogReason_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_ClogReason] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);

