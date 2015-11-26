CREATE TABLE [dbo].[ForwarderWhse_Detail] (
    [ID]     BIGINT        NOT NULL,
    [WhseNo] NVARCHAR (50) NOT NULL,
    [UKey]   BIGINT        IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ForwarderWhse_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [WhseNo] ASC)
);

