CREATE TABLE [dbo].[FIR_Laboratory] (
    [ID]                BIGINT        CONSTRAINT [DF_FIR_Laboratory_ID] DEFAULT ((0)) NOT NULL,
    [POID]              VARCHAR (13)  CONSTRAINT [DF_FIR_Laboratory_POID] DEFAULT ('') NOT NULL,
    [SEQ1]              VARCHAR (3)   CONSTRAINT [DF_FIR_Laboratory_SEQ] DEFAULT ('') NOT NULL,
    [SEQ2]              VARCHAR (2)   CONSTRAINT [DF_FIR_Laboratory_SEQ2] DEFAULT ('') NOT NULL,
    [InspDeadline]      DATE          NULL,
    [Crocking]          VARCHAR (5)   CONSTRAINT [DF_FIR_Laboratory_Crocking] DEFAULT ('') NULL,
    [Heat]              VARCHAR (5)   CONSTRAINT [DF_FIR_Laboratory_Heat] DEFAULT ('') NULL,
    [Wash]              VARCHAR (5)   CONSTRAINT [DF_FIR_Laboratory_Wash] DEFAULT ('') NULL,
    [CrockingDate]      DATE          NULL,
    [HeatDate]          DATE          NULL,
    [WashDate]          DATE          NULL,
    [CrockingRemark]    VARCHAR (100) CONSTRAINT [DF_FIR_Laboratory_CrockingRemark] DEFAULT ('') NULL,
    [HeatRemark]        VARCHAR (100) CONSTRAINT [DF_FIR_Laboratory_HeatRemark] DEFAULT ('') NULL,
    [WashRemark]        VARCHAR (100) CONSTRAINT [DF_FIR_Laboratory_WashRemark] DEFAULT ('') NULL,
    [ReceiveSampleDate] DATE          NULL,
    [Result]            VARCHAR (5)   CONSTRAINT [DF_FIR_Laboratory_Result] DEFAULT ('') NULL,
    [nonCrocking]       BIT           CONSTRAINT [DF_FIR_Laboratory_nonCrocking] DEFAULT ((0)) NULL,
    [nonHeat]           BIT           CONSTRAINT [DF_FIR_Laboratory_nonHeat] DEFAULT ((0)) NULL,
    [nonWash]           BIT           CONSTRAINT [DF_FIR_Laboratory_nonWash] DEFAULT ((0)) NULL,
    [CrockingEncode]    BIT           CONSTRAINT [DF_FIR_Laboratory_CrockingEncode] DEFAULT ((0)) NULL,
    [HeatEncode]        BIT           CONSTRAINT [DF_FIR_Laboratory_HeatEncode] DEFAULT ((0)) NULL,
    [WashEncode]        BIT           CONSTRAINT [DF_FIR_Laboratory_WashEncode] DEFAULT ((0)) NULL,
    [SkewnessOptionID]  VARCHAR (1)   CONSTRAINT [DF_FIR_Laboratory_SkewnessOptionID] DEFAULT ((1)) NULL,
    [CrockingInspector] VARCHAR (10)  CONSTRAINT [DF_FIR_Laboratory_CrockingInspector] DEFAULT ('') NOT NULL,
    [HeatInspector]     VARCHAR (10)  CONSTRAINT [DF_FIR_Laboratory_HeatInspector] DEFAULT ('') NOT NULL,
    [WashInspector]     VARCHAR (10)  CONSTRAINT [DF_FIR_Laboratory_WashInspector] DEFAULT ('') NOT NULL,
	ReportNo varchar(14) not null CONSTRAINT [DF_FIR_Laboratory_ReportNo] default '',
	Iron varchar(5) not null CONSTRAINT [DF_FIR_Laboratory_Iron] DEFAULT '',
	IronDate DATE NULL,
	IronRemark nvarchar(100) not null CONSTRAINT [DF_FIR_Laboratory_IronRemark] DEFAULT '',
	nonIron bit not null CONSTRAINT [DF_FIR_Laboratory_nonIron] DEFAULT 0,
	IronEncode bit not null CONSTRAINT [DF_FIR_Laboratory_IronEncode] DEFAULT 0,
	IronInspector varchar(10) not null CONSTRAINT [DF_FIR_Laboratory_IronInspector] DEFAULT '',
	CrockingReceiveDate DATE NULL,
	CrockingApprover varchar(10) NOT NULL CONSTRAINT [DF_FIR_Laboratory_CrockingApprover] DEFAULT '',
	HeatReceiveDate DATE NULL,
	HeatApprover varchar(10) NOT NULL CONSTRAINT [DF_FIR_Laboratory_HeatApprover] DEFAULT '',
	IronReceiveDate DATE NULL,
	IronApprover varchar(10) NOT NULL CONSTRAINT [DF_FIR_Laboratory_IronApprover] DEFAULT '',
	WashReceiveDate DATE NULL,
	WashApprover  varchar(10) NOT NULL CONSTRAINT [DF_FIR_Laboratory_WashApprover] DEFAULT ''
    CONSTRAINT [PK_FIR_Laboratory] PRIMARY KEY CLUSTERED ([ID] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Laboratory Crocking & shrinkage Test', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'POID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗截止日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'InspDeadline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色脫落結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Crocking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'熱壓縮律結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Heat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗縮律結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Wash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色脫落測試 日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'CrockingDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'熱縮律測試日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'HeatDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗縮律測試日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色脫落備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'CrockingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'熱縮律測試備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'HeatRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗縮律測是備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'樣品收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'ReceiveSampleDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'免測試色脫落', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'nonCrocking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'免測試熱縮', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'nonHeat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'免測試水洗縮', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'nonWash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色脫落確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'CrockingEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'熱縮確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'HeatEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
CREATE NONCLUSTERED INDEX [ID_FIR_Laboratory_POID_SEQ]
    ON [dbo].[FIR_Laboratory]([POID] ASC, [SEQ1] ASC, [SEQ2] ASC);


GO