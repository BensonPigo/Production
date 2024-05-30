CREATE TABLE [dbo].[MtlType_Brand](
	[ID] [varchar](20) NOT NULL CONSTRAINT [DF_MtlType_Brand_ID]  DEFAULT '',
	[BrandID] [varchar](8) NOT NULL CONSTRAINT [DF_MtlType_Brand_BrandID]  DEFAULT '',
	[BossLockDay] [int] NOT NULL CONSTRAINT [DF_MtlType_Brand_BossLockDay]  DEFAULT 0,
	[IsSustainableMaterial] [bit] NOT NULL CONSTRAINT [DF_MtlType_Brand_IsSustainableMaterial]  DEFAULT 0,
	[IsSustainableMaterialforSMTT] [bit] NOT NULL CONSTRAINT [DF_MtlType_Brand_IsSustainableMaterialforSMTT]  DEFAULT 0,
	[IsBCIforSMTT] [bit] NOT NULL CONSTRAINT [DF_MtlType_Brand_IsBCIforSMTT]  DEFAULT 0,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[BrandID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MtlType_Brand', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MtlType_Brand', @level2type=N'COLUMN',@level2name=N'BrandID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Boss Lock Day' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MtlType_Brand', @level2type=N'COLUMN',@level2name=N'BossLockDay'
GO


