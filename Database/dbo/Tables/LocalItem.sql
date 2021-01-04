CREATE TABLE [dbo].[LocalItem] (
    [RefNo]          VARCHAR (21)    CONSTRAINT [DF_LocalItem_RefNo] DEFAULT ('') NOT NULL,
    [Junk]           BIT             CONSTRAINT [DF_LocalItem_Junk] DEFAULT ((0)) NULL,
    [Description]    NVARCHAR (MAX)  CONSTRAINT [DF_LocalItem_Description] DEFAULT ('') NOT NULL,
    [Category]       VARCHAR (20)    CONSTRAINT [DF_LocalItem_Category] DEFAULT ('') NOT NULL,
    [UnitID]         VARCHAR (8)     CONSTRAINT [DF_LocalItem_UnitID] DEFAULT ('') NOT NULL,
    [LocalSuppid]    VARCHAR (8)     CONSTRAINT [DF_LocalItem_LocalSuppid] DEFAULT ('') NULL,
    [Price]          NUMERIC (12, 4) CONSTRAINT [DF_LocalItem_Price] DEFAULT ((0)) NULL,
    [CtnLength]      NUMERIC (8, 4)  CONSTRAINT [DF_LocalItem_CtnLength] DEFAULT ((0)) NULL,
    [CtnWidth]       NUMERIC (8, 4)  CONSTRAINT [DF_LocalItem_CtnWidth] DEFAULT ((0)) NULL,
    [CtnHeight]      NUMERIC (8, 4)  CONSTRAINT [DF_LocalItem_CtnHeight] DEFAULT ((0)) NULL,
    [CtnUnit]        VARCHAR (8)     CONSTRAINT [DF_LocalItem_CtnUnit] DEFAULT ('') NULL,
    [CtnWeight]      NUMERIC (11, 6) CONSTRAINT [DF_LocalItem_CtnWeight] DEFAULT ((0)) NULL,
    [CBM]            NUMERIC (13, 4) CONSTRAINT [DF_LocalItem_CBM] DEFAULT ((0)) NULL,
    [MeterToCone]    NUMERIC (7, 1)  CONSTRAINT [DF_LocalItem_MeterToCone] DEFAULT ((0)) NULL,
    [ThreadTypeID]   VARCHAR (15)    CONSTRAINT [DF_LocalItem_ThreadTypeID] DEFAULT ('') NULL,
    [ThreadTex]      NUMERIC (3)     CONSTRAINT [DF_LocalItem_ThreadTex] DEFAULT ((0)) NULL,
    [AccountID]      VARCHAR (8)     CONSTRAINT [DF_LocalItem_AccountNo] DEFAULT ('') NULL,
    [Weight]         NUMERIC (11, 6) CONSTRAINT [DF_LocalItem_Weight] DEFAULT ((0)) NULL,
    [AxleWeight]     NUMERIC (9, 4)  CONSTRAINT [DF_LocalItem_AxleWeight] DEFAULT ((0)) NULL,
    [CurrencyID]     VARCHAR (3)     CONSTRAINT [DF_LocalItem_CurrencyID] DEFAULT ('') NULL,
    [QuotDate]       DATE            NULL,
    [AddName]        VARCHAR (10)    CONSTRAINT [DF_LocalItem_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME        NULL,
    [EditName]       VARCHAR (10)    CONSTRAINT [DF_LocalItem_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME        NULL,
    [NLCode]         VARCHAR (9)     CONSTRAINT [DF_LocalItem_NLCode] DEFAULT ('') NULL,
    [HSCode]         VARCHAR (11)    CONSTRAINT [DF_LocalItem_HSCode] DEFAULT ('') NULL,
    [CustomsUnit]    VARCHAR (8)     CONSTRAINT [DF_LocalItem_CustomsUnit] DEFAULT ('') NULL,
    [PcsWidth]       NUMERIC (7, 4)  CONSTRAINT [DF_LocalItem_PcsWidth] DEFAULT ((0)) NULL,
    [PcsLength]      NUMERIC (7, 4)  CONSTRAINT [DF_LocalItem_PcsLength] DEFAULT ((0)) NULL,
    [PcsKg]          NUMERIC (5, 4)  CONSTRAINT [DF_LocalItem_PcsKg] DEFAULT ((0)) NULL,
    [NoDeclare]      BIT             CONSTRAINT [DF_LocalItem_NoDeclare] DEFAULT ((0)) NULL,
    [NLCodeEditName] VARCHAR (10)    CONSTRAINT [DF_LocalItem_NLCodeEditName] DEFAULT ('') NULL,
    [NLCodeEditDate] DATETIME        NULL,
    [ArtTkt ] VARCHAR(20) NULL DEFAULT (''), 
    [IsCarton] BIT NULL DEFAULT ((0)), 
    [UnPack] BIT NOT NULL DEFAULT 0, 
	CartonType Varchar (6) NOT NULL CONSTRAINT [DF_LocalItem_CartonType]  DEFAULT(''),
    CONSTRAINT [PK_LocalItem] PRIMARY KEY CLUSTERED ([RefNo] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Purchase Item', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'LocalSuppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱規格-長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'CtnLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱規格-寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'CtnWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱規格-高', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'CtnHeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'紙箱單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'CtnUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'CBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'米跟cone的換算值(1Cone為幾米)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'MeterToCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'ThreadTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線每千米克重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'ThreadTex';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量(g)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'Weight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'軸心重(g)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'AxleWeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報價確認日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'QuotDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalItem', @level2type = N'COLUMN', @level2name = N'AccountID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'確認該物料為純紙箱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalItem',
    @level2type = N'COLUMN',
    @level2name = N'IsCarton';

GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否支援自動開箱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalItem',
    @level2type = N'COLUMN',
    @level2name = N'UnPack';
	
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
	@value = N'客人定義紙箱的規格，工廠看到 Carton Type 便會知道是哪種規格的紙箱',
	@level0type = N'SCHEMA',
	@level0name = N'dbo',
	@level1type = N'TABLE',
	@level1name = N'LocalItem',
	@level2type = N'COLUMN',
	@level2name = N'CartonType'
GO
