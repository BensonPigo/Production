CREATE TABLE [dbo].[P_LoadingvsCapacity]
(
	[MDivisionID]        VARCHAR(8)         CONSTRAINT [DF_P_LoadingvsCapacity_MDivisionID]     NOT NULL DEFAULT (('')), 
    [KpiCode]            VARCHAR(8)         CONSTRAINT [DF_P_LoadingvsCapacity_KpiCode]         NOT NULL DEFAULT (('')), 
    [Key]                VARCHAR(6)         CONSTRAINT [DF_P_LoadingvsCapacity_Key]             NOT NULL DEFAULT (('')), 
    [Halfkey]            VARCHAR(8)         CONSTRAINT [DF_P_LoadingvsCapacity_Halfkey]         NOT NULL DEFAULT (('')), 
    [ArtworkTypeID]      VARCHAR(20)        CONSTRAINT [DF_P_LoadingvsCapacity_ArtworkTypeID]   NOT NULL DEFAULT (('')), 
    [Capacity(CPU)]      NUMERIC(38, 6)     CONSTRAINT [DF_P_LoadingvsCapacity_Capacity(CPU)]   NOT NULL DEFAULT ((0)), 
    [Loading (CPU)]      NUMERIC(38, 6)     CONSTRAINT [DF_P_LoadingvsCapacity_Loading (CPU)]   NOT NULL DEFAULT ((0)), 
    [TransferBIDate]     DATETIME NULL, 
    [Ukey]               BIGINT NOT NULL    IDENTITY(1,1), 
    CONSTRAINT [PK_P_LoadingvsCapacity] PRIMARY KEY ([MDivisionID], [Key], [KpiCode], [Halfkey], [ArtworkTypeID])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'組織代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingvsCapacity',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠kpi統計群組',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingvsCapacity',
    @level2type = N'COLUMN',
    @level2name = N'KpiCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'月代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingvsCapacity',
    @level2type = N'COLUMN',
    @level2name = N'Key'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'半月份代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingvsCapacity',
    @level2type = N'COLUMN',
    @level2name = N'Halfkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'作工代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingvsCapacity',
    @level2type = N'COLUMN',
    @level2name = N'ArtworkTypeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Capacity(CPU)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingvsCapacity',
    @level2type = N'COLUMN',
    @level2name = N'Capacity(CPU)'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Loading (CPU)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_LoadingvsCapacity',
    @level2type = N'COLUMN',
    @level2name = N'Loading (CPU)'