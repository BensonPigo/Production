CREATE TABLE [dbo].[RefnoRelaxtime] (
    [Refno]              VARCHAR (36)   NOT NULL,
    [FabricRelaxationID] VARCHAR (30)   NULL,
    [Relaxtime]          NUMERIC (5, 2) NULL,
    [CmdTime]            DATETIME       NULL,
    CONSTRAINT [PK_RefnoRelaxtime] PRIMARY KEY CLUSTERED ([Refno] ASC)
);

