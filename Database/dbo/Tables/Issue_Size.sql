CREATE TABLE [dbo].[Issue_Size] (
    [Id]       VARCHAR (13)    CONSTRAINT [DF_Issue_Size_Id] DEFAULT ('') NOT NULL,
    [Poid]     VARCHAR (13)    CONSTRAINT [DF_Issue_Size_Poid] DEFAULT ('') NOT NULL,
    [Seq1]     VARCHAR (3)     CONSTRAINT [DF_Issue_Size_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]     VARCHAR (2)     CONSTRAINT [DF_Issue_Size_Seq2] DEFAULT ('') NOT NULL,
    [SizeCode] VARCHAR (8)     CONSTRAINT [DF_Issue_Size_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]      NUMERIC (10, 2) CONSTRAINT [DF_Issue_Size_Qty] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Issue_Size] PRIMARY KEY CLUSTERED ([Id] ASC, [Poid] ASC, [Seq1] ASC, [Seq2] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Issue Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'Poid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Size', @level2type = N'COLUMN', @level2name = N'Qty';

