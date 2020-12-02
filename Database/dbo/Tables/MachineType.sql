CREATE TABLE [dbo].[MachineType] (
    [ID]             VARCHAR (10)   CONSTRAINT [DF_MachineType_ID] DEFAULT ('') NOT NULL,
    [Description]    NVARCHAR (60)  CONSTRAINT [DF_MachineType_Description] DEFAULT ('') NULL,
    [DescCH]         NVARCHAR (60)  CONSTRAINT [DF_MachineType_DescCH] DEFAULT ('') NULL,
    [ISO]            VARCHAR (10)   CONSTRAINT [DF_MachineType_ISO] DEFAULT ('') NULL,
    [ArtworkTypeID]  VARCHAR (20)   CONSTRAINT [DF_MachineType_ArtworkTypeID] DEFAULT ('') NULL,
    [Mold]           BIT            CONSTRAINT [DF_MachineType_Mold] DEFAULT ((0)) NULL,
    [RPM]            INT            CONSTRAINT [DF_MachineType_RPM] DEFAULT ((0)) NULL,
    [Stitches]       NUMERIC (6, 1) CONSTRAINT [DF_MachineType_Stitches] DEFAULT ((0)) NULL,
    [Picture1]       NVARCHAR (60)  CONSTRAINT [DF_MachineType_Picture1] DEFAULT ('') NULL,
    [Picture2]       NVARCHAR (60)  CONSTRAINT [DF_MachineType_Picture2] DEFAULT ('') NULL,
    [MachineAllow]   NUMERIC (4, 2) CONSTRAINT [DF_MachineType_MachineAllow] DEFAULT ((0)) NULL,
    [ManAllow]       NUMERIC (4, 2) CONSTRAINT [DF_MachineType_ManAllow] DEFAULT ((0)) NULL,
    [MachineGroupID] VARCHAR (2)    CONSTRAINT [DF_MachineType_MachineGroupID] DEFAULT ('') NULL,
    [Junk]           BIT            CONSTRAINT [DF_MachineType_Junk] DEFAULT ((0)) NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_MachineType_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME       NULL,
    [EditName]       VARCHAR (10)   CONSTRAINT [DF_MachineType_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME       NULL,
    [isThread]       BIT            NULL,
    [MasterGroupID]  VARCHAR (2)    NULL,
    [Hem]            BIT            DEFAULT ((0)) NOT NULL,
    [IsDesignatedArea] BIT CONSTRAINT [DF_MachineType_IsDesignatedArea] NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_MachineType] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台類別基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台類別代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文機台描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'DescCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台ISO代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'ISO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台作工種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否需要模具', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'Mold';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉速', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'RPM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每公分車縫針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'Stitches';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'Picture1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'Picture2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台耗損率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'MachineAllow';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人工耗損率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'ManAllow';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台群組代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'MachineGroupID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該機器類型要用於指定位置(非sewing line)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MachineType',
    @level2type = N'COLUMN',
    @level2name = N'IsDesignatedArea'