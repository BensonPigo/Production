CREATE TABLE [dbo].[OverrunGMT_Detail] (
    [ID]              VARCHAR (13) CONSTRAINT [DF_OverrunGMT_Detail_ID] DEFAULT ('') NOT NULL,
    [Article]         VARCHAR (8)  CONSTRAINT [DF_OverrunGMT_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]        VARCHAR (8)  CONSTRAINT [DF_OverrunGMT_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]             INT          CONSTRAINT [DF_OverrunGMT_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [PackingReasonID] VARCHAR (5)  CONSTRAINT [DF_OverrunGMT_Detail_PackingReasonID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_OverrunGMT_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Article] ASC, [SizeCode] ASC, [PackingReasonID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Overrun Garment Record Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OverrunGMT_Detail', @level2type = N'COLUMN', @level2name = N'PackingReasonID';

