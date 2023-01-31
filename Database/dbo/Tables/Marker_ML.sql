CREATE TABLE [dbo].[Marker_ML] (
    [ID]                   VARCHAR (10)    CONSTRAINT [DF_Marker_ML_ID] DEFAULT ('') NOT NULL,
    [Version]              VARCHAR (3)     CONSTRAINT [DF_Marker_ML_Version] DEFAULT ('') NOT NULL,
    [MarkerUkey]           BIGINT          CONSTRAINT [DF_Marker_ML_MarkerUkey] DEFAULT ((0)) NOT NULL,
    [MarkerName]           VARCHAR (10)    CONSTRAINT [DF_Marker_ML_MarkerName] DEFAULT ('') NOT NULL,
    [FabricPanelCode]      VARCHAR (20)    CONSTRAINT [DF_Marker_ML_FabricPanelCode] DEFAULT ('') NOT NULL,
    [SCIRefno]             VARCHAR (30)    CONSTRAINT [DF_Marker_ML_SCIRefno] DEFAULT ('') NOT NULL,
    [MarkerLength]         VARCHAR (15)    CONSTRAINT [DF_Marker_ML_MarkerLength] DEFAULT ('') NOT NULL,
    [PatternPanel]         VARCHAR (MAX)   CONSTRAINT [DF_Marker_ML_PatternPanel] DEFAULT ('') NOT NULL,
    [FabricCode]           VARCHAR (3)     CONSTRAINT [DF_Marker_ML_FabricCode] DEFAULT ('') NOT NULL,
    [Width]                VARCHAR (6)     CONSTRAINT [DF_Marker_ML_Width] DEFAULT ('') NOT NULL,
    [Efficiency]           VARCHAR (9)     CONSTRAINT [DF_Marker_ML_Efficiency] DEFAULT ('') NOT NULL,
    [Remark]               VARCHAR (30)    CONSTRAINT [DF_Marker_ML_Remark] DEFAULT ('') NOT NULL,
    [ConsPC]               VARCHAR (12)    CONSTRAINT [DF_Marker_ML_ConsPC] DEFAULT ('') NOT NULL,
    [Article]              NVARCHAR (MAX)  CONSTRAINT [DF_Marker_ML_Article] DEFAULT ('') NOT NULL,
    [ActCuttingPerimeter]  NVARCHAR (30)   CONSTRAINT [DF_Marker_ML_ActCuttingPerimeter] DEFAULT ('') NOT NULL,
    [Perimeter]            NVARCHAR (30)   CONSTRAINT [DF_Marker_ML_Perimeter] DEFAULT ('') NOT NULL,
    [StraightLength]       NVARCHAR (30)   CONSTRAINT [DF_Marker_ML_StraightLength] DEFAULT ('') NOT NULL,
    [CurvedLength]         NVARCHAR (30)   CONSTRAINT [DF_Marker_ML_CurvedLength] DEFAULT ('') NOT NULL,
    [TotalCuttingPieceNum] NVARCHAR (30)   CONSTRAINT [DF_Marker_ML_TotalCuttingPieceNum] DEFAULT ('') NOT NULL,
    [AllSize]              BIT             CONSTRAINT [DF_Marker_ML_AllSize] DEFAULT ((0)) NOT NULL,
    [OneTwoWay]            BIT             CONSTRAINT [DF_Marker_ML_OneTwoWay] DEFAULT ((0)) NOT NULL,
    [V_Repeat]             NUMERIC (12, 4) CONSTRAINT [DF_Marker_ML_V_Repeat] DEFAULT ((0)) NOT NULL,
    [H_Repeat]             NUMERIC (12, 4) CONSTRAINT [DF_Marker_ML_H_Repeat] DEFAULT ((0)) NOT NULL,
    [MatchFabric]          VARCHAR (1)     CONSTRAINT [DF_Marker_ML_MatchFabric] DEFAULT ('') NOT NULL,
    [HorizontalCutting]    BIT             CONSTRAINT [DF_Marker_ML_HorizontalCutting] DEFAULT ((0)) NOT NULL,
    [UKey_Old]             VARCHAR (10)    CONSTRAINT [DF_Marker_ML_UKey_Old] DEFAULT ('') NOT NULL,
    [Mtl_Key]              VARCHAR (10)    CONSTRAINT [DF_Marker_ML_Mtl_Key] DEFAULT ('') NOT NULL,
    [Mtl_Ver]              VARCHAR (2)     CONSTRAINT [DF_Marker_ML_Mtl_Ver] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Marker_ML] PRIMARY KEY CLUSTERED ([ID] ASC, [Version] ASC, [MarkerName] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKEY_OLD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'UKey_Old';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'橫裁', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'HorizontalCutting';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'循環', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'MatchFabric';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'H_Repeat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'H_Repeat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'V_Repeat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'V_Repeat';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排版方向', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'OneTwoWay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全段尺碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'AllSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片總數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'TotalCuttingPieceNum';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'曲線總長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'CurvedLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'直線總長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'StraightLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'周長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'Perimeter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁切周長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'ActCuttingPerimeter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'Efficiency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pattern Panel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'MarkerLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LectraCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結 Marker', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'MarkerUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMNotice ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML', @level2type = N'COLUMN', @level2name = N'ID';

