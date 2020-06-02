CREATE TABLE [dbo].[P_TransRegion] (
    [Region]         VARCHAR (50)  NOT NULL,
    [LinkServerName] VARCHAR (100) CONSTRAINT [DF_P_TransRegion_LinkServerName] DEFAULT ('') NULL,
    [ConnectionName] VARCHAR (50)  CONSTRAINT [DF_P_TransRegion_ConnectionName] DEFAULT ('') NULL,
    CONSTRAINT [PK_P_TransRegion] PRIMARY KEY CLUSTERED ([Region] ASC)
);

