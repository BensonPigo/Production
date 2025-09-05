CREATE TABLE [dbo].[System](
	[Mailserver] [varchar](60) NOT NULL,
	[Sendfrom] [varchar](40) NOT NULL,
	[EmailID] [varchar](40) NOT NULL,
	[EmailPwd] [varchar](20) NOT NULL,
	[PicPath] [varchar](80) NOT NULL,
	[StdTMS] [int] NOT NULL,
	[ClipPath] [varchar](80) NOT NULL,
	[FtpIP] [varchar](36) NOT NULL,
	[FtpID] [varchar](10) NOT NULL,
	[FtpPwd] [varchar](36) NOT NULL,
	[SewLock] [date] NULL,
	[SampleRate] [tinyint] NOT NULL,
	[PullLock] [date] NULL,
	[RgCode] [varchar](3) NOT NULL,
	[ImportDataPath] [varchar](60) NOT NULL,
	[ImportDataFileName] [varchar](60) NOT NULL,
	[ExportDataPath] [varchar](60) NOT NULL,
	[CurrencyID] [varchar](4) NOT NULL,
	[USDRate] [decimal](9, 4) NOT NULL,
	[POApproveName] [varchar](10) NOT NULL,
	[POApproveDay] [tinyint] NOT NULL,
	[CutDay] [tinyint] NOT NULL,
	[AccountKeyword] [varchar](1) NOT NULL,
	[ReadyDay] [tinyint] NOT NULL,
	[VNMultiple] [decimal](4, 2) NOT NULL,
	[MtlLeadTime] [tinyint] NOT NULL,
	[ExchangeID] [varchar](2) NOT NULL,
	[RFIDServerName] [varchar](20) NOT NULL,
	[RFIDDatabaseName] [varchar](20) NOT NULL,
	[RFIDLoginId] [varchar](20) NOT NULL,
	[RFIDLoginPwd] [varchar](20) NOT NULL,
	[RFIDTable] [varchar](20) NOT NULL,
	[ProphetSingleSizeDeduct] [decimal](3, 0) NOT NULL,
	[PrintingSuppID] [varchar](8) NOT NULL,
	[QCMachineDelayTime] [decimal](2, 1) NOT NULL,
	[APSLoginId] [varchar](15) NOT NULL,
	[APSLoginPwd] [varchar](15) NOT NULL,
	[SQLServerName] [varchar](130) NOT NULL,
	[APSDatabaseName] [varchar](15) NOT NULL,
	[RFIDMiddlewareInRFIDServer] [bit] NOT NULL,
	[UseAutoScanPack] [bit] NOT NULL,
	[MtlAutoLock] [bit] NOT NULL,
	[InspAutoLockAcc] [bit] NOT NULL,
	[ShippingMarkPath] [varchar](80) NOT NULL,
	[StyleSketch] [varchar](80) NOT NULL,
	[ARKServerName] [varchar](20) NOT NULL,
	[ARKDatabaseName] [varchar](20) NOT NULL,
	[ARKLoginId] [varchar](20) NOT NULL,
	[ARKLoginPwd] [varchar](20) NOT NULL,
	[MarkerInputPath] [nvarchar](80) NOT NULL,
	[MarkerOutputPath] [nvarchar](80) NOT NULL,
	[ReplacementReport] [varchar](80) NOT NULL,
	[CuttingP10mustCutRef] [bit] NOT NULL,
	[Automation] [bit] NOT NULL,
	[AutomationAutoRunTime] [tinyint] NOT NULL,
	[CanReviseDailyLockData] [bit] NOT NULL,
	[AutoGenerateByTone] [bit] NOT NULL,
	[MiscPOApproveName] [varchar](10) NOT NULL,
	[MiscPOApproveDay] [tinyint] NOT NULL,
	[QMSAutoAdjustMtl] [bit] NOT NULL,
	[ShippingMarkTemplatePath] [varchar](80) NOT NULL,
	[WIP_FollowCutOutput] [bit] NOT NULL,
	[NoRestrictOrdersDelivery] [bit] NOT NULL,
	[WIP_ByShell] [bit] NOT NULL,
	[RFCardEraseBeforePrinting] [bit] NOT NULL,
	[SewlineAvgCPU] [int] NOT NULL,
	[SmallLogoCM] [decimal](5, 2) NOT NULL,
	[CheckRFIDCardDuplicateByWebservice] [bit] NOT NULL,
	[IsCombineSubProcess] [bit] NOT NULL,
	[IsNoneShellNoCreateAllParts] [bit] NOT NULL,
	[Region] [varchar](2) NOT NULL,
	[FinalInspection_CTNMoistureStandard] [numeric](5, 2) NOT NULL,
	[StyleFDFilePath] [varchar](80) NOT NULL,
	[StyleRRLRPath] [varchar](80) NOT NULL,
	[WH_MtlTransChkLocation] [bit] NOT NULL,
	[CartonTransferToSisterFty] [bit] NOT NULL,
	[MailServerPort] [smallint] NOT NULL,
	[PMS_FabricQRCode_LabelSize] [varchar](5) NOT NULL,
	[PDA_FabricQRCode_LabelSize] [varchar](5) NOT NULL,
	[HandoverATPath] [varchar](80) NOT NULL,
	[HandoverSpecialToolsPath] [varchar](80) NOT NULL,
	[CriticalOperationPath] [varchar](80) NOT NULL,
	[FinalPatternPath] [varchar](80) NOT NULL,
	[PadPrintPath] [varchar](80) NOT NULL,
	[FabricPath] [nvarchar](120) NOT NULL,
	[ColorPath] [nvarchar](120) NOT NULL,
	[IsLoginCheckADAccount] [bit] NOT NULL,
	[NikeFactoryCode] [varchar](3) NOT NULL,
	[NikeStickerPrintServer] [varchar](20) NOT NULL,
	[NikeStickerPrintFileFolder] [varchar](100) NOT NULL,
	[FtpIPDummy] [varchar](36) NOT NULL,
	[FtpIDDummy] [varchar](20) NOT NULL,
	[FtpPwdDummy] [varchar](36) NOT NULL,
	[ImportDataPathDummy] [varchar](60) NOT NULL,
	[ImportDataFileNameDummy] [varchar](60) NOT NULL,
	[ExportDataPathDummy] [varchar](60) NOT NULL,
	[IEPath] [nvarchar](120) NOT NULL,
	[FtpPortDummy] [varchar](4) NOT NULL,
	[FtpPort] [varchar](5) NOT NULL,
	[BITimeout] [numeric](18, 0) NOT NULL,
	[isNeedOTPFty] [bit] NOT NULL,
	[SFtpIP] [varchar](36) NOT NULL,
	[SFtpPort] [varchar](5) NOT NULL,
	[SFtpID] [varchar](20) NOT NULL,
	[SFtpPwd] [varchar](36) NOT NULL,
	[SFtpIPDummy] [varchar](36) NOT NULL,
	[SFtpPortDummy] [varchar](5) NOT NULL,
	[SFtpIDDummy] [varchar](20) NOT NULL,
	[SFtpPwdDummy] [varchar](36) NOT NULL,
	[MercuryErrorEmailLastDate] [date] NULL,
	[UseCutRefToRequestFabric] [tinyint] NOT NULL,
	[QASortOutStandard] [decimal](5, 2) NOT NULL,
	[JukiExchangeDBActive] [bit] NOT NULL,
	[JukiExchangeDBServerName] [varchar](20) NOT NULL,
	[JukiExchangeDBDataBaseName] [varchar](20) NOT NULL,
	[JukiExchangeDBLoginID] [varchar](20) NOT NULL,
	[JukiExchangeDBLoginPWD] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RgCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_Mailserver]  DEFAULT ('') FOR [Mailserver]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_Sendfrom]  DEFAULT ('') FOR [Sendfrom]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_EmailID]  DEFAULT ('') FOR [EmailID]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_EmailPwd]  DEFAULT ('') FOR [EmailPwd]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_PicPath]  DEFAULT ('') FOR [PicPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_StdTMS]  DEFAULT ((0)) FOR [StdTMS]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ClipPath]  DEFAULT ('') FOR [ClipPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpIP]  DEFAULT ('') FOR [FtpIP]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpID]  DEFAULT ('') FOR [FtpID]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpPwd]  DEFAULT ('') FOR [FtpPwd]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SampleRate]  DEFAULT ((0)) FOR [SampleRate]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_RgCode]  DEFAULT ('') FOR [RgCode]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ImportDataPath]  DEFAULT ('') FOR [ImportDataPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ImportDataFileName]  DEFAULT ('') FOR [ImportDataFileName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ExportDataPath]  DEFAULT ('') FOR [ExportDataPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_CurrencyID]  DEFAULT ('') FOR [CurrencyID]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_USDRate]  DEFAULT ((0)) FOR [USDRate]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_POApproveName]  DEFAULT ('') FOR [POApproveName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_POApproveDay]  DEFAULT ((0)) FOR [POApproveDay]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_CutDay]  DEFAULT ((0)) FOR [CutDay]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_AccountKeyword]  DEFAULT ('') FOR [AccountKeyword]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ReadyDay]  DEFAULT ((0)) FOR [ReadyDay]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_VNMultiple]  DEFAULT ((0)) FOR [VNMultiple]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_MtlLeadTime]  DEFAULT ((0)) FOR [MtlLeadTime]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ExchangeID]  DEFAULT ('') FOR [ExchangeID]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_RFIDServerName]  DEFAULT ('') FOR [RFIDServerName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_RFIDDatabaseName]  DEFAULT ('') FOR [RFIDDatabaseName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_RFIDLoginId]  DEFAULT ('') FOR [RFIDLoginId]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_RFIDLoginPwd]  DEFAULT ('') FOR [RFIDLoginPwd]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_RFIDTable]  DEFAULT ('') FOR [RFIDTable]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ProphetSingleSizeDeduct]  DEFAULT ((0)) FOR [ProphetSingleSizeDeduct]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_PrintingSuppID]  DEFAULT ('') FOR [PrintingSuppID]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_QCMachineDelayTime]  DEFAULT ((0)) FOR [QCMachineDelayTime]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_APSLoginId]  DEFAULT ('') FOR [APSLoginId]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_APSLoginPwd]  DEFAULT ('') FOR [APSLoginPwd]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SQLServerName]  DEFAULT ('') FOR [SQLServerName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_APSDatabaseName]  DEFAULT ('') FOR [APSDatabaseName]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [RFIDMiddlewareInRFIDServer]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_UseAutoScanPack]  DEFAULT ((0)) FOR [UseAutoScanPack]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_MtlAutoLock]  DEFAULT ((0)) FOR [MtlAutoLock]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [InspAutoLockAcc]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ShippingMarkPath]  DEFAULT ('') FOR [ShippingMarkPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_StyleSketch]  DEFAULT ('') FOR [StyleSketch]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ARKServerName]  DEFAULT ('') FOR [ARKServerName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ARKDatabaseName]  DEFAULT ('') FOR [ARKDatabaseName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ARKLoginId]  DEFAULT ('') FOR [ARKLoginId]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ARKLoginPwd]  DEFAULT ('') FOR [ARKLoginPwd]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_MarkerInputPath]  DEFAULT ('') FOR [MarkerInputPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_MarkerOutputPath]  DEFAULT ('') FOR [MarkerOutputPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ReplacementReport]  DEFAULT ('') FOR [ReplacementReport]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [CuttingP10mustCutRef]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [Automation]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_AutomationAutoRunTime]  DEFAULT ((0)) FOR [AutomationAutoRunTime]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [CanReviseDailyLockData]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_AutoGenerateByTone]  DEFAULT ((0)) FOR [AutoGenerateByTone]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_MiscPOApproveName]  DEFAULT ('') FOR [MiscPOApproveName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_MiscPOApproveDay]  DEFAULT ((0)) FOR [MiscPOApproveDay]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [QMSAutoAdjustMtl]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_WIP_FollowCutOutput]  DEFAULT ((0)) FOR [WIP_FollowCutOutput]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_NoRestrictOrdersDelivery]  DEFAULT ((0)) FOR [NoRestrictOrdersDelivery]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_WIP_ByShell]  DEFAULT ((0)) FOR [WIP_ByShell]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [RFCardEraseBeforePrinting]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SewlineAvgCPU]  DEFAULT ((0)) FOR [SewlineAvgCPU]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SmallLogoCM]  DEFAULT ((0)) FOR [SmallLogoCM]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_CheckRFIDCardDuplicateByWebservice]  DEFAULT ((0)) FOR [CheckRFIDCardDuplicateByWebservice]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_IsCombineSubProcess]  DEFAULT ((0)) FOR [IsCombineSubProcess]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_IsNoneShellNoCreateAllParts]  DEFAULT ((0)) FOR [IsNoneShellNoCreateAllParts]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_Region]  DEFAULT ('') FOR [Region]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FinalInspection_CTNMoistureStandard]  DEFAULT ((0)) FOR [FinalInspection_CTNMoistureStandard]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [StyleFDFilePath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [StyleRRLRPath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [WH_MtlTransChkLocation]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_CartonTransferToSisterFty]  DEFAULT ((0)) FOR [CartonTransferToSisterFty]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_MailServerPort]  DEFAULT ((0)) FOR [MailServerPort]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_PMS_FabricQRCode_LabelSize]  DEFAULT ('5X5') FOR [PMS_FabricQRCode_LabelSize]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_PDA_FabricQRCode_LabelSize]  DEFAULT ('5X5') FOR [PDA_FabricQRCode_LabelSize]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [HandoverATPath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [HandoverSpecialToolsPath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [CriticalOperationPath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [FinalPatternPath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [PadPrintPath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [FabricPath]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [ColorPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_IsLoginCheckADAccount]  DEFAULT ((0)) FOR [IsLoginCheckADAccount]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_NikeFactoryCode]  DEFAULT ('') FOR [NikeFactoryCode]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_NikeStickerPrintServer]  DEFAULT ('') FOR [NikeStickerPrintServer]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_NikeStickerPrintFileFolder]  DEFAULT ('') FOR [NikeStickerPrintFileFolder]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpIPDummy]  DEFAULT ('') FOR [FtpIPDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpIDDummy]  DEFAULT ('') FOR [FtpIDDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpPwdDummy]  DEFAULT ('') FOR [FtpPwdDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ImportDataPathDummy]  DEFAULT ('') FOR [ImportDataPathDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ImportDataFileNameDummy]  DEFAULT ('') FOR [ImportDataFileNameDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_ExportDataPathDummy]  DEFAULT ('') FOR [ExportDataPathDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_IEPath]  DEFAULT ('') FOR [IEPath]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpPortDummy]  DEFAULT ('') FOR [FtpPortDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_FtpPort]  DEFAULT ('') FOR [FtpPort]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [BITimeout]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_isNeedOTPFty]  DEFAULT ((0)) FOR [isNeedOTPFty]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpIP]  DEFAULT ('') FOR [SFtpIP]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpPort]  DEFAULT ('') FOR [SFtpPort]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpID]  DEFAULT ('') FOR [SFtpID]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpPwd]  DEFAULT ('') FOR [SFtpPwd]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpIPDummy]  DEFAULT ('') FOR [SFtpIPDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpPortDummy]  DEFAULT ('') FOR [SFtpPortDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpIDDummy]  DEFAULT ('') FOR [SFtpIDDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_SFtpPwdDummy]  DEFAULT ('') FOR [SFtpPwdDummy]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_UseCutRefToRequestFabric]  DEFAULT ((0)) FOR [UseCutRefToRequestFabric]
