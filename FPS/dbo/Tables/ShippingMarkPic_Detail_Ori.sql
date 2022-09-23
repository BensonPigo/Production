CREATE TABLE [dbo].[ShippingMarkPic_Detail_Ori](
	[SCICtnNo] [varchar](16) NOT NULL,
	[Side] [varchar](5) NOT NULL,
	[Seq] [int] NOT NULL,
	[FilePath] [varchar](80) NULL,
	[FileName] [varchar](30) NULL,
	[CmdTime] [datetime] NOT NULL,
	[SunriseUpdated] [bit] NOT NULL,
	[GenSongUpdated] [bit] NOT NULL,
	[Image] [varbinary](max) NULL,
	[GensongUpdateTime] [varchar](50) NULL,
 CONSTRAINT [PK_ShippingMarkPic_Detail_Ori] PRIMARY KEY CLUSTERED 
(
	[SCICtnNo] ASC,
	[Side] ASC,
	[Seq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail_Ori] ADD  DEFAULT ((0)) FOR [SunriseUpdated]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail_Ori] ADD  DEFAULT ((0)) FOR [GenSongUpdated]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail_Ori] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_Ori_GensongUpdateTime]  DEFAULT ((0)) FOR [GensongUpdateTime]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'SCICtnNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'貼碼面, 上下左右前後' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'Side'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'圖檔位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'FilePath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'圖檔名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'FileName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SCI寫入/更新此筆資料時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'CmdTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sunrise是否已轉製' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'SunriseUpdated'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GenSong是否已轉製' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'GenSongUpdated'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gensong 判斷資料不需要同步時會寫入當下日期，主要使用此欄位 + CmdTime 排序資料更新的順序。' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingMarkPic_Detail_Ori', @level2type=N'COLUMN',@level2name=N'GensongUpdateTime'
GO

