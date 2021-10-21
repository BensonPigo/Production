CREATE TABLE [dbo].[GarmentDefectCode] (
    [ID]                  VARCHAR (3)    CONSTRAINT [DF_GarmentDefectCode_ID] DEFAULT ('') NOT NULL,
    [Description]         NVARCHAR (100) CONSTRAINT [DF_GarmentDefectCode_Description] DEFAULT ('') NULL,
    [GarmentDefectTypeID] VARCHAR (1)    CONSTRAINT [DF_GarmentDefectCode_GarmentDefectTypeID] DEFAULT ('') NULL,
    [Junk]                BIT            CONSTRAINT [DF_GarmentDefectCode_Junk] DEFAULT ((0)) NULL,
    [Seq]                 TINYINT        CONSTRAINT [DF_GarmentDefectCode_Seq] DEFAULT ((0)) NULL,
    [ReworkTotalFailCode] NVARCHAR (10)  CONSTRAINT [DF_GarmentDefectCode_ReworkTotalFailCode] DEFAULT ('') NULL,
    [IsCFA]               BIT            CONSTRAINT [DF_GarmentDefectCode_IsCFA] DEFAULT ((0)) NULL,
    [IsCriticalDefect]    BIT            CONSTRAINT [DF_GarmentDefectCode_IsCriticalDefect] DEFAULT ((0)) NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_GarmentDefectCode_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_GarmentDefectCode_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME       NULL,
    CONSTRAINT [PK_GarmentDefectCode] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為嚴重defect code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'IsCriticalDefect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否給CFA Inspection使用，1: 是；0: 不是 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'IsCFA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hanger System-FailCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'ReworkTotalFailCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'GarmentDefectTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentDefectCode', @level2type = N'COLUMN', @level2name = N'ID';

