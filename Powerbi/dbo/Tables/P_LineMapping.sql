CREATE TABLE [dbo].[P_LineMapping](
	[FactoryID] [varchar](8) NOT NULL,
	[StyleUKey] [bigint] NOT NULL,
	[ComboType] [varchar](1) NOT NULL,
	[Version] [tinyint] NOT NULL,
	[Phase] [varchar](7) NOT NULL,
	[SewingLine] [varchar](8) NOT NULL,
	[IsFrom] [varchar](6) NOT NULL,
	[Team] [varchar](8) NOT NULL,
	[ID] [bigint] NOT NULL,
	[Style] [varchar](15) NOT NULL,
	[Season] [varchar](10) NOT NULL,
	[Brand] [varchar](8) NOT NULL,
	[Desc.] [varchar](100) NOT NULL,
	[CPU/PC] [decimal](5, 3) NOT NULL,
	[No. of Sewer] [tinyint] NOT NULL,
	[LBR By GSD Time(%)] [numeric](7, 2) NOT NULL,
	[Total GSD Time] [numeric](7, 2) NOT NULL,
	[Avg. GSD Time] [numeric](7, 2) NOT NULL,
	[Highest GSD Time] [numeric](12, 2) NULL,
	[LBR By Cycle Time(%)] [numeric](7, 2) NOT NULL,
	[Total Cycle Time] [numeric](7, 2) NOT NULL,
	[Avg. Cycle Time] [numeric](7, 2) NOT NULL,
	[Highest Cycle Time] [numeric](6, 2) NOT NULL,
	[Total % Time Diff(%)] [int] NOT NULL,
	[No. of Hours] [numeric](3, 1) NOT NULL,
	[Oprts of Presser] [tinyint] NOT NULL,
	[Oprts of Packer] [tinyint] NOT NULL,
	[Ttl Sew Line Oprts] [tinyint] NOT NULL,
	[Target / Hr.(100%)] [int] NOT NULL,
	[Daily Demand / Shift] [numeric](7, 1) NOT NULL,
	[Takt Time] [numeric](6, 2) NOT NULL,
	[EOLR] [numeric](6, 2) NOT NULL,
	[PPH] [numeric](6, 2) NOT NULL,
	[GSD Status] [varchar](15) NOT NULL,
	[GSD Version] [varchar](2) NOT NULL,
	[Status] [varchar](9) NOT NULL,
	[Add Name] [varchar](10) NOT NULL,
	[Add Date] [datetime] NULL,
	[Edit Name] [varchar](10) NOT NULL,
	[Edit Date] [datetime] NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK__P_LineMa__3214EC271FA606EB] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[StyleUKey] ASC,
	[ComboType] ASC,
	[Version] ASC,
	[Phase] ASC,
	[SewingLine] ASC,
	[IsFrom] ASC,
	[Team] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Factory]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_StyleUKey]  DEFAULT ((0)) FOR [StyleUKey]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Version]  DEFAULT ('') FOR [Version]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Phase]  DEFAULT ('') FOR [Phase]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_SewingLine]  DEFAULT ('') FOR [SewingLine]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_IsFrom]  DEFAULT ('') FOR [IsFrom]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Style]  DEFAULT ('') FOR [Style]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Season]  DEFAULT ('') FOR [Season]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Brand]  DEFAULT ('') FOR [Brand]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Desc.]  DEFAULT ('') FOR [Desc.]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_CPU/PC]  DEFAULT ((0)) FOR [CPU/PC]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_No. of Sewer]  DEFAULT ((0)) FOR [No. of Sewer]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_LBR By GSD Time(%)]  DEFAULT ((0)) FOR [LBR By GSD Time(%)]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Total GSD Time]  DEFAULT ((0)) FOR [Total GSD Time]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Avg. GSD Time]  DEFAULT ((0)) FOR [Avg. GSD Time]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Highest GSD Time]  DEFAULT ((0)) FOR [Highest GSD Time]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_LBR By Cycle Time(%)]  DEFAULT ((0)) FOR [LBR By Cycle Time(%)]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Total Cycle Time]  DEFAULT ((0)) FOR [Total Cycle Time]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Avg. Cycle Time]  DEFAULT ((0)) FOR [Avg. Cycle Time]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Highest Cycle Time]  DEFAULT ((0)) FOR [Highest Cycle Time]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Total % Time Diff(%)]  DEFAULT ((0)) FOR [Total % Time Diff(%)]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_No. of Hours]  DEFAULT ((0)) FOR [No. of Hours]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Oprts of Presser]  DEFAULT ((0)) FOR [Oprts of Presser]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Oprts of Packer]  DEFAULT ((0)) FOR [Oprts of Packer]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Ttl Sew Line Oprts]  DEFAULT ((0)) FOR [Ttl Sew Line Oprts]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Target / Hr.(100%)]  DEFAULT ((0)) FOR [Target / Hr.(100%)]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Daily Demand / Shift]  DEFAULT ((0)) FOR [Daily Demand / Shift]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Takt Time]  DEFAULT ((0)) FOR [Takt Time]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_EOLR]  DEFAULT ((0)) FOR [EOLR]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_PPH]  DEFAULT ((0)) FOR [PPH]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_GSD Status]  DEFAULT ('') FOR [GSD Status]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_GSD Version]  DEFAULT ('') FOR [GSD Version]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Status]  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Add Name]  DEFAULT ('') FOR [Add Name]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_Edit Name]  DEFAULT ('') FOR [Edit Name]
GO

ALTER TABLE [dbo].[P_LineMapping] ADD  CONSTRAINT [DF_P_LineMapping_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串Style.Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'StyleUKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM版號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Phase'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'SewingLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源為IE P03或IE P06' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'IsFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串表身ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Style'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Season'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Brand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Style說明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Desc.'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'產能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'CPU/PC'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車工人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'No. of Sewer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total GSD Time] / [Highest GSD Time] / [No. of Sewer] × 100%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'LBR By GSD Time(%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總GSD時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Total GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total GSD Time] / [No. of Sewer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Avg. GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最高的GSD時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Highest GSD Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total Cycle Time] / [Highest Cycle Time] / [No. of Sewer] × 100%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'LBR By Cycle Time(%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總CycleTime時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Total Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Total Cycle Time] / [No. of Sewer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Avg. Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最高的CycleTime時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Highest Cycle Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'([Total GSD Time]-[Total Cycle Time]) / [Total Cycle Time]×100%' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Total % Time Diff(%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每站工時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'No. of Hours'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'整燙人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Oprts of Presser'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'包裝人力' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Oprts of Packer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[No. Of Sewer] + [Oprts Of Presser] + [Oprts Of Packer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Ttl Sew Line Oprts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3600 × [No. of Sewer] / [Total Cycle Time]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Target / Hr.(100%)'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[Target / Hr. (100%)] × [No. of Hours]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Daily Demand / Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3600 × [No. Of Hours] / [Daily Demand / Shift]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Takt Time'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'3600 / [Highest Cycle Time]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'EOLR'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'[ELOR] × [CPU /PC] / [No. Of Sewer]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'PPH'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty GSD狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'GSD Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty GSD版號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'GSD Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line Mapping狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Add Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Add Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Edit Name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'Edit Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO