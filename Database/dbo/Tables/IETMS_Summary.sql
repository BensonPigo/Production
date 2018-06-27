CREATE TABLE [dbo].[IETMS_Summary] (
    [IETMSUkey]     BIGINT          NOT NULL,
    [StyleUkey]     BIGINT          NOT NULL,
    [MachineTypeID] VARCHAR (10)    NOT NULL,
    [ArtworkTypeID] VARCHAR (20)    NOT NULL,
    [Location]      VARCHAR (1)     NOT NULL,
    [ProSMV]        NUMERIC (14, 6) NOT NULL,
    [ProTMS]        NUMERIC (7, 2)  NOT NULL,
    [ProPrice]      NUMERIC (18, 6) NOT NULL,
    CONSTRAINT [PK_IETMS_Summary] PRIMARY KEY CLUSTERED ([IETMSUkey] ASC, [MachineTypeID] ASC, [ArtworkTypeID] ASC, [Location] ASC)
);

