CREATE TABLE [dbo].[Order_TmsCost] (
    [ID]             VARCHAR (13)    CONSTRAINT [DF_Order_TmsCost_ID] DEFAULT ('') NOT NULL,
    [ArtworkTypeID]  VARCHAR (20)    CONSTRAINT [DF_Order_TmsCost_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [Seq]            VARCHAR (4)     CONSTRAINT [DF_Order_TmsCost_Seq] DEFAULT ('') NOT NULL,
    [Qty]            NUMERIC (6)     CONSTRAINT [DF_Order_TmsCost_Qty] DEFAULT ((0)) NULL,
    [ArtworkUnit]    VARCHAR (10)    CONSTRAINT [DF_Order_TmsCost_ArtworkUnit] DEFAULT ('') NULL,
    [TMS]            NUMERIC (5)     CONSTRAINT [DF_Order_TmsCost_TMS] DEFAULT ((0)) NULL,
    [Price]          NUMERIC (16, 4) CONSTRAINT [DF_Order_TmsCost_Price] DEFAULT ((0)) NOT NULL,
    [InhouseOSP]     VARCHAR (1)     CONSTRAINT [DF_Order_TmsCost_InhouseOSP] DEFAULT ('') NULL,
    [LocalSuppID]    VARCHAR (8)     CONSTRAINT [DF_Order_TmsCost_LocalSuppID] DEFAULT ('') NULL,
    [ArtworkInLine]  DATE            NULL,
    [ArtworkOffLine] DATE            NULL,
    [ApvDate]        DATE            NULL,
    [ApvName]        VARCHAR (10)    CONSTRAINT [DF_Order_TmsCost_ApvName] DEFAULT ('') NULL,
    [AddName]        VARCHAR (10)    CONSTRAINT [DF_Order_TmsCost_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME        NULL,
    [EditName]       VARCHAR (10)    CONSTRAINT [DF_Order_TmsCost_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME        NULL,
    [TPEEditName] VARCHAR(10) NULL DEFAULT (''), 
    [TPEEditDate] DATETIME NULL, 
    CONSTRAINT [PK_Order_TmsCost] PRIMARY KEY CLUSTERED ([ID] ASC, [ArtworkTypeID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TMS & COST', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號(排序用)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位(ex.Stitches/Panels/pcs)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'ArtworkUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'TMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發或內製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'InhouseOSP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'LocalSuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'ArtworkInLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工下線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'ArtworkOffLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'核可人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_TmsCost', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [<Name3 of Missing Index, sysname,>]
    ON [dbo].[Order_TmsCost]([ArtworkTypeID] ASC, [ArtworkInLine] ASC, [ArtworkOffLine] ASC, [ApvDate] ASC)
    INCLUDE([ID], [Price], [InhouseOSP]);


GO
CREATE NONCLUSTERED INDEX [<Name2 of Missing Index, sysname,>]
    ON [dbo].[Order_TmsCost]([InhouseOSP] ASC, [ApvDate] ASC)
    INCLUDE([ID], [ArtworkTypeID], [Qty], [LocalSuppID], [ArtworkInLine], [ArtworkOffLine]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Order_TmsCost]([ArtworkTypeID] ASC, [LocalSuppID] ASC, [ApvDate] ASC)
    INCLUDE([ID], [Price], [InhouseOSP]);

GO
CREATE NONCLUSTERED INDEX [Index_Seq] ON [dbo].[Order_TmsCost]
(
	[Seq] ASC
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北最後修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_TmsCost',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北最後修改時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_TmsCost',
    @level2type = N'COLUMN',
    @level2name = N'TPEEditDate'