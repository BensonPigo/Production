CREATE TABLE [dbo].[Capacity] (
    [Issuedate]     DATE         NOT NULL,
    [ArtworktypeID] VARCHAR (20) CONSTRAINT [DF_Capacity_artworktypeid] DEFAULT ('') NOT NULL,
    [MDivisionID]   VARCHAR (8)  NOT NULL,
    [FtySupp]       VARCHAR (8)  CONSTRAINT [DF_Capacity_ftysupp] DEFAULT ('') NOT NULL,
    [Capacity]      NUMERIC (7)  CONSTRAINT [DF_Capacity_capacity] DEFAULT ((0)) NULL,
    [Unit]          NUMERIC (1)  CONSTRAINT [DF_Capacity_unit] DEFAULT ((0)) NULL,
    [Heads]         NUMERIC (2)  CONSTRAINT [DF_Capacity_headsno] DEFAULT ((0)) NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Capacity_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Capacity_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_Capacity] PRIMARY KEY CLUSTERED ([Issuedate] ASC, [ArtworktypeID] ASC, [MDivisionID] ASC, [FtySupp] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Issuedate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'Issuedate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'artworktypeid', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'artworktypeid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ftysupp', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'ftysupp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'capacity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'capacity';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'unit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'unit';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'headsno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Capacity', @level2type = N'COLUMN', @level2name = N'Heads';

