CREATE TABLE [dbo].[TimeStudyHistory_Detail] (
    [ID]            BIGINT         CONSTRAINT [DF_TimeStudyHistory_Detail_TimeStudyUKey] DEFAULT ((0)) NOT NULL,
    [Seq]           VARCHAR (4)    CONSTRAINT [DF_TimeStudyHistory_Detail_Seq] DEFAULT ('') NOT NULL,
    [OperationID]   VARCHAR (20)   CONSTRAINT [DF_TimeStudyHistory_Detail_OperationID] DEFAULT ('') NULL,
    [Annotation]    NVARCHAR (MAX) CONSTRAINT [DF_TimeStudyHistory_Detail_Annotation] DEFAULT ('') NULL,
    [PcsPerHour]    NUMERIC (8, 1) CONSTRAINT [DF_TimeStudyHistory_Detail_PcsPerHour] DEFAULT ((0)) NULL,
    [Sewer]         NUMERIC (8, 1) CONSTRAINT [DF_TimeStudyHistory_Detail_Sewer] DEFAULT ((0)) NULL,
    [MachineTypeID] VARCHAR (20)   CONSTRAINT [DF_TimeStudyHistory_Detail_MachineTypeID] DEFAULT ('') NULL,
    [Frequency]     NUMERIC (7, 2) CONSTRAINT [DF_TimeStudyHistory_Detail_Frequency] DEFAULT ((0)) NULL,
    [IETMSSMV]      NUMERIC (9, 4) CONSTRAINT [DF_TimeStudyHistory_Detail_IETMSSMV] DEFAULT ((0)) NULL,
    [Mold]          NVARCHAR (65)  CONSTRAINT [DF_TimeStudyHistory_Detail_MoldID] DEFAULT ('') NULL,
    [SMV]           NUMERIC (12, 4) CONSTRAINT [DF_TimeStudyHistory_Detail_SMV] DEFAULT ((0)) NULL,
    [OldKey]        VARCHAR (13)   CONSTRAINT [DF_TimeStudyHistory_Detail_OldKey] DEFAULT ('') NULL,
    [SeamLength]    NUMERIC (12, 2) NULL,
    [MtlFactorID] VARCHAR(3) NULL, 
    [Template] VARCHAR(100)         CONSTRAINT [DF_TimeStudyHistory_Detail_Template] DEFAULT ('')  NOT NULL, 
    [MasterPlusGroup] VARCHAR(4)    CONSTRAINT [DF_TimeStudyHistory_Detail_MasterPlusGroup] DEFAULT ('')  NOT NULL, 
    [IsSubprocess] BIT              CONSTRAINT [DF_TimeStudyHistory_Detail_IsSubprocess] DEFAULT ((0))  NOT NULL, 
    [SewingMachineAttachmentID] VARCHAR(200)    CONSTRAINT [DF_TimeStudyHistory_Detail_SewingMachineAttachmentID] DEFAULT ('') NOT NULL , 
    [PPA] VARCHAR(2)                CONSTRAINT [DF_TimeStudyHistory_Detail_PPA] DEFAULT ('') NOT NULL , 
    [IsNonSewingLine] BIT           CONSTRAINT [DF_TimeStudyHistory_Detail_IsNonSewingLine] NOT NULL DEFAULT ((0)), 
    [StdSMV] NUMERIC(12, 4) CONSTRAINT [DF_TimeStudyHistory_Detail_StdSMV] DEFAULT (0) not NULL,
    [Thread_ComboID] VARCHAR(10) CONSTRAINT [DF_TimeStudyHistory_Detail_Thread_ComboID] DEFAULT ('') not NULL,
    CONSTRAINT [PK_TimeStudyHistory_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Seq] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory GSD Detail (History)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time Study UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'OperationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'註釋', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'Annotation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每小時產出件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'PcsPerHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewer', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'Sewer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'MachineTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Frequency', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'Frequency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMV(分)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'IETMSSMV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模具', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'Mold';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMV(秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'SMV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudyHistory_Detail', @level2type = N'COLUMN', @level2name = N'OldKey';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'模板',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TimeStudyHistory_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Template'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MC Group',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TimeStudyHistory_Detail',
    @level2type = N'COLUMN',
    @level2name = N'MasterPlusGroup'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1=是，0=否',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TimeStudyHistory_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IsSubprocess'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SewingMachineAttachmen.ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TimeStudyHistory_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SewingMachineAttachmentID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'C=Centralized，S=SewingLine',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TimeStudyHistory_Detail',
    @level2type = N'COLUMN',
    @level2name = N'PPA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1=是，0=否',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TimeStudyHistory_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IsNonSewingLine'