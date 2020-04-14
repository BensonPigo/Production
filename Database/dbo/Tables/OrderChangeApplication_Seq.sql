CREATE TABLE [dbo].[OrderChangeApplication_Seq](
	[Ukey] [bigint] NOT NULL,
	[ID] [varchar](13) NOT NULL,
	[Seq] [varchar](2) NOT NULL,
	[NewSeq] [varchar](2) NULL,
	[ShipmodeID] [varchar](10) NOT NULL,
	[BuyerDelivery] [date] NOT NULL,
	[FtyKPI] [date] NULL,
	[ReasonID] [varchar](5) NULL,
	[ReasonRemark] [nvarchar](150) NULL,
	[ShipModeRemark] [nvarchar](150) NULL,
 CONSTRAINT [PK_OrderChangeApplication_Seq] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderChangeApplication_Seq] ADD  CONSTRAINT [DF_OrderChangeApplication_Seq_ReasonID]  DEFAULT ('') FOR [ReasonID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改客戶交期原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderChangeApplication_Seq', @level2type=N'COLUMN',@level2name=N'ReasonID'
GO