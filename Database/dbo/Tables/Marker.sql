CREATE TABLE [dbo].[Marker] (
    [ID]                  VARCHAR (10)   CONSTRAINT [DF_Marker_ID] DEFAULT ('') NOT NULL,
    [Version]             VARCHAR (3)    CONSTRAINT [DF_Marker_Version] DEFAULT ('') NOT NULL,
    [BrandID]             VARCHAR (8)    CONSTRAINT [DF_Marker_BrandID] DEFAULT ('') NOT NULL,
    [StyleID]             VARCHAR (15)   CONSTRAINT [DF_Marker_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]            VARCHAR (10)   CONSTRAINT [DF_Marker_SeasonID] DEFAULT ('') NOT NULL,
    [MarkerNo]            VARCHAR (10)   CONSTRAINT [DF_Marker_MarkerNo] DEFAULT ('') NOT NULL,
    [ActFtyMarker]        VARCHAR (8)    CONSTRAINT [DF_Marker_ActFtyMarker] DEFAULT ('') NOT NULL,
    [PatternID]           VARCHAR (10)   CONSTRAINT [DF_Marker_PatternID] DEFAULT ('') NOT NULL,
    [PatternNo]           VARCHAR (10)   CONSTRAINT [DF_Marker_PatternNo] DEFAULT ('') NOT NULL,
    [PatternVersion]      VARCHAR (3)    CONSTRAINT [DF_Marker_PatternVersion] DEFAULT ('') NOT NULL,
    [MarkerName]          VARCHAR (10)   CONSTRAINT [DF_Marker_MarkerName] DEFAULT ('') NOT NULL,
    [MarkerFormat]        VARCHAR (3)    CONSTRAINT [DF_Marker_MarkerFormat] DEFAULT ('') NOT NULL,
    [EstFinDate]          DATE           NULL,
    [ActFinDate]          DATETIME       NULL,
    [PLUS]                NUMERIC (3)    CONSTRAINT [DF_Marker_PLUS] DEFAULT ((0)) NOT NULL,
    [RevisedReason]       VARCHAR (4)    CONSTRAINT [DF_Marker_RevisedReason] DEFAULT ('') NOT NULL,
    [Status]              VARCHAR (15)   CONSTRAINT [DF_Marker_Status] DEFAULT ('') NOT NULL,
    [CFMName]             VARCHAR (10)   CONSTRAINT [DF_Marker_CFMName] DEFAULT ('') NOT NULL,
    [UKey]                BIGINT         NOT NULL,
    [StyleRemark]         NVARCHAR (MAX) CONSTRAINT [DF_Marker_StyleRemark] DEFAULT ('') NOT NULL,
    [HisRemark]           NVARCHAR (MAX) CONSTRAINT [DF_Marker_HisRemark] DEFAULT ('') NOT NULL,
    [PendingRemark]       NVARCHAR (MAX) CONSTRAINT [DF_Marker_PendingRemark] DEFAULT ('') NOT NULL,
    [StyleUkey]           BIGINT         CONSTRAINT [DF_Marker_StyleUkey] DEFAULT ((0)) NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Marker_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Marker_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [KeepPreviousVersion] BIT            CONSTRAINT [DF_Marker_KeepPreviousVersion] DEFAULT ((0)) NOT NULL,
    [SizeReason]          VARCHAR (5)    CONSTRAINT [DF_Marker_SizeReason] DEFAULT ('') NOT NULL,
    [MarkerNoLoss]        BIT            CONSTRAINT [DF_Marker_MarkerNoLoss] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Marker] PRIMARY KEY CLUSTERED ([ID] ASC, [Version] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Keep Previous Version', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'KeepPreviousVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'StyleUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'待確認備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'PendingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷史備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'HisRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'給款示的備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'StyleRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'UKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'CFMName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'改版原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'RevisedReason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Plus %', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'PLUS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際完成日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'ActFinDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克預計完成日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'EstFinDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克檔格式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'MarkerFormat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'PatternVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版版號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'PatternNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'PatternID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際馬克工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'ActFtyMarker';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'MarkerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Season', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker', @level2type = N'COLUMN', @level2name = N'ID';

