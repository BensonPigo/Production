


CREATE TABLE [dbo].[ReplacementLocalItem_Detail](
	[ID] [varchar](13) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_Detail_ID]  DEFAULT (''),
	[Refno] [varchar](21) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_Detail_Refno]  DEFAULT (''),
	[RequestQty] [numeric](10, 2) NULL CONSTRAINT [DF_ReplacementLocalItem_Detail_RequestQty]  DEFAULT ((0)),
	[ReplacementLocalItemReasonID] [varchar](5) NOT NULL CONSTRAINT [DF_ReplacementLocalItem_Detail_PPICReasonID]  DEFAULT (''),
	[Remark] [nvarchar](60) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_ReplacementLocalItem_Detail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[Refno] ASC,
	[ReplacementLocalItemReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'LocalItem Refno' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem_Detail', @level2type=N'COLUMN',@level2name=N'Refno'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem_Detail', @level2type=N'COLUMN',@level2name=N'RequestQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem_Detail', @level2type=N'COLUMN',@level2name=N'ReplacementLocalItemReasonID'
GO


EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lacking & Replacement for LocalItem' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ReplacementLocalItem_Detail'
GO
