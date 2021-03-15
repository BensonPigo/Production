CREATE TABLE [dbo].[ShippingMarkPic_Detail](
	[ShippingMarkPicUkey] [bigint] NOT NULL,
	[SCICtnNo] [varchar](15) NOT NULL,
	[FileName] [varchar](30) NOT NULL,
	[Image] [varbinary](max) NULL,
	[ShippingMarkCombinationUkey] [bigint] NOT NULL,
	[ShippingMarkTypeUkey] [bigint] NOT NULL,
	[Side] [varchar](5) NOT NULL,
	[Seq] [int] NOT NULL,
	[Is2Side] [bit] NOT NULL,
	[IsHorizontal] [bit] NOT NULL,
	[IsSSCC] [bit] NOT NULL,
	[FromRight] [numeric](8, 2) NOT NULL,
	[FromBottom] [numeric](8, 2) NOT NULL,
	[Width] [int] NOT NULL,
	[Length] [int] NOT NULL,
	[DPI] INT NOT NULL DEFAULT 0, 
	FilePath Varchar (150) NOT NULL CONSTRAINT [DF_ShippingMarkPic_Detail_FilePath]  DEFAULT(''),
	CtnHeight numeric (8, 4) NOT NULL CONSTRAINT [DF_ShippingMarkPic_Detail_CtnHeight]  DEFAULT (0),
	IsOverCtnHt bit NOT NULL CONSTRAINT [DF_ShippingMarkPic_Detail_IsOverCtnHt]  DEFAULT 0,
	NotAutomate bit NOT NULL CONSTRAINT [DF_ShippingMarkPic_Detail_NotAutomate]  DEFAULT 0,
    CONSTRAINT [PK_ShippingMarkPic_Detail] PRIMARY KEY CLUSTERED 
(
	[ShippingMarkPicUkey] ASC,
	[SCICtnNo] ASC,
	[ShippingMarkTypeUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_FileName]  DEFAULT ('') FOR [FileName]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_ShippingMarkCombinationUkey]  DEFAULT ((0)) FOR [ShippingMarkCombinationUkey]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_ShippingMarkTypeUkey]  DEFAULT ((0)) FOR [ShippingMarkTypeUkey]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_Side]  DEFAULT ('') FOR [Side]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_Seq]  DEFAULT ((0)) FOR [Seq]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_Is2Side]  DEFAULT ((0)) FOR [Is2Side]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_IsHorizontal]  DEFAULT ((0)) FOR [IsHorizontal]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_IsSSCC]  DEFAULT ((0)) FOR [IsSSCC]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_FromRight]  DEFAULT ((0)) FOR [FromRight]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_FromBottom]  DEFAULT ((0)) FOR [FromBottom]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_Width]  DEFAULT ((0)) FOR [Width]
GO

ALTER TABLE [dbo].[ShippingMarkPic_Detail] ADD  CONSTRAINT [DF_ShippingMarkPic_Detail_Length]  DEFAULT ((0)) FOR [Length]
GO


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖檔名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'SCICtnNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貼碼面, 上下左右前後', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Side';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉 HTML 的 DPI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'DPI';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖檔位置'
, @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail'
, @level2type = N'COLUMN', @level2name = N'FilePath';
;	
GO