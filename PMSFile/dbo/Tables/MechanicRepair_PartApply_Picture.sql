CREATE TABLE [dbo].[MechanicRepair_PartApply_Picture](
	[Ukey] BIGINT IDENTITY(1,1) NOT NULL,
	[MechanicRepairPartApplyUkey] BIGINT NOT NULL,
	[Picture] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_MechanicRepair_PartApply_Picture] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MechanicRepair_PartApply_Picture] ADD  CONSTRAINT [DF_MechanicRepair_PartApply_Picture_MechanicRepairPartApplyUkey]  DEFAULT ((0)) FOR [MechanicRepairPartApplyUkey]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'零件照片主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MechanicRepair_PartApply_Picture', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'零件申請主鍵' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MechanicRepair_PartApply_Picture', @level2type=N'COLUMN',@level2name=N'MechanicRepairPartApplyUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'維修圖片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MechanicRepair_PartApply_Picture', @level2type=N'COLUMN',@level2name=N'Picture'
GO


