CREATE TABLE [dbo].[SubconReason] (
    [Type]        VARCHAR (2)  NOT NULL,
    [ID]          VARCHAR (5)  NOT NULL,
    [Reason]      VARCHAR (50) NOT NULL,
    [Responsible] VARCHAR (50) NULL,
    [Junk]        BIT          CONSTRAINT [DF_SubconReason_Junk] DEFAULT ((0)) NULL,
    [AddDate]     DATETIME     NULL,
    [AddName]     VARCHAR (10) CONSTRAINT [DF_SubconReason_AddName] DEFAULT ('') NULL,
    [EditDate]    DATETIME     NULL,
    [EditName]    VARCHAR (10) CONSTRAINT [DF_SubconReason_EditName] DEFAULT ('') NULL,
    [Status]      VARCHAR (15) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SubconReason] PRIMARY KEY CLUSTERED ([Type] ASC, [ID] ASC)
);

