CREATE TABLE [dbo].[GarmentDefectType] (
    [ID]               VARCHAR (1)    CONSTRAINT [DF_GarmentDefectType_ID] DEFAULT ('') NOT NULL,
    [Description]      NVARCHAR (60)  CONSTRAINT [DF_GarmentDefectType_Description] DEFAULT ('') NULL,
    [Junk]             BIT            CONSTRAINT [DF_GarmentDefectType_Junk] DEFAULT ((0)) NULL,
    [LocalDescription] NVARCHAR (100) CONSTRAINT [DF_GarmentDefectType_LocalDescription] DEFAULT ('') NULL,
    [AddName]          VARCHAR (10)   CONSTRAINT [DF_GarmentDefectType_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME       NULL,
    [EditName]         VARCHAR (10)   CONSTRAINT [DF_GarmentDefectType_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME       NULL,
    CONSTRAINT [PK_GarmentDefectType] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Defect type for REF/CFA(Garment)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectType', @level2type = N'COLUMN', @level2name = N'EditDate';