GO

ALTER TABLE [dbo].[System] ADD  DEFAULT ((0)) FOR [QASortOutStandard]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_JukiExchangeDBActive]  DEFAULT ((0)) FOR [JukiExchangeDBActive]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_JukiExchangeDBServerName]  DEFAULT ('') FOR [JukiExchangeDBServerName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_JukiExchangeDBDataBaseName]  DEFAULT ('') FOR [JukiExchangeDBDataBaseName]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_JukiExchangeDBLoginID]  DEFAULT ('') FOR [JukiExchangeDBLoginID]
GO

ALTER TABLE [dbo].[System] ADD  CONSTRAINT [DF_System_JukiExchangeDBLoginPWD]  DEFAULT ('') FOR [JukiExchangeDBLoginPWD]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mail Server IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'Mailserver'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'寄件者名稱(有@)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'Sendfrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'EmailID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email Password' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'EmailPwd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Picture Path' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'PicPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CPU和TMS換算值(1400)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'StdTMS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'夾檔存放位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ClipPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FTP IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpIP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FTP ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FTP PASSWORD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpPwd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'車縫日報表月結日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SewLock'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'銷樣單的倍數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SampleRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pullout Report月結日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'PullLock'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'區域代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'RgCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料上傳位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ImportDataPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料上傳檔名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ImportDataFileName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料下載位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ExportDataPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'當地幣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'CurrencyID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'美金匯率' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'USDRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Local Purchase Approve Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'POApproveName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Local Purchase 自動Approve的天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'POApproveDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'裁剪上線日為Sewing Inline的前幾日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'CutDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'帳號保留字' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'AccountKeyword'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Production Ready day' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ReadyDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'VN出口報關調整營收倍數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'VNMultiple'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Material Leat-time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'MtlLeadTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rate Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ExchangeID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'驗布機延遲時間(秒)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'QCMachineDelayTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Misc Local Purchase Approve Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'MiscPOApproveName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Misc Local Purchase 自動Approve的天數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'MiscPOApproveDay'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QMS檢驗自動調整長度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'QMSAutoAdjustMtl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標籤範本檔路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ShippingMarkTemplatePath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RF Card Erase Before Printing' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'RFCardEraseBeforePrinting'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PH 產線平均一天的 CPU' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SewlineAvgCPU'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'以下判斷 smalll logo 的長度標準 (CM)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SmallLogoCM'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否特過WS來檢查RFID卡重複使用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'CheckRFIDCardDuplicateByWebservice'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否combine subprocess產生bundle' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'IsCombineSubProcess'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否不產生AllParts' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'IsNoneShellNoCreateAllParts'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠區' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'Region'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Final Inspection Carton Moisure Standard' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FinalInspection_CTNMoistureStandard'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FD 檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'StyleFDFilePath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RR, LR 檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'StyleRRLRPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'判斷轉倉庫 (包含借料, 轉庫存, 領庫 等) 是否需要填寫 From & To Location' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'WH_MtlTransChkLocation'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否要將箱子轉至姊妹廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'CartonTransferToSisterFty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SMTP Port' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'MailServerPort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PMS 列印布捲 QR Code 的標籤大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'PMS_FabricQRCode_LabelSize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PDA 列印布捲 QR Code 的標籤大小' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'PDA_FabricQRCode_LabelSize'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'放Template/Auto Template檔案的路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'HandoverATPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'放Special Tools檔案的路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'HandoverSpecialToolsPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'放Critical Operation檔案的路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'CriticalOperationPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'放Final Pattern and Marker List檔案的路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FinalPatternPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PadPrintLayout檔案路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'PadPrintPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料圖片路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FabricPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'顏色圖片路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ColorPath'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否檢查AD帳號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'IsLoginCheckADAccount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike 系統的廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'NikeFactoryCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike 標籤虛擬印表機 ( PDF )' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'NikeStickerPrintServer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nike 標籤生成後下載的路徑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'NikeStickerPrintFileFolder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料交換dummy ftp ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpIPDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料交換dummy ftp 帳號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpIDDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料交換dummy ftp 密碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpPwdDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料交換dummy Import檔案位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ImportDataPathDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料交換dummy import檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ImportDataFileNameDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料交換dummy 轉出檔案位置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'ExportDataPathDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dummy的 FTP Port' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpPortDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Formal的 FTP Port' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'FtpPort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該工廠是否啟用OTP驗證功能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'isNeedOTPFty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SFTP IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpIP'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Formal的SFTP連接埠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpPort'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SFTP ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SFTP Password' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpPwd'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dummy的SFTP IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpIPDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dummy的SFTP連接埠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpPortDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SFTP ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpIDDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SFTP Password' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'SFtpPwdDummy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mercury Error Email 最後寄出日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'MercuryErrorEmailLastDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該區是否使用裁剪裁次要料' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'UseCutRefToRequestFabric'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否啟用JaNtes的資料交換' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'JukiExchangeDBActive'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Juki資料交換伺服器' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'JukiExchangeDBServerName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Juki資料交換資料庫名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'JukiExchangeDBDataBaseName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Juki資料交換資料庫帳號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'JukiExchangeDBLoginID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Juki資料交換資料庫密碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System', @level2type=N'COLUMN',@level2name=N'JukiExchangeDBLoginPWD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統參數檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'System'
GO
