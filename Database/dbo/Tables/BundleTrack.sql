CREATE TABLE [dbo].[BundleTrack] (
    [Id]           VARCHAR (13) CONSTRAINT [DF_BundleTrack_Id] DEFAULT ('') NOT NULL,
    [IssueDate]    DATE         NOT NULL,
    [StartProcess] VARCHAR (10) CONSTRAINT [DF_BundleTrack_StartProcess] DEFAULT ('') NOT NULL,
    [EndProcess]   VARCHAR (10) CONSTRAINT [DF_BundleTrack_EndProcess] DEFAULT ('') NULL,
    [StartSite]    VARCHAR (8)  CONSTRAINT [DF_BundleTrack_StartSite] DEFAULT ('') NOT NULL,
    [EndSite]      VARCHAR (8)  CONSTRAINT [DF_BundleTrack_EndSite] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10) CONSTRAINT [DF_BundleTrack_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME     NULL,
    [EditName]     VARCHAR (10) CONSTRAINT [DF_BundleTrack_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME     NULL,
    CONSTRAINT [PK_BundleTrack] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Real time bundle track', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始作工', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'StartProcess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'接手作工', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'EndProcess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'StartSite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'接手單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'EndSite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack', @level2type = N'COLUMN', @level2name = N'EditDate';

