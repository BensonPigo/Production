CREATE TABLE [dbo].[LossRateAccessory] (
    [MtltypeId]          VARCHAR (20)   CONSTRAINT [DF_LossRateAccessory_MtltypeId] DEFAULT ('') NOT NULL,
    [LossUnit]           TINYINT        CONSTRAINT [DF_LossRateAccessory_LossUnit] DEFAULT ((0)) NULL,
    [LossTW]             TINYINT        CONSTRAINT [DF_LossRateAccessory_LossTW] DEFAULT ((0)) NULL,
    [LossNonTW]          TINYINT        CONSTRAINT [DF_LossRateAccessory_LossNonTW] DEFAULT ((0)) NULL,
    [PerQtyTW]           INT            CONSTRAINT [DF_LossRateAccessory_PerQtyTW] DEFAULT ((0)) NULL,
    [PlsQtyTW]           SMALLINT       CONSTRAINT [DF_LossRateAccessory_PlsQtyTW] DEFAULT ((0)) NULL,
    [PerQtyNonTW]        INT            CONSTRAINT [DF_LossRateAccessory_PerQtyNonTW] DEFAULT ((0)) NULL,
    [PlsQtyNonTW]        SMALLINT       CONSTRAINT [DF_LossRateAccessory_PlsQtyNonTW] DEFAULT ((0)) NULL,
    [FOCTW]              TINYINT        CONSTRAINT [DF_LossRateAccessory_FOCTW] DEFAULT ((0)) NULL,
    [FOCNonTW]           TINYINT        CONSTRAINT [DF_LossRateAccessory_FOCNonTW] DEFAULT ((0)) NULL,
    [AddName]            VARCHAR (10)   CONSTRAINT [DF_LossRateAccessory_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME       NULL,
    [EditName]           VARCHAR (10)   CONSTRAINT [DF_LossRateAccessory_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME       NULL,
    [Waste]              NUMERIC (5, 3) CONSTRAINT [DF_LossRateAccessory_Waste] DEFAULT ((0)) NULL,
    [IgnoreLimitUpBrand] NVARCHAR (200) NULL,
    CONSTRAINT [PK_LossRateAccessory] PRIMARY KEY CLUSTERED ([MtltypeId] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Loss rate of Accessory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'MtltypeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'LossUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗百分比 - 台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'LossTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗百分比 - 非台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'LossNonTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'根據採購數量 - 台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'PerQtyTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗數  - 台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'PlsQtyTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'根據採購數量 - 非台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'PerQtyNonTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上損耗數  - 非台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'PlsQtyNonTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上免費數量百分比 - 台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'FOCTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加上免費數量百分比 - 非台灣', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'FOCNonTW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'損耗率(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LossRateAccessory', @level2type = N'COLUMN', @level2name = N'Waste';

