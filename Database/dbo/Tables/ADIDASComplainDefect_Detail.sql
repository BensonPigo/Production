CREATE TABLE [dbo].[ADIDASComplainDefect_Detail] (
    [ID]      VARCHAR (2)    CONSTRAINT [DF_ADIDASComplainDefect_Detail_ID] DEFAULT ('') NOT NULL,
    [SubID]   VARCHAR (1)    CONSTRAINT [DF_ADIDASComplainDefect_Detail_SubID] DEFAULT ('') NOT NULL,
    [SubName] NVARCHAR (250) CONSTRAINT [DF_ADIDASComplainDefect_Detail_SubName] DEFAULT ('') NOT NULL,
    [MtlTypeID] VARCHAR(20) CONSTRAINT [DF_ADIDASComplainDefect_Detail_MtlTypeID] DEFAULT ('') NOT NULL, 
    [FabricType] VARCHAR(1) CONSTRAINT [DF_ADIDASComplainDefect_Detail_FabricType] DEFAULT ('') NOT NULL, 
    [Responsibility] VARCHAR(2) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_ADIDASComplainDefect_Detail] PRIMARY KEY ([ID], [SubID], [FabricType]) 
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Adidas Complain Defect 代碼主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Main ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sub ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect_Detail', @level2type = N'COLUMN', @level2name = N'SubID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ADIDASComplainDefect_Detail', @level2type = N'COLUMN', @level2name = N'SubName';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'責任歸屬',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ADIDASComplainDefect_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Responsibility'