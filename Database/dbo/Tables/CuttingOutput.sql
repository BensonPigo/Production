CREATE TABLE [dbo].[CuttingOutput] (
    [ID]          VARCHAR (13)    CONSTRAINT [DF_CuttingOutput_ID] DEFAULT ('') NOT NULL,
    [cDate]       DATE            NULL,
    [MDivisionid] VARCHAR (8)     CONSTRAINT [DF_CuttingOutput_Factoryid] DEFAULT ('') NULL,
    [Manpower]    NUMERIC (3)     CONSTRAINT [DF_CuttingOutput_Manpower] DEFAULT ((0)) NULL,
    [Manhours]    NUMERIC (5, 1)  CONSTRAINT [DF_CuttingOutput_Manhours] DEFAULT ((0)) NULL,
    [Actoutput]   NUMERIC (7)     CONSTRAINT [DF_CuttingOutput_Actoutput] DEFAULT ((0)) NULL,
    [ActGarment]  INT     CONSTRAINT [DF_CuttingOutput_ActGarment] DEFAULT ((0)) NULL,
    [Lock]        DATE            NULL,
    [Status]      VARCHAR (15)    CONSTRAINT [DF_CuttingOutput_Status] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)    CONSTRAINT [DF_CuttingOutput_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME        NULL,
    [EditName]    VARCHAR (10)    CONSTRAINT [DF_CuttingOutput_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME        NULL,
    [PPH]         NUMERIC (8, 2)  CONSTRAINT [DF_CuttingOutput_PPH] DEFAULT ((0)) NULL,
    [FactoryID]   VARCHAR (8)     NULL,
    [ActTTCPU]    NUMERIC (10, 3) CONSTRAINT [DF_CuttingOutput_ActTTCPU] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CuttingOutput] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cutting Daily Output', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'cDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人力', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'Manpower';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總時數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'Manhours';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際產出(Yds)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'Actoutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際產出數量(GMT PCS)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'ActGarment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lock', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'Lock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutput', @level2type = N'COLUMN', @level2name = N'MDivisionid';

