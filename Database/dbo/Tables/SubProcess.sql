CREATE TABLE [dbo].[SubProcess] (
    [Id]                      VARCHAR (10) CONSTRAINT [DF_SubProcess_Id] DEFAULT ('') NOT NULL,
    [ArtworkTypeId]           VARCHAR (20) CONSTRAINT [DF_SubProcess_ArtworkTypeId] DEFAULT ('') NOT NULL,
    [IsSelection]             BIT          CONSTRAINT [DF_SubProcess_IsSelection] DEFAULT ((0)) NULL,
    [IsRFIDProcess]           BIT          CONSTRAINT [DF_SubProcess_IsProcess] DEFAULT ((0)) NULL,
    [IsRFIDDefault]           BIT          CONSTRAINT [DF_SubProcess_IsDisplay] DEFAULT ((0)) NULL,
    [ShowSeq]                 VARCHAR (2)  CONSTRAINT [DF_SubProcess_ShowSeq] DEFAULT ('') NULL,
    [Junk]                    BIT          CONSTRAINT [DF_SubProcess_Junk] DEFAULT ((0)) NULL,
    [AddName]                 VARCHAR (10) CONSTRAINT [DF_SubProcess_AddName] DEFAULT ('') NULL,
    [AddDate]                 DATETIME     NULL,
    [EditName]                VARCHAR (10) CONSTRAINT [DF_SubProcess_EditName] DEFAULT ('') NULL,
    [EditDate]                DATETIME     NULL,
    [BCSDate]                 NUMERIC (2)  NULL,
    [InOutRule]               TINYINT      DEFAULT ((0)) NOT NULL,
    [FullName]                VARCHAR (15) NULL,
    [IsLackingAndReplacement] BIT          DEFAULT ((0)) NOT NULL,
    [Seq]                     TINYINT      DEFAULT ((0)) NOT NULL,
    [IsBoundedProcess]        BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SubProcess] PRIMARY KEY CLUSTERED ([Id] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Subprocess Basic', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'ArtworkTypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'IsSelection';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = 'IsRFIDProcess';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = 'IsRFIDDefault';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'ShowSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = '0-NotSetting
1-OnlyIn
2-OnlyOut
3-FromInToOut
4-FromOutToIn', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SubProcess', @level2type = N'COLUMN', @level2name = N'InOutRule';



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Subprocess Full Name',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProcess',
    @level2type = N'COLUMN',
    @level2name = N'FullName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Lacking and Replacement selected',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProcess',
    @level2type = N'COLUMN',
    @level2name = N'IsLackingAndReplacement'