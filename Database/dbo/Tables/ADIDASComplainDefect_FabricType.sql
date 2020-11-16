CREATE TABLE [dbo].[ADIDASComplainDefect_FabricType] (
    [ID]             VARCHAR (2)    CONSTRAINT [DF_ADIDASComplainDefect_FabricType_ID] DEFAULT ('') NOT NULL,
    [SubID]          VARCHAR (1)    CONSTRAINT [DF_ADIDASComplainDefect_FabricType_SubID] DEFAULT ('') NOT NULL,
    [FabricType]     VARCHAR (1)    CONSTRAINT [DF_ADIDASComplainDefect_FabricType_FabricType] DEFAULT ('') NOT NULL,
    [Responsibility] VARCHAR (2)    CONSTRAINT [DF_ADIDASComplainDefect_FabricType_Responsibility] DEFAULT ('') NOT NULL,
    [MtlTypeID]      VARCHAR (5000) CONSTRAINT [DF_ADIDASComplainDefect_FabricType_MtlTypeID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ADIDASComplainDefect_FabricType] PRIMARY KEY CLUSTERED ([ID] ASC, [SubID] ASC, [FabricType] ASC)
);

