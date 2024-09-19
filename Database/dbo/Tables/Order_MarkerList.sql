CREATE TABLE [dbo].[Order_MarkerList] (
    [Id]                  VARCHAR (13)   CONSTRAINT [DF_Order_MarkerList_Id] DEFAULT ('') NOT NULL,
    [Ukey]                BIGINT         CONSTRAINT [DF_Order_MarkerList_Ukey] DEFAULT ((0)) NOT NULL,
    [Seq]                 NUMERIC (4)    CONSTRAINT [DF_Order_MarkerList_Seq] DEFAULT ((0)) NOT NULL,
    [MarkerName]          NVARCHAR (20)  CONSTRAINT [DF_Order_MarkerList_MarkerName] DEFAULT ('') NOT NULL,
    [FabricCode]          VARCHAR (3)    CONSTRAINT [DF_Order_MarkerList_FabricCode] DEFAULT ('') NOT NULL,
    [FabricCombo]         VARCHAR (2)    CONSTRAINT [DF_Order_MarkerList_FabricCombo] DEFAULT ('') NOT NULL,
    [FabricPanelCode]     VARCHAR (2)    CONSTRAINT [DF_Order_MarkerList_FabricPanelCode] DEFAULT ('') NOT NULL,
    [isQT]                BIT            CONSTRAINT [DF_Order_MarkerList_isQT] DEFAULT ((0)) NOT NULL,
    [MarkerLength]        VARCHAR (15)   CONSTRAINT [DF_Order_MarkerList_MarkerLength] DEFAULT ('') NOT NULL,
    [ConsPC]              NUMERIC (8, 4) CONSTRAINT [DF_Order_MarkerList_ConsPC] DEFAULT ((0)) NOT NULL,
    [Cuttingpiece]        BIT            CONSTRAINT [DF_Order_MarkerList_Cuttingpiece] DEFAULT ((0)) NOT NULL,
    [ActCuttingPerimeter] NVARCHAR (15)  CONSTRAINT [DF_Order_MarkerList_ActCuttingPerimeter] DEFAULT ('') NOT NULL,
    [StraightLength]      VARCHAR (15)   CONSTRAINT [DF_Order_MarkerList_StraightLength] DEFAULT ('') NOT NULL,
    [CurvedLength]        VARCHAR (15)   CONSTRAINT [DF_Order_MarkerList_CurvedLength] DEFAULT ('') NOT NULL,
    [Efficiency]          VARCHAR (9)    CONSTRAINT [DF_Order_MarkerList_Efficiency] DEFAULT ('') NOT NULL,
    [Remark]              NVARCHAR (250) CONSTRAINT [DF_Order_MarkerList_Remark] DEFAULT ('') NOT NULL,
    [MixedSizeMarker]     VARCHAR (1)    CONSTRAINT [DF_Order_MarkerList_MultitudeSize] DEFAULT ('') NOT NULL,
    [MarkerNo]            VARCHAR (10)   CONSTRAINT [DF_Order_MarkerList_MarkerNo] DEFAULT ('') NOT NULL,
    [MarkerUpdate]        DATETIME       NULL,
    [MarkerUpdateName]    VARCHAR (10)   CONSTRAINT [DF_Order_MarkerList_MarkerUpdateName] DEFAULT ('') NOT NULL,
    [AllSize]             BIT            CONSTRAINT [DF_Order_MarkerList_AllSize] DEFAULT ((0)) NOT NULL,
    [PhaseID]             VARCHAR (10)   CONSTRAINT [DF_Order_MarkerList_PhaseID] DEFAULT ('') NOT NULL,
    [SMNoticeID]          VARCHAR (10)   CONSTRAINT [DF_Order_MarkerList_SMNoticeID] DEFAULT ('') NOT NULL,
    [MarkerVersion]       VARCHAR (3)    CONSTRAINT [DF_Order_MarkerList_MarkerVersion] DEFAULT ('') NOT NULL,
    [Direction]           NVARCHAR (40)  CONSTRAINT [DF_Order_MarkerList_Direction] DEFAULT ('') NOT NULL,
    [CuttingWidth]        VARCHAR (8)    CONSTRAINT [DF_Order_MarkerList_CuttingWidth] DEFAULT ('') NOT NULL,
    [Width]               VARCHAR (6)    CONSTRAINT [DF_Order_MarkerList_Width] DEFAULT ('') NOT NULL,
    [Type]                VARCHAR (1)    CONSTRAINT [DF_Order_MarkerList_Type] DEFAULT ('') NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Order_MarkerList_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Order_MarkerList_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [MarkerType]          INT            CONSTRAINT [DF_Order_MarkerList_MarkerType] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_MarkerList] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單-馬克檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序 (Seq)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別第一組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'FabricCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是QT 自動Copy的資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'isQT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MarkerLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁滾條', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Cuttingpiece';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁切週長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'ActCuttingPerimeter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁切週長(直線)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'StraightLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁切週長(曲線)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'CurvedLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Efficiency';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'多尺碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MixedSizeMarker';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MarkerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'從樣衣系統更新日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MarkerUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'從樣衣系統更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MarkerUpdateName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全段尺碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'AllSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克階段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'PhaseID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'SMNoticeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MarkerVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布紋裁向', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Direction';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'CuttingWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外裁種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'EditDate';


GO


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Marker版本(0=採購用，1=生產用)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_MarkerList', @level2type = N'COLUMN', @level2name = N'MarkerType';

