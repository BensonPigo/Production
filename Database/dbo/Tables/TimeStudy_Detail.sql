CREATE TABLE [dbo].[TimeStudy_Detail] (
    [ID]            BIGINT         CONSTRAINT [DF_TimeStudy_Detail_TimeStudyUKey] DEFAULT ((0)) NOT NULL,
    [Seq]           VARCHAR (4)    CONSTRAINT [DF_TimeStudy_Detail_Seq] DEFAULT ('') NOT NULL,
    [OperationID]   VARCHAR (20)   CONSTRAINT [DF_TimeStudy_Detail_OperationID] DEFAULT ('') NULL,
    [Annotation]    NVARCHAR (MAX) CONSTRAINT [DF_TimeStudy_Detail_Annotation] DEFAULT ('') NULL,
    [PcsPerHour]    NUMERIC (8, 1) CONSTRAINT [DF_TimeStudy_Detail_PcsPerHour] DEFAULT ((0)) NULL,
    [Sewer]         NUMERIC (4, 1) CONSTRAINT [DF_TimeStudy_Detail_Sewer] DEFAULT ((0)) NULL,
    [MachineTypeID] VARCHAR (20)   CONSTRAINT [DF_TimeStudy_Detail_MachineTypeID] DEFAULT ('') NULL,
    [Frequency]     NUMERIC (7, 2) CONSTRAINT [DF_TimeStudy_Detail_Frequency] DEFAULT ((0)) NULL,
    [IETMSSMV]      NUMERIC (9, 4) CONSTRAINT [DF_TimeStudy_Detail_IETMSSMV] DEFAULT ((0)) NULL,
    [Mold]          NVARCHAR (65)  CONSTRAINT [DF_TimeStudy_Detail_MoldID] DEFAULT ('') NULL,
    [SMV]           NUMERIC (12, 4) CONSTRAINT [DF_TimeStudy_Detail_SMV] DEFAULT ((0)) NULL,
    [OldKey]        VARCHAR (13)   CONSTRAINT [DF_TimeStudy_Detail_OldKey] DEFAULT ('') NULL,
    [SeamLength]    NUMERIC (12, 2) NULL,
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [MtlFactorID] VARCHAR(3) NULL, 
    [MasterPlusGroup] VARCHAR(4) NOT NULL DEFAULT (''), 
    [IsSubprocess] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_TimeStudy_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory GSD Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time Study UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'OperationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'註釋', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'Annotation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每小時產出件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'PcsPerHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'Sewer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'MachineTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Frequency', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'Frequency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMV(分)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'IETMSSMV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模具', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'Mold';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMV(秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'SMV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy_Detail', @level2type = N'COLUMN', @level2name = N'OldKey';


GO
CREATE NONCLUSTERED INDEX [IDSeq]
    ON [dbo].[TimeStudy_Detail]([ID] ASC, [Seq] ASC);

