CREATE TABLE [dbo].[I_CartonMaterialStatus] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CtnRefno]       VARCHAR (21)   NOT NULL,
    [CtnLength]      NUMERIC (8, 4) NOT NULL,
    [CtnWidth]       NUMERIC (8, 4) NOT NULL,
    [CtnHeight]      NUMERIC (8, 4) NOT NULL,
    [Auto]           BIT            NULL,
    [IsReady]        BIT            CONSTRAINT [DF_I_CartonMaterialStatus_IsReady] DEFAULT ((0)) NULL,
    [IsNeed]         BIT            CONSTRAINT [DF_I_CartonMaterialStatus_IsNeed] DEFAULT ((0)) NULL,
    [SunriseUpdated] BIT            CONSTRAINT [DF_I_CartonMaterialStatus_SunriseUpdated] DEFAULT ((0)) NULL,
    [GenSongUpdated] BIT            CONSTRAINT [DF_I_CartonMaterialStatus_GenSongUpdated] DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

