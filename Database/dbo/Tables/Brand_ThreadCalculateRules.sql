CREATE TABLE [dbo].[Brand_ThreadCalculateRules] (
    [ID]                 VARCHAR (8)  NOT NULL,
    [FabricType]         VARCHAR (5)  NOT NULL,
    [UseRatioRule]       VARCHAR (1)  CONSTRAINT [DF_Brand_ThreadCalculateRules_UseRatioRule] DEFAULT ('') NOT NULL,
    [UseRatioRule_Thick] VARCHAR (1)  CONSTRAINT [DF_Brand_ThreadCalculateRules_UseRatioRule_Thick] DEFAULT ('') NOT NULL,
    [ProgramID]          VARCHAR (12) CONSTRAINT [DF_Brand_ThreadCalculateRules_ProgramID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK__Brand_Th__E4FE7DA41330067D] PRIMARY KEY CLUSTERED ([ID] ASC, [FabricType] ASC, [ProgramID] ASC)
);


