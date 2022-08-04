CREATE TABLE [dbo].[PO_Artwork] (
    [ID]         VARCHAR (13)    CONSTRAINT [DF_PO_Artwork_ID] DEFAULT ('') NOT NULL,
    [SEQ1]       VARCHAR (3)     CONSTRAINT [DF_PO_Artwork_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]       VARCHAR (2)     CONSTRAINT [DF_PO_Artwork_SEQ2] DEFAULT ('') NOT NULL,
    [Refno]      VARCHAR (36)    CONSTRAINT [DF_PO_Artwork_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]   VARCHAR (26)    CONSTRAINT [DF_PO_Artwork_SCIRefno] DEFAULT ('') NULL,
    [FabricType] VARCHAR (1)     CONSTRAINT [DF_PO_Artwork_FabricType] DEFAULT ('') NOT NULL,
    [Qty]        NUMERIC (8, 2)  CONSTRAINT [DF_PO_Artwork_Qty] DEFAULT ((0)) NULL,
    [POUnit]     VARCHAR (8)     CONSTRAINT [DF_PO_Artwork_POUnit] DEFAULT ('') NOT NULL,
    [ColorID]    VARCHAR (6)     CONSTRAINT [DF_PO_Artwork_ColorID] DEFAULT ('') NULL,
    [Remark]     NVARCHAR (MAX)  CONSTRAINT [DF_PO_Artwork_Remark] DEFAULT ('') NULL,
    [Width]      NUMERIC (3, 1)  CONSTRAINT [DF_PO_Artwork_Width] DEFAULT ((0)) NULL,
    [Special]    NVARCHAR (60)   CONSTRAINT [DF_PO_Artwork_Special] DEFAULT ('') NULL,
    [Junk]       BIT             CONSTRAINT [DF_PO_Artwork_Junk] DEFAULT ((0)) NULL,
    [AddName]    VARCHAR (10)    CONSTRAINT [DF_PO_Artwork_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME        NULL,
    [EditName]   VARCHAR (10)    CONSTRAINT [DF_PO_Artwork_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME        NULL,
    [SizeSpec]   VARCHAR (8)     NULL,
    [StockUnit]  VARCHAR (8)     NULL,
    [InQty]      NUMERIC (10, 2) NULL,
    [OutQty]     NUMERIC (10, 2) NULL,
    [AdjustQty]  NUMERIC (10, 2) NULL,
    [ALocation]  VARCHAR (100)   NULL,
    CONSTRAINT [PK_PO_Artwork] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC, [SEQ2] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'領料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'POUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特殊規格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'Special';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PO_Artwork', @level2type = N'COLUMN', @level2name = N'EditDate';

