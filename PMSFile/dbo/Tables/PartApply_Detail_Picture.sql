CREATE TABLE [dbo].[PartApply_Detail_Picture](
	[Ukey] BIGINT IDENTITY(1,1) NOT NULL,
	[PartApplyDetailUkey] BIGINT NOT NULL,
	[Picture] [varbinary](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PartApply_Detail_Picture] ADD  CONSTRAINT [DF_PartApply_Detail_Picture_PartApplyDetailUkey]  DEFAULT ((0)) FOR [PartApplyDetailUkey]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'零件照片主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PartApply_Detail_Picture', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申請明細主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PartApply_Detail_Picture', @level2type=N'COLUMN',@level2name=N'PartApplyDetailUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'維修圖片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PartApply_Detail_Picture', @level2type=N'COLUMN',@level2name=N'Picture'
GO