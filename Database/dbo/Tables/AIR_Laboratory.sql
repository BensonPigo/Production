CREATE TABLE [dbo].[AIR_Laboratory] (
    [ID]            BIGINT          CONSTRAINT [DF_AIR_Laboratory_ID] DEFAULT ((0)) NOT NULL,
    [POID]          VARCHAR (13)    CONSTRAINT [DF_AIR_Laboratory_POID] DEFAULT ('') NOT NULL,
    [SEQ1]          VARCHAR (3)     CONSTRAINT [DF_AIR_Laboratory_SEQ] DEFAULT ('') NOT NULL,
    [SEQ2]          VARCHAR (2)     CONSTRAINT [DF_AIR_Laboratory_SEQ2] DEFAULT ('') NOT NULL,
    [Qty]           NUMERIC (10, 2) CONSTRAINT [DF_AIR_Laboratory_Qty] DEFAULT ((0)) NULL,
    [Result]        VARCHAR (5)     CONSTRAINT [DF_AIR_Laboratory_Result] DEFAULT ('') NULL,
    [InspDeadLine]  DATE            NULL,
    [NonOven]       BIT             CONSTRAINT [DF_AIR_Laboratory_NonOven] DEFAULT ((0)) NULL,
    [Oven]          VARCHAR (5)     CONSTRAINT [DF_AIR_Laboratory_Oven] DEFAULT ('') NULL,
    [OvenEncode]    BIT             CONSTRAINT [DF_AIR_Laboratory_OvenEncode] DEFAULT ((0)) NULL,
    [OvenDate]      DATE            NULL,
    [OvenScale]     VARCHAR (5)     CONSTRAINT [DF_AIR_Laboratory_OvenScale] DEFAULT ('') NULL,
    [OvenInspector] VARCHAR (10)    CONSTRAINT [DF_AIR_Laboratory_OvenInspector] DEFAULT ('') NULL,
    [OvenRemark]    VARCHAR (60)    CONSTRAINT [DF_AIR_Laboratory_OvenRemark] DEFAULT ('') NULL,
    [NonWash]       BIT             CONSTRAINT [DF_AIR_Laboratory_NonWash] DEFAULT ((0)) NULL,
    [Wash]          VARCHAR (5)     CONSTRAINT [DF_AIR_Laboratory_Wash] DEFAULT ('') NULL,
    [WashEncode]    BIT             CONSTRAINT [DF_AIR_Laboratory_WashEncode] DEFAULT ((0)) NULL,
    [WashDate]      DATE            NULL,
    [WashScale]     VARCHAR (5)     CONSTRAINT [DF_AIR_Laboratory_WashScale] DEFAULT ('') NULL,
    [WashInspector] VARCHAR (10)    CONSTRAINT [DF_AIR_Laboratory_WashInspector] DEFAULT ('') NULL,
    [WashRemark]    VARCHAR (60)    CONSTRAINT [DF_AIR_Laboratory_WashRemark] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_AIR_Laboratory_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_AIR_Laboratory_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME        NULL,
	ReportNo varchar(14) not null CONSTRAINT [DF_AIR_Laboratory_ReportNo] default '',
    [OvenApprover] VARCHAR(10)               CONSTRAINT [DF_AIR_Laboratory_OvenApprover]     NOT NULL DEFAULT (''), 
    [WashApprover] VARCHAR(10)               CONSTRAINT [DF_AIR_Laboratory_WashApprover]     NOT NULL DEFAULT (''), 
    [WashingFastnessApprover] VARCHAR(10)    CONSTRAINT [DF_AIR_Laboratory_WashingFastnessApprover]     NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_AIR_Laboratory] PRIMARY KEY CLUSTERED ([ID] ASC, [POID] ASC, [SEQ1] ASC, [SEQ2] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'OvenEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'OvenDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱灰階', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'OvenScale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'OvenInspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'OvenRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不檢驗水洗', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'NonWash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Wash';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗Encode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashEncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗灰階', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashScale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashInspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'WashRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Accessory Laboratory- Oven & Wash', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'POID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗截止日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'InspDeadLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不檢驗烘箱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'NonOven';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'Oven';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_Laboratory', @level2type = N'COLUMN', @level2name = N'SEQ1';

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'OvenApprover',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AIR_Laboratory',
    @level2type = N'COLUMN',
    @level2name = N'OvenApprover'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WashApprover',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AIR_Laboratory',
    @level2type = N'COLUMN',
    @level2name = N'WashApprover'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WashingFastnessApprover',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AIR_Laboratory',
    @level2type = N'COLUMN',
    @level2name = N'WashingFastnessApprover'