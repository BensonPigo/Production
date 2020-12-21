CREATE TABLE [dbo].[Order_BOF] (
    [Id]                 VARCHAR (13)    CONSTRAINT [DF_Order_BOF_Id] DEFAULT ('') NOT NULL,
    [FabricCode]         VARCHAR (3)     CONSTRAINT [DF_Order_BOF_FabricCode] DEFAULT ('') NOT NULL,
    [Refno]              VARCHAR (20)    CONSTRAINT [DF_Order_BOF_Refno] DEFAULT ('') NOT NULL,
    [SCIRefno]           VARCHAR (30)    CONSTRAINT [DF_Order_BOF_SCIRefno] DEFAULT ('') NOT NULL,
    [SuppID]             VARCHAR (6)     CONSTRAINT [DF_Order_BOF_SuppID] DEFAULT ('') NOT NULL,
    [ConsPC]             NUMERIC (11, 4) CONSTRAINT [DF_Order_BOF_ConsPC] DEFAULT ((0)) NULL,
    [Seq1]               VARCHAR (3)     CONSTRAINT [DF_Order_BOF_Seq1] DEFAULT ('') NOT NULL,
    [Kind]               VARCHAR (1)     CONSTRAINT [DF_Order_BOF_Kind] DEFAULT ('') NULL,
    [Ukey]               BIGINT          CONSTRAINT [DF_Order_BOF_Ukey] DEFAULT ((0)) NOT NULL,
    [Remark]             NVARCHAR (MAX)  CONSTRAINT [DF_Order_BOF_Remark] DEFAULT ('') NULL,
    [LossType]           NUMERIC (1)     CONSTRAINT [DF_Order_BOF_LossType] DEFAULT ((0)) NOT NULL,
    [LossPercent]        NUMERIC (5, 1)  CONSTRAINT [DF_Order_BOF_LossPercent] DEFAULT ((0)) NULL,
    [RainwearTestPassed] BIT             CONSTRAINT [DF_Order_BOF_RainwearTestPassed] DEFAULT ((0)) NULL,
    [HorizontalCutting]  BIT             CONSTRAINT [DF_Order_BOF_HorizontalCutting] DEFAULT ((0)) NULL,
    [ColorDetail]        NVARCHAR (100)  CONSTRAINT [DF_Order_BOF_ColorDetail] DEFAULT ('') NULL,
    [AddName]            VARCHAR (10)    CONSTRAINT [DF_Order_BOF_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME        NULL,
    [EditName]           VARCHAR (10)    CONSTRAINT [DF_Order_BOF_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME        NULL,
    [SpecialWidth]       NUMERIC (5, 2)  NULL,
    [LimitUp]            DECIMAL (7, 2)  NULL,
    [LimitDown]          DECIMAL (7, 2)  NULL,
    CONSTRAINT [PK_Order_BOF] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order : Bill of Fabric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價的單件用量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'ConsPC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單大項序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用途', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'Kind';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗計算方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'LossType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗百分比', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'LossPercent';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'水洗測式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'RainwearTestPassed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'橫裁', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'HorizontalCutting';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'ColorDetail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_BOF', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Order_BOF]([Id] ASC);

