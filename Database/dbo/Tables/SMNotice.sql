CREATE TABLE [dbo].[SMNotice] (
    [ID]            VARCHAR (10) CONSTRAINT [DF_SMNotice_ID] DEFAULT ('') NOT NULL,
    [MainID]        VARCHAR (10) CONSTRAINT [DF_SMNotice_MainID] DEFAULT ('') NULL,
    [Mr]            VARCHAR (10) CONSTRAINT [DF_SMNotice_Mr] DEFAULT ('') NOT NULL,
    [SMR]           VARCHAR (10) CONSTRAINT [DF_SMNotice_SMR] DEFAULT ('') NOT NULL,
    [BrandID]       VARCHAR (8)  CONSTRAINT [DF_SMNotice_BrandID] DEFAULT ('') NOT NULL,
    [StyleID]       VARCHAR (15) CONSTRAINT [DF_SMNotice_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]      VARCHAR (10) CONSTRAINT [DF_SMNotice_SeasonID] DEFAULT ('') NOT NULL,
    [StyleUkey]     VARCHAR (10) CONSTRAINT [DF_SMNotice_StyleUkey] DEFAULT ('') NULL,
    [CountryID]     VARCHAR (2)  CONSTRAINT [DF_SMNotice_CountryID] DEFAULT ('') NOT NULL,
    [PatternNo]     VARCHAR (10) CONSTRAINT [DF_SMNotice_PatternNo] DEFAULT ('') NULL,
    [OldStyleID]    VARCHAR (15) CONSTRAINT [DF_SMNotice_OldStyleID] DEFAULT ('') NULL,
    [OldSeasonID]   VARCHAR (10) CONSTRAINT [DF_SMNotice_OldSeasonID] DEFAULT ('') NULL,
    [SizeGroup]     VARCHAR (1)  CONSTRAINT [DF_SMNotice_SizeGroup] DEFAULT ('') NOT NULL,
    [SizeCode]      VARCHAR (8)  CONSTRAINT [DF_SMNotice_SizeCode] DEFAULT ('') NOT NULL,
    [BuyReady]      DATE         NULL,
    [Status]        VARCHAR (1)  CONSTRAINT [DF_SMNotice_Status] DEFAULT ('') NULL,
    [StatusPattern] VARCHAR (1)  CONSTRAINT [DF_SMNotice_StatusPattern] DEFAULT ('') NULL,
    [StatusIE]      VARCHAR (1)  CONSTRAINT [DF_SMNotice_StatusIE] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_SMNotice_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_SMNotice_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_SMNotice] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'樣衣製作申請', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請單號母單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'MainID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'Mr';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬部門/組別(帶組長Id)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'SMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'記錄Style Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'區域別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'PatternNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'變更款號後記錄舊款式ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'OldStyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'變更款號後記錄舊款式季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'OldSeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客群', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'SizeGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用量Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Buy Ready Day', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'BuyReady';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號狀態(New/Approve/Complete/ReVise/Junk)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern/Marker申請狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'StatusPattern';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE申請狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'StatusIE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SMNotice', @level2type = N'COLUMN', @level2name = N'EditDate';

