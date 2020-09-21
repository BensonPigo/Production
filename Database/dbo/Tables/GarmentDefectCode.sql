CREATE TABLE [dbo].[GarmentDefectCode] (
    [ID]                  VARCHAR (3)    CONSTRAINT [DF_GarmentDefectCode_ID] DEFAULT ('') NOT NULL,
    [Description]         NVARCHAR (100) CONSTRAINT [DF_GarmentDefectCode_Description] DEFAULT ('') NOT NULL,
    [GarmentDefectTypeID] VARCHAR (1)    CONSTRAINT [DF_GarmentDefectCode_GarmentDefectTypeID] DEFAULT ('') NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_GarmentDefectCode_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NOT NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_GarmentDefectCode_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [Junk]                BIT            CONSTRAINT [df_Junk_Zero] DEFAULT ((0)) NOT NULL,
    [LocalDescription]    NVARCHAR (100) NOT NULL,
    [ReworkTotalFailCode] NVARCHAR(10) NULL CONSTRAINT [DF_GarmentDefectCode_ReworkTotalFailCode] DEFAULT (''), 
    [IsCFA] BIT NOT NULL CONSTRAINT [DF_GarmentDefectCode_IsCFA] DEFAULT ((0)), 
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

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hanger System的FailCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'ReworkTotalFailCode';

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否給CFA Inspection使用，1: 是；0: 不是',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GarmentDefectCode',
    @level2type = N'COLUMN',
    @level2name = N'IsCFA'