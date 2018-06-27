CREATE TABLE [dbo].[IETMS_Summary_detail] (
    [IETMSUkey]     BIGINT          NOT NULL,
    [StyleUkey]     BIGINT          NOT NULL,
    [ArtworkTypeID] VARCHAR (20)    NOT NULL,
    [ProSMV]        NUMERIC (14, 6) NOT NULL,
    [ProTMS]        NUMERIC (7, 2)  NOT NULL,
    [ProPrice]      NUMERIC (18, 6) NOT NULL,
    [CIPF]          VARCHAR (1)     NOT NULL,
    CONSTRAINT [PK_IETMS_Summary_detail] PRIMARY KEY CLUSTERED ([IETMSUkey] ASC, [ArtworkTypeID] ASC, [CIPF] ASC)
);

