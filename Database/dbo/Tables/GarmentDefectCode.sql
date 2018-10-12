CREATE TABLE [dbo].[GarmentDefectCode] (
    [ID]                  VARCHAR (3)    CONSTRAINT [DF_GarmentDefectCode_ID] DEFAULT ('') NOT NULL,
    [Description]         NVARCHAR (100) CONSTRAINT [DF_GarmentDefectCode_Description] DEFAULT ('') NULL,
    [GarmentDefectTypeID] VARCHAR (1)    CONSTRAINT [DF_GarmentDefectCode_GarmentDefectTypeID] DEFAULT ('') NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_GarmentDefectCode_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_GarmentDefectCode_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME       NULL,
    [Junk]                BIT            NULL,
    [LocalDescription]    NVARCHAR (100) NULL,
    CONSTRAINT [PK_GarmentDefectCode] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Defect Code for REF/CFA(Garment)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'GarmentDefectTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'EditDate';

