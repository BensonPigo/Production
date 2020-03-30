CREATE TABLE [dbo].[ShipMode] (
    [ID]                   VARCHAR (10)   CONSTRAINT [DF_ShipMode_ID] DEFAULT ('') NOT NULL,
    [Description]          NVARCHAR (50)  CONSTRAINT [DF_ShipMode_Description] DEFAULT ('') NULL,
    [UseFunction]          NVARCHAR (100) CONSTRAINT [DF_ShipMode_UseFunction] DEFAULT ('') NULL,
    [Junk]                 BIT            CONSTRAINT [DF_ShipMode_Junk] DEFAULT ((0)) NULL,
    [ShareBase]            VARCHAR (1)    CONSTRAINT [DF_ShipMode_ShareBase] DEFAULT ('') NULL,
    [AddName]              VARCHAR (10)   CONSTRAINT [DF_ShipMode_AddName] DEFAULT ('') NULL,
    [AddDate]              DATETIME       NULL,
    [EditName]             VARCHAR (10)   CONSTRAINT [DF_ShipMode_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME       NULL,
    [IncludeSeaShipping]   BIT            DEFAULT ((0)) NOT NULL,
    [NeedCreateAPP]        BIT            CONSTRAINT [DF_ShipMode_NeedCreateAPP] DEFAULT ((0)) NOT NULL,
    [NeedCreateIntExpress] BIT            DEFAULT ((0)) NOT NULL,
    [ShipGroup]            VARCHAR (10)   DEFAULT ('') NOT NULL,
    [LoadingType]          VARCHAR (20)   DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ShipMode] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用的功能', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'UseFunction';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報廢', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分攤', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'ShareBase';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Need create International Express', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShipMode', @level2type = N'COLUMN', @level2name = N'NeedCreateIntExpress';