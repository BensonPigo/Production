CREATE TABLE [dbo].[MarkerFileNameSwitchRecord] (
    [CutRef]           VARCHAR (10) NOT NULL,
    [EstCutDate]       DATE         NULL,
    [CuttingID]        VARCHAR (13) NULL,
    [OriFileName]      VARCHAR (25) NULL,
    [OriFileLastMDate] DATETIME     NULL,
    [TransInDate]      DATETIME     NULL,
    [UpdateDate]       DATETIME     NULL,
    CONSTRAINT [PK_MarkerFileNameSwitchRecord] PRIMARY KEY CLUSTERED ([CutRef] ASC)
);

