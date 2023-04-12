CREATE TABLE [dbo].[Order_EachCons] (
    [Id]                  VARCHAR (13)   CONSTRAINT [DF_Order_EachCons_Id] DEFAULT ('') NOT NULL,
    [Ukey]                BIGINT         CONSTRAINT [DF_Order_EachCons_Ukey] DEFAULT ((0)) NOT NULL,
    [Seq]                 NUMERIC (4)    CONSTRAINT [DF_Order_EachCons_Seq] DEFAULT ((0)) NOT NULL,
    [MarkerName]          NVARCHAR (20)  CONSTRAINT [DF_Order_EachCons_MarkerName] DEFAULT ('') NOT NULL,
    [FabricCombo]         VARCHAR (2)    CONSTRAINT [DF_Order_EachCons_FabricCombo] DEFAULT ('') NOT NULL,
    [MarkerLength]        VARCHAR (15)   CONSTRAINT [DF_Order_EachCons_MarkerLength] DEFAULT ('') NULL,
    [FabricPanelCode]          VARCHAR (2)    CONSTRAINT [DF_Order_EachCons_FabricPanelCode] DEFAULT ('') NULL,
    [ConsPC]              NUMERIC (8, 4) CONSTRAINT [DF_Order_EachCons_ConsPC] DEFAULT ((0)) NOT NULL,
    [CuttingPiece]        BIT            CONSTRAINT [DF_Order_EachCons_CuttingPiece] DEFAULT ((0)) NULL,
    [ActCuttingPerimeter] NVARCHAR(15)   CONSTRAINT [DF_Order_EachCons_ActCuttingPerimeter] DEFAULT ('') NULL,
    [StraightLength]      VARCHAR (15)   CONSTRAINT [DF_Order_EachCons_StraightLength] DEFAULT ('') NULL,
    [FabricCode]          VARCHAR (3)    CONSTRAINT [DF_Order_EachCons_FabricCode] DEFAULT ('') NOT NULL,
    [CurvedLength]        VARCHAR (15)   CONSTRAINT [DF_Order_EachCons_CurvedLength] DEFAULT ('') NULL,
    [Efficiency]          VARCHAR (9)    CONSTRAINT [DF_Order_EachCons_Efficiency] DEFAULT ('') NULL,
    [Article]             NVARCHAR (MAX) CONSTRAINT [DF_Order_EachCons_Article] DEFAULT ('') NULL,
    [Remark]              NVARCHAR (100) CONSTRAINT [DF_Order_EachCons_Remark] DEFAULT ('') NULL,
    [MixedSizeMarker]     VARCHAR (1)    CONSTRAINT [DF_Order_EachCons_MultitudeSize] DEFAULT ('') NULL,
    [MarkerNo]            VARCHAR (10)   CONSTRAINT [DF_Order_EachCons_MarkerNo] DEFAULT ('') NULL,
    [MarkerUpdate]        DATETIME       NULL,
    [MarkerUpdateName]    VARCHAR (10)   CONSTRAINT [DF_Order_EachCons_MarkerUpdateName] DEFAULT ('') NULL,
    [AllSize]             BIT            CONSTRAINT [DF_Order_EachCons_AllSize] DEFAULT ((0)) NULL,
    [PhaseID]             VARCHAR (10)   CONSTRAINT [DF_Order_EachCons_PhaseID] DEFAULT ('') NULL,
    [SMNoticeID]          VARCHAR (10)   CONSTRAINT [DF_Order_EachCons_SMNoticeID] DEFAULT ('') NULL,
    [MarkerVersion]       VARCHAR (3)    CONSTRAINT [DF_Order_EachCons_MarkerVersion] DEFAULT ('') NULL,
    [Direction]           NVARCHAR(40)     CONSTRAINT [DF_Order_EachCons_Direction] DEFAULT ('') NULL,
    [CuttingWidth]        VARCHAR (8)    CONSTRAINT [DF_Order_EachCons_CuttingWidth] DEFAULT ('') NULL,
    [Width]               VARCHAR (6)    CONSTRAINT [DF_Order_EachCons_Width] DEFAULT ('') NULL,
    [TYPE]                VARCHAR (1)    CONSTRAINT [DF_Order_EachCons_TYPE] DEFAULT ((0)) NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Order_EachCons_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Order_EachCons_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME       NULL,
    [isQT]                BIT            CONSTRAINT [DF_Order_EachCons_isQT] DEFAULT ((0)) NULL,
    [MarkerDownloadID]    VARCHAR (25)   NULL,
    [OrderCUkey_Old]      VARCHAR (10)   NULL,
    [NoNotch] BIT CONSTRAINT [DF_Order_EachCons_NoNotch] DEFAULT ((0)) not NULL,
    CONSTRAINT [PK_Order_EachCons] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order Each Cons', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'碼克名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'FabricCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'MarkerLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁滾條/-(不是Table上裁的)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'CuttingPiece';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁切週長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'ActCuttingPerimeter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁切週長(直線)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'StraightLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際裁切週長(曲線)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'CurvedLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Efficiency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'指定顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為多尺碼 1:單 2:多', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'MixedSizeMarker';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'樣衣系統的版號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'MarkerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'從樣衣系統更新日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'MarkerUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'從樣衣系統更新人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'MarkerUpdateName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全尺碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'AllSize';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克階段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'PhaseID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'碼克需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'SMNoticeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'碼克需求版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'MarkerVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'直裁/橫裁/斜裁', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Direction';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'CuttingWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外裁種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'TYPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是QT複製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_EachCons', @level2type = N'COLUMN', @level2name = N'isQT';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Order_EachCons]([Id] ASC);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'紀錄CutPart是否不須放置Notch',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_EachCons',
    @level2type = N'COLUMN',
    @level2name = N'NoNotch'