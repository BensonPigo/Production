CREATE TABLE [dbo].[IEReasonType] (
    [Type]     VARCHAR (50) NOT NULL,
    [Junk]     BIT          CONSTRAINT [DF_IEReasonType_Junk] DEFAULT ((0)) NOT NULL,
    [AddDate]  DATETIME     NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_IEReasonType_AddName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_IEReasonType_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_IEReasonType] PRIMARY KEY CLUSTERED ([Type] ASC)
);


GO


GO