CREATE TABLE [dbo].[InspectionType_Detail](
	[Function] [varchar](30) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[InspectionTypeID] [bigint] NOT NULL,
 CONSTRAINT [PK_InspectionType_Detail] PRIMARY KEY CLUSTERED 
(
	[Function] ASC,
	[BrandID] ASC,
	[InspectionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[InspectionType_Detail] ADD  CONSTRAINT [DF_InspectionType_Detail_Function]  DEFAULT ('') FOR [Function]
GO

ALTER TABLE [dbo].[InspectionType_Detail] ADD  CONSTRAINT [DF_InspectionType_Detail_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[InspectionType_Detail] ADD  CONSTRAINT [DF_InspectionType_Detail_InspectionTypeID]  DEFAULT ((0)) FOR [InspectionTypeID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QKPI功能名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType_Detail', @level2type=N'COLUMN',@level2name=N'Function'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType_Detail', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InspectionType.ID，該功能套用哪一種檢驗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'InspectionType_Detail', @level2type=N'COLUMN',@level2name=N'InspectionTypeID'
GO