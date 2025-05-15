CREATE TABLE [dbo].[ArtworkType](
	[ID] [varchar](20) NOT NULL,
	[Abbreviation] [varchar](2) NOT NULL,
	[Classify] [varchar](1) NOT NULL,
	[Seq] [varchar](4) NOT NULL,
	[Junk] [bit] NOT NULL,
	[ArtworkUnit] [varchar](10) NOT NULL,
	[ProductionUnit] [varchar](10) NOT NULL,
	[IsTMS] [bit] NOT NULL,
	[IsPrice] [bit] NOT NULL,
	[IsArtwork] [bit] NOT NULL,
	[IsTtlTMS] [bit] NOT NULL,
	[IsSubprocess] [bit] NOT NULL,
	[Remark] [nvarchar](60) NOT NULL,
	[ReportDropdown] [bit] NOT NULL,
	[UseArtwork] [bit] NOT NULL,
	[SystemType] [varchar](1) NOT NULL,
	[InhouseOSP] [varchar](1) NOT NULL,
	[AccountNo] [varchar](8) NOT NULL,
	[BcsLt] [decimal](2, 1) NOT NULL,
	[CutLt] [tinyint] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NOT NULL,
	[EditDate] [datetime] NULL,
	[PostSewingDays] [int] NOT NULL,
	[IsPrintToCMP] [bit] NOT NULL,
	[IsLocalPurchase] [bit] NOT NULL,
	[IsImportTestDox] [bit] NOT NULL,
	[IsSubCon] [bit] NOT NULL,
	[SubconFarmInOutfromSewOutput] [bit] NOT NULL,
 CONSTRAINT [PK_ArtworkType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_Abbreviation]  DEFAULT ('') FOR [Abbreviation]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_Classify]  DEFAULT ('') FOR [Classify]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_Seq]  DEFAULT ('') FOR [Seq]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_Junk]  DEFAULT ((0)) FOR [Junk]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_ArtworkUnit]  DEFAULT ('') FOR [ArtworkUnit]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_ProductionUnit]  DEFAULT ('') FOR [ProductionUnit]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsTMS]  DEFAULT ((0)) FOR [IsTMS]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsPrice]  DEFAULT ((0)) FOR [IsPrice]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsArtwork]  DEFAULT ((0)) FOR [IsArtwork]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsTtlTMS]  DEFAULT ((0)) FOR [IsTtlTMS]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsSubprocess]  DEFAULT ((0)) FOR [IsSubprocess]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_Remark]  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_ReportDropdown]  DEFAULT ((0)) FOR [ReportDropdown]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_UseArtwork]  DEFAULT ((0)) FOR [UseArtwork]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_SystemType]  DEFAULT ('') FOR [SystemType]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_InhouseOSP]  DEFAULT ('') FOR [InhouseOSP]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_AccountNo]  DEFAULT ('') FOR [AccountNo]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_BcsLt]  DEFAULT ((0)) FOR [BcsLt]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_CutLt]  DEFAULT ((0)) FOR [CutLt]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_PostSewingDays]  DEFAULT ((0)) FOR [PostSewingDays]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsPrintToCMP]  DEFAULT ((0)) FOR [IsPrintToCMP]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsLocalPurchase]  DEFAULT ((0)) FOR [IsLocalPurchase]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  DEFAULT ((0)) FOR [IsImportTestDox]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_IsSubCon]  DEFAULT ((0)) FOR [IsSubCon]
GO

ALTER TABLE [dbo].[ArtworkType] ADD  CONSTRAINT [DF_ArtworkType_SubconFarmInOutfromSewOutput]  DEFAULT ((0)) FOR [SubconFarmInOutfromSewOutput]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'作工代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'簡碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'Abbreviation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'大分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'Classify'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'序號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'取消' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'Junk'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'ArtworkUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產能單位設定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'ProductionUnit'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'秒數換算成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'IsTMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'直接輸入成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'IsPrice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否加入Artwork' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'IsArtwork'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否加入ttl TMS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'IsTtlTMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為SubProcess' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'IsSubprocess'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'報表下拉顯示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'ReportDropdown'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'允許建立在Artwork' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'UseArtwork'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統區分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'SystemType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'InHouse/OSP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'InhouseOSP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'會科- 銷貨成本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'AccountNo'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subprocess BCS Lead Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'BcsLt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準裁剪Leadtime' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'CutLt'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用來判斷此item是否可以在工廠端當地採購' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'IsLocalPurchase'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否需要外發加工，主要應用在 WH P54' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'IsSubCon'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Subcon Farm In/Out data from Sewing Output; Farm Out = Sewing Output date - 2 day; Farm In = Sewing Output date - 1 day;' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType', @level2type=N'COLUMN',@level2name=N'SubconFarmInOutfromSewOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Artwork Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ArtworkType'
GO