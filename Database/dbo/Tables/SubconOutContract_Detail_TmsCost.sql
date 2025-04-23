CREATE TABLE [dbo].[SubconOutContract_Detail_TmsCost](
SubConOutFty [varchar](8) NOT NULL CONSTRAINT [DF_SubconOutContract_Detail_TmsCost_SubConOutFty] DEFAULT (''),
ContractNumber [varchar](50) NOT NULL CONSTRAINT [DF_SubconOutContract_Detail_TmsCost_ContractNumber] DEFAULT (''),
OrderId [varchar](13) NOT NULL CONSTRAINT [DF_SubconOutContract_Detail_TmsCost_OrderId] DEFAULT (''),
ArtworkTypeID [varchar](20) NOT NULL CONSTRAINT [DF_SubconOutContract_Detail_TmsCost_ArtworkTypeID] DEFAULT (''),
TMS [decimal](5,0) NOT NULL CONSTRAINT [DF_SubconOutContract_Detail_TmsCost_TMS] DEFAULT (0),
Price [decimal](16,4) NOT NULL CONSTRAINT [DF_SubconOutContract_Detail_TmsCost_Price] DEFAULT (0)
 CONSTRAINT [PK_SubconOutContract_Detail_TmsCost] PRIMARY KEY CLUSTERED 
(
	[SubConOutFty],[ContractNumber],[OrderId],[ArtworkTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubconOutContract_Detail_TmsCost', @level2type=N'COLUMN',@level2name=N'OrderId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加工應付單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubconOutContract_Detail_TmsCost', @level2type=N'COLUMN',@level2name=N'ArtworkTypeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'合約Confirm當下Tms' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubconOutContract_Detail_TmsCost', @level2type=N'COLUMN',@level2name=N'TMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'合約Confirm當下的Price' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SubconOutContract_Detail_TmsCost', @level2type=N'COLUMN',@level2name=N'Price'
GO

