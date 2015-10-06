CREATE TABLE [dbo].[Issue_Breakdown] (
    [Id]       VARCHAR (13) CONSTRAINT [DF_Issue_Breakdown_Id] DEFAULT ('') NOT NULL,
    [OrderID]  VARCHAR (13) CONSTRAINT [DF_Issue_Breakdown_OrderID] DEFAULT ('') NOT NULL,
    [Article]  VARCHAR (8)  CONSTRAINT [DF_Issue_Breakdown_Article] DEFAULT ('') NOT NULL,
    [SizeCode] VARCHAR (8)  CONSTRAINT [DF_Issue_Breakdown_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]      NUMERIC (6)  CONSTRAINT [DF_Issue_Breakdown_Qty] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Issue_Breakdown] PRIMARY KEY CLUSTERED ([Id] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Breakdown';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Breakdown', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Breakdown', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Breakdown', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Breakdown', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Issue_Breakdown', @level2type = N'COLUMN', @level2name = N'Qty';

