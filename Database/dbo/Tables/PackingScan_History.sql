CREATE TABLE [dbo].[PackingScan_History](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL DEFAULT (''),
	[PackingListID] [varchar](13) NOT NULL DEFAULT (''),
	[OrderID] [varchar](13) NOT NULL DEFAULT (''),
	[CTNStartNo] [varchar](6) NOT NULL DEFAULT (''),
	[SCICtnNo] [varchar](16) NULL DEFAULT (''),
	[DeleteFrom] [varchar](12) NOT NULL DEFAULT (''),
	[ScanQty] [smallint] NOT NULL DEFAULT ((0)),
	[ScanEditDate] [datetime] NULL,
	[ScanName] [varchar](10) NOT NULL DEFAULT (''),
	[AddName] [varchar](10) NULL DEFAULT (''),
	[AddDate] [datetime] NULL,
 [LackingQty] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_PackingScan_History] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PackingScan_History', @level2type=N'COLUMN',@level2name=N'CTNStartNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PackingScan_History', @level2type=N'COLUMN',@level2name=N'SCICtnNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'從哪個程式清空，Packing P18/Clog P03' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PackingScan_History', @level2type=N'COLUMN',@level2name=N'DeleteFrom'
GO