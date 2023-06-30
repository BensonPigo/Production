CREATE TABLE [dbo].[IEReasonTypeGroup] (
    [Type]                  VARCHAR (50) NOT NULL,
    [TypeGroup]             VARCHAR (50) NOT NULL,
    [IEReasonApplyFunction] INT          CONSTRAINT [DF_IEReasonTypeGroup_IEReasonApplyFunction] DEFAULT ((0)) NOT NULL,
    [Junk]                  BIT          CONSTRAINT [DF_IEReasonTypeGroup_Junk] DEFAULT ((0)) NOT NULL,
    [AddDate]               DATETIME     NULL,
    [AddName]               VARCHAR (10) CONSTRAINT [DF_IEReasonTypeGroup_AddName] DEFAULT ('') NOT NULL,
    [EditDate]              DATETIME     NULL,
    [EditName]              VARCHAR (10) CONSTRAINT [DF_IEReasonTypeGroup_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_IEReasonTypeGroup] PRIMARY KEY CLUSTERED ([Type] ASC, [TypeGroup] ASC)
);


GO


GO