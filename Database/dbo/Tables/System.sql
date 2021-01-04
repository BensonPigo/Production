CREATE TABLE [dbo].[System] (
    [Mailserver]                 VARCHAR (60)   CONSTRAINT [DF_System_Mailserver] DEFAULT ('') NULL,
    [Sendfrom]                   VARCHAR (40)   CONSTRAINT [DF_System_Sendfrom] DEFAULT ('') NULL,
    [EmailID]                    VARCHAR (40)   CONSTRAINT [DF_System_Emailid] DEFAULT ('') NULL,
    [EmailPwd]                   VARCHAR (20)   CONSTRAINT [DF_System_EmailPwd] DEFAULT ('') NULL,
    [PicPath]                    VARCHAR (80)   CONSTRAINT [DF_System_PicPath] DEFAULT ('') NULL,
    [StdTMS]                     INT            CONSTRAINT [DF_System_StdTMS] DEFAULT ((0)) NOT NULL,
    [ClipPath]                   VARCHAR (80)   CONSTRAINT [DF_System_ClipPath] DEFAULT ('') NULL,
    [FtpIP]                      VARCHAR (36)   CONSTRAINT [DF_System_FtpIP] DEFAULT ('') NULL,
    [FtpID]                      VARCHAR (10)   CONSTRAINT [DF_System_FtpID] DEFAULT ('') NULL,
    [FtpPwd]                     VARCHAR (36)   CONSTRAINT [DF_System_FtpPwd] DEFAULT ('') NULL,
    [SewLock]                    DATE           NULL,
    [SampleRate]                 TINYINT        CONSTRAINT [DF_System_SPLRate] DEFAULT ((0)) NULL,
    [PullLock]                   DATE           NULL,
    [RgCode]                     VARCHAR (3)    CONSTRAINT [DF_System_RgCode] DEFAULT ('') NOT NULL,
    [ImportDataPath]             VARCHAR (60)   CONSTRAINT [DF_System_UdsPath] DEFAULT ('') NULL,
    [ImportDataFileName]         VARCHAR (60)   CONSTRAINT [DF_System_UdFileName] DEFAULT ('') NULL,
    [ExportDataPath]             VARCHAR (60)   CONSTRAINT [DF_System_DnsPath] DEFAULT ('') NULL,
    [CurrencyID]                 VARCHAR (4)    CONSTRAINT [DF_System_CurrencyID] DEFAULT ('') NULL,
    [USDRate]                    NUMERIC (9, 4) CONSTRAINT [DF_System_USDRate] DEFAULT ((0)) NULL,
    [POApproveName]              VARCHAR (10)   CONSTRAINT [DF_System_POApproveName] DEFAULT ('') NULL,
    [POApproveDay]               TINYINT        CONSTRAINT [DF_System_POApproveDay] DEFAULT ((0)) NULL,
    [CutDay]                     TINYINT        CONSTRAINT [DF_System_CutDay] DEFAULT ((0)) NULL,
    [AccountKeyword]             VARCHAR (1)    CONSTRAINT [DF_System_AccountKeyword] DEFAULT ('') NULL,
    [ReadyDay]                   TINYINT        CONSTRAINT [DF_System_ReadyDay] DEFAULT ((0)) NULL,
    [VNMultiple]                 NUMERIC (4, 2) CONSTRAINT [DF_System_VNMultiple] DEFAULT ((0)) NULL,
    [MtlLeadTime]                TINYINT        CONSTRAINT [DF_System_MtlLeadTime] DEFAULT ((0)) NULL,
    [ExchangeID]                 VARCHAR (2)    DEFAULT ('') NULL,
    [RFIDServerName]             VARCHAR (20)   NULL,
    [RFIDDatabaseName]           VARCHAR (20)   NULL,
    [RFIDLoginId]                VARCHAR (20)   NULL,
    [RFIDLoginPwd]               VARCHAR (20)   NULL,
    [RFIDTable]                  VARCHAR (20)   NULL,
    [ProphetSingleSizeDeduct]    NUMERIC (3)    NULL,
    [PrintingSuppID]             VARCHAR (8)    NULL,
    [QCMachineDelayTime]         NUMERIC (2, 1) NULL,
    [APSLoginId]                 VARCHAR (15)   CONSTRAINT [DF_System_APSLoginId] DEFAULT ('') NULL,
    [APSLoginPwd]                VARCHAR (15)   CONSTRAINT [DF_System_APSLoginPwd] DEFAULT ('') NULL,
    [SQLServerName]              VARCHAR (130)  CONSTRAINT [DF_System_SQLServerName] DEFAULT ('') NULL,
    [APSDatabaseName]            VARCHAR (15)   CONSTRAINT [DF_System_APSDatabaseName] DEFAULT ('') NULL,
    [RFIDMiddlewareInRFIDServer] BIT            DEFAULT ((0)) NOT NULL,
    [UseAutoScanPack]            BIT            CONSTRAINT [DF_System_UseAutoScanPack] DEFAULT ((0)) NOT NULL,
    [MtlAutoLock]                BIT            CONSTRAINT [DF_System_MtlAutoLock] DEFAULT ((0)) NOT NULL,
    [InspAutoLockAcc]            BIT            DEFAULT ((0)) NOT NULL,
    [ShippingMarkPath]           VARCHAR (80)   NULL,
    [StyleSketch]                VARCHAR (80)   NULL,
    [ARKServerName]              VARCHAR (20)   NULL,
    [ARKDatabaseName]            VARCHAR (20)   NULL,
    [ARKLoginId]                 VARCHAR (20)   NULL,
    [ARKLoginPwd]                VARCHAR (20)   NULL,
    [MarkerInputPath]            NVARCHAR (80)  NULL,
    [MarkerOutputPath]           NVARCHAR (80)  NULL,
    [ReplacementReport]          VARCHAR (80)   NULL,
    [CuttingP10mustCutRef]       BIT            DEFAULT ((0)) NOT NULL,
    [Automation]                 BIT            DEFAULT ((0)) NOT NULL,
    [AutomationAutoRunTime]      TINYINT        CONSTRAINT [DF_System_AutomationAutoRunTime] DEFAULT ((0)) NOT NULL,
    [CanReviseDailyLockData]     BIT            DEFAULT ((0)) NOT NULL,
    [AutoGenerateByTone]         BIT            CONSTRAINT [DF_System_AutoGenerateByTone] DEFAULT ((0)) NOT NULL,
    [MiscPOApproveName]          VARCHAR (10)   CONSTRAINT [DF_System_MiscPOApproveName] DEFAULT ('') NULL,
    [MiscPOApproveDay]           TINYINT        CONSTRAINT [DF_System_MiscPOApproveDay] DEFAULT ((0)) NULL,
    [QMSAutoAdjustMtl]           BIT            DEFAULT ((0)) NOT NULL,
    [ShippingMarkTemplatePath]   VARCHAR (80)   CONSTRAINT [DF_System_ShippingMarkPath] DEFAULT ('') NOT NULL,
    [WIP_ByShell]                BIT            CONSTRAINT [DF_System_WIP_FollowCutOutput] DEFAULT ((0)) NOT NULL,
    [NoRestrictOrdersDelivery]   BIT            CONSTRAINT [DF_System_NoRestrictOrdersDelivery] DEFAULT ((0)) NOT NULL,
    [RFCardEraseBeforePrinting]  BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RgCode] PRIMARY KEY CLUSTERED ([RgCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Misc Local Purchase Approve Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'MiscPOApproveName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Misc Local Purchase �۰�Approve���Ѽ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'MiscPOApproveDay';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄件者名稱(有@)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'Sendfrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Email ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = 'EmailID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Email Password', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'EmailPwd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Picture Path', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'PicPath';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CPU和TMS換算值(1400)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'StdTMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'夾檔存放位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'ClipPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FTP IP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'FtpIP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FTP ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'FtpID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FTP PASSWORD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'FtpPwd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫日報表月結日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'SewLock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷樣單的倍數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = 'SampleRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Report月結日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'PullLock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'區域代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'RgCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料上傳位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = 'ImportDataPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料上傳檔名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = 'ImportDataFileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料下載位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = 'ExportDataPath';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'美金匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'USDRate';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Purchase Approve Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'POApproveName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Purchase 自動Approve的天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'POApproveDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪上線日為Sewing Inline的前幾日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'CutDay';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號保留字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'AccountKeyword';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production Ready day', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'ReadyDay';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'VN出口報關調整營收倍數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'VNMultiple';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Material Leat-time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'MtlLeadTime';


GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統參數檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mail Server IP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'Mailserver';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Rate Type',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'System',
    @level2type = N'COLUMN',
    @level2name = N'ExchangeID'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'驗布機延遲時間(秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'QCMachineDelayTime';



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QMS����۰ʽվ����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'QMSAutoAdjustMtl';



Go


	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���ҽd���ɸ�|', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'ShippingMarkTemplatePath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'RF Card Erase Before Printing', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'RFCardEraseBeforePrinting';


EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'RF Card Erase Before Printing',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'System',
    @level2type = N'COLUMN',
    @level2name = N'RFCardEraseBeforePrinting'