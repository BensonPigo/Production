CREATE TABLE [dbo].[SubconReason] (
    [Type]        VARCHAR (2)  NOT NULL,
    [ID]          VARCHAR (5)  NOT NULL,
    [Reason]      VARCHAR (50) NOT NULL,
    [Responsible] VARCHAR (50) CONSTRAINT [DF_SubconReason_Responsible] DEFAULT ('') NOT NULL,
    [Junk]        BIT          CONSTRAINT [DF_SubconReason_Junk] DEFAULT ((0)) NOT NULL,
    [AddDate]     DATETIME     NULL,
    [AddName]     VARCHAR (10) CONSTRAINT [DF_SubconReason_AddName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME     NULL,
    [EditName]    VARCHAR (10) CONSTRAINT [DF_SubconReason_EditName] DEFAULT ('') NOT NULL,
    [Status]      VARCHAR (15) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SubconReason] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);


