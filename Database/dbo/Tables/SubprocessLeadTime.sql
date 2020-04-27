CREATE TABLE [dbo].[SubprocessLeadTime] (
    [ID]       SMALLINT     IDENTITY (1, 1) NOT NULL,
    [LeadTime] TINYINT      CONSTRAINT [DF_SubprocessLeadTime_LeadTime] DEFAULT ((0)) NOT NULL,
    [AddName]  VARCHAR (10) NOT NULL,
    [AddDate]  DATETIME     NOT NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_SubprocessLeadTime_EditName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME     NULL,
    [MDivisionID] VARCHAR(8) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_SubprocessLeadTime] PRIMARY KEY CLUSTERED ([ID] ASC)
);

