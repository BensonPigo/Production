CREATE TABLE [dbo].[ADIDASComplainDefect_Detail] (
    [ID]      VARCHAR (2)    CONSTRAINT [DF_ADIDASComplainDefect_Detail_ID] DEFAULT ('') NOT NULL,
    [SubID]   VARCHAR (1)    CONSTRAINT [DF_ADIDASComplainDefect_Detail_SubID] DEFAULT ('') NOT NULL,
    [SubName] NVARCHAR (250) CONSTRAINT [DF_ADIDASComplainDefect_Detail_SubName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ADIDASComplainDefect_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [SubID] ASC)
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
