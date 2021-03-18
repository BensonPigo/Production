CREATE TABLE [dbo].[SpreadingSchedule] (
    [Ukey]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [FactoryID]  VARCHAR (8)  CONSTRAINT [DF_SpreadingSchedule_FactoryID] DEFAULT ('') NOT NULL,
    [EstCutDate] DATE         NOT NULL,
    [CutCellID]  VARCHAR (2)  CONSTRAINT [DF_SpreadingSchedule_CutCellID] DEFAULT ('') NOT NULL,
    [AddDate]    DATETIME     NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_SpreadingSchedule_AddName] DEFAULT ('') NOT NULL,
    [EditDate]   DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_SpreadingSchedule_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SpreadingSchedule] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UniqueKey]
    ON [dbo].[SpreadingSchedule]([FactoryID] ASC, [EstCutDate] ASC, [CutCellID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪Cell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'CutCellID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉布日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'EstCutDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule', @level2type = N'COLUMN', @level2name = N'Ukey';

