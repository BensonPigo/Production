CREATE TABLE [dbo].[OrderType] (
    [ID]                  VARCHAR (20)    CONSTRAINT [DF_OrderType_ID] DEFAULT ('') NOT NULL,
    [BrandID]             VARCHAR (8)     CONSTRAINT [DF_OrderType_BrandID] DEFAULT ('') NOT NULL,
    [PhaseID]             VARCHAR (20)    CONSTRAINT [DF_OrderType_PhaseID] DEFAULT ('') NULL,
    [ProjectID]           VARCHAR (5)     CONSTRAINT [DF_OrderType_ProjectID] DEFAULT ('') NULL,
    [Junk]                BIT             CONSTRAINT [DF_OrderType_Junk] DEFAULT ((0)) NULL,
    [CpuRate]             NUMERIC (3, 1)  CONSTRAINT [DF_OrderType_CpuRate] DEFAULT ((0)) NULL,
    [Category]            VARCHAR (1)     CONSTRAINT [DF_OrderType_Category] DEFAULT ('') NULL,
    [PriceDays]           NUMERIC (16, 4) CONSTRAINT [DF_OrderType_PriceDays] DEFAULT ((0)) NULL,
    [MtlLetaDays]         NUMERIC (2)     CONSTRAINT [DF_OrderType_MtlLetaDays] DEFAULT ((0)) NULL,
    [EachConsDays]        NUMERIC (2)     CONSTRAINT [DF_OrderType_EachConsDays] DEFAULT ((0)) NULL,
    [KPI]                 BIT             CONSTRAINT [DF_OrderType_KPI] DEFAULT ((0)) NULL,
    [Remark]              NVARCHAR (60)   CONSTRAINT [DF_OrderType_Remark] DEFAULT ('') NULL,
    [AddName]             VARCHAR (10)    CONSTRAINT [DF_OrderType_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME        NULL,
    [EditName]            VARCHAR (10)    CONSTRAINT [DF_OrderType_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME        NULL,
    [IsGMTMaster]         BIT             DEFAULT ('0') NULL,
    [IsGMTDetail]         BIT             DEFAULT ('0') NULL,
    [IsDevSample]         BIT             NULL,
    [KPIProjectID]        VARCHAR (5)     NULL,
    [CalByBOFConsumption] BIT             NULL,
    CONSTRAINT [PK_OrderType] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁階段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'PhaseID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'專案代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'ProjectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能的倍率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'CpuRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Category', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KPI 推算採購 Price 設定達標的天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'PriceDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'換算Mtl LETA Date的天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'MtlLetaDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KPI 推算Each consumption 達標的天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'EachConsDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否扣KPI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'KPI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�O�_���}�o��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OrderType', @level2type = N'COLUMN', @level2name = N'IsDevSample';

