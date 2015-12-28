CREATE TABLE [dbo].[System] (
    [Terminal]            VARCHAR (MAX)  CONSTRAINT [DF_System_Terminal] DEFAULT ('') NULL,
    [ITerminal]           VARCHAR (MAX)  CONSTRAINT [DF_System_ITerminal] DEFAULT ('') NULL,
    [Mailserver]          VARCHAR (60)   CONSTRAINT [DF_System_Mailserver] DEFAULT ('') NULL,
    [Sendfrom]            VARCHAR (40)   CONSTRAINT [DF_System_Sendfrom] DEFAULT ('') NULL,
    [Emailid]             VARCHAR (40)   CONSTRAINT [DF_System_Emailid] DEFAULT ('') NULL,
    [EmailPwd]            VARCHAR (20)   CONSTRAINT [DF_System_EmailPwd] DEFAULT ('') NULL,
    [PicPath]             VARCHAR (80)   CONSTRAINT [DF_System_PicPath] DEFAULT ('') NULL,
    [AccPath]             VARCHAR (80)   CONSTRAINT [DF_System_AccPath] DEFAULT ('') NULL,
    [MMSPath]             VARCHAR (80)   CONSTRAINT [DF_System_MMSPath] DEFAULT ('') NULL,
    [SFCPath]             VARCHAR (80)   CONSTRAINT [DF_System_SFCPath] DEFAULT ('') NULL,
    [StdTMS]              INT            CONSTRAINT [DF_System_StdTMS] DEFAULT ((0)) NOT NULL,
    [ClipPath]            VARCHAR (80)   CONSTRAINT [DF_System_ClipPath] DEFAULT ('') NULL,
    [FtpIP]               VARCHAR (36)   CONSTRAINT [DF_System_FtpIP] DEFAULT ('') NULL,
    [FtpID]               VARCHAR (10)   CONSTRAINT [DF_System_FtpID] DEFAULT ('') NULL,
    [FtpPwd]              VARCHAR (36)   CONSTRAINT [DF_System_FtpPwd] DEFAULT ('') NULL,
    [SewLock]             DATE           NULL,
    [SPLRate]             TINYINT        CONSTRAINT [DF_System_SPLRate] DEFAULT ((0)) NOT NULL,
    [PullLock]            DATE           NULL,
    [RgCode]              VARCHAR (3)    CONSTRAINT [DF_System_RgCode] DEFAULT ('') NULL,
    [UdsPath]             VARCHAR (60)   CONSTRAINT [DF_System_UdsPath] DEFAULT ('') NULL,
    [UdFileName]          VARCHAR (60)   CONSTRAINT [DF_System_UdFileName] DEFAULT ('') NULL,
    [DnsPath]             VARCHAR (60)   CONSTRAINT [DF_System_DnsPath] DEFAULT ('') NULL,
    [SendBack]            BIT            CONSTRAINT [DF_System_SendBack] DEFAULT ((0)) NULL,
    [CurrencyID]          VARCHAR (4)    CONSTRAINT [DF_System_CurrencyID] DEFAULT ('') NULL,
    [USDRate]             NUMERIC (9, 4) CONSTRAINT [DF_System_USDRate] DEFAULT ((0)) NULL,
    [VATRate]             NUMERIC (3, 1) CONSTRAINT [DF_System_VATRate] DEFAULT ((0)) NULL,
    [Bond]                VARCHAR (1)    CONSTRAINT [DF_System_Bond] DEFAULT ('') NULL,
    [POApproveName]       VARCHAR (10)   CONSTRAINT [DF_System_POApproveName] DEFAULT ('') NULL,
    [POApproveDay]        TINYINT        CONSTRAINT [DF_System_POApproveDay] DEFAULT ((0)) NULL,
    [CutDay]              TINYINT        CONSTRAINT [DF_System_CutDay] DEFAULT ((0)) NULL,
    [DailyUpdateSendMail] BIT            CONSTRAINT [DF_System_DailyUpdateSendMail] DEFAULT ((0)) NULL,
    [ExchangeId]          VARCHAR (2)    CONSTRAINT [DF_System_ExchangeId] DEFAULT ('') NULL,
    [AccountKeyword]      VARCHAR (1)    CONSTRAINT [DF_System_AccountKeyword] DEFAULT ('') NULL,
    [ReadyDay]            TINYINT        CONSTRAINT [DF_System_ReadyDay] DEFAULT ((0)) NULL,
    [PopOut]              SMALLINT       CONSTRAINT [DF_System_PopOut] DEFAULT ((0)) NULL,
    [SubConBCS]           VARCHAR (4)    CONSTRAINT [DF_System_SubConBCS] DEFAULT ('') NULL,
    [StdFarmInDay]        TINYINT        CONSTRAINT [DF_System_StdFarmInDay] DEFAULT ((0)) NULL,
    [VNMultiple]          NUMERIC (4, 2) CONSTRAINT [DF_System_VNMultiple] DEFAULT ((0)) NULL,
    [MtlLeadTime]         TINYINT        CONSTRAINT [DF_System_MtlLeadTime] DEFAULT ((0)) NULL
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄件者名稱(有@)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'Sendfrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Email ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'Emailid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Email Password', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'EmailPwd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Picture Path', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'PicPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Account data Path', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'AccPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MMS Data Path', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'MMSPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SFC Data Path', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'SFCPath';


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'銷樣單的倍數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'SPLRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Report月結日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'PullLock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'區域代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'RgCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料上傳位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'UdsPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料上傳檔名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'UdFileName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料下載位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'DnsPath';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否回傳PMS資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'SendBack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'美金匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'USDRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'VATRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'保證金', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'Bond';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Purchase Approve Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'POApproveName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Purchase 自動Approve的天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'POApproveDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪上線日為Sewing Inline的前幾日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'CutDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Send an e-mail from update', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'DailyUpdateSendMail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'匯率使用Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'ExchangeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳號保留字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'AccountKeyword';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production Ready day', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'ReadyDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'設定Pop Out的Timer分鐘數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'PopOut';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sub Process BCS Report時間切點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'SubConBCS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sub Process BCS Report Farm In標準天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'StdFarmInDay';


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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'遠端主機列表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'Terminal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'內部遠端主機列表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'ITerminal';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mail Server IP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'System', @level2type = N'COLUMN', @level2name = N'Mailserver';

