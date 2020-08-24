CREATE TABLE [dbo].[Issue_Size] (
    [Id]               VARCHAR (13)    CONSTRAINT [DF_Issue_Size_Id] DEFAULT ('') NOT NULL,
    [Issue_DetailUkey] BIGINT          NOT NULL,
    [SizeCode]         VARCHAR (8)     CONSTRAINT [DF_Issue_Size_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]              NUMERIC (11, 2) CONSTRAINT [DF_Issue_Size_Qty] DEFAULT ((0)) NULL,
    [AutoPickQty] NUMERIC(11, 2) NULL, 
    CONSTRAINT [PK_Issue_Size_1] PRIMARY KEY CLUSTERED ([Id] ASC, [Issue_DetailUkey] ASC, [SizeCode] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Issue Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'Qty';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Issue_Size]([Issue_DetailUkey] ASC);

