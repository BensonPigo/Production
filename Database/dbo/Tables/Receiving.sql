CREATE TABLE [dbo].[Receiving] (
    [Id]              VARCHAR (13)   CONSTRAINT [DF_Receiving_Id] DEFAULT ('') NOT NULL,
    [InvNo]           VARCHAR (25)   CONSTRAINT [DF_Receiving_InvNo] DEFAULT ('') NULL,
    [Type]            VARCHAR (1)    CONSTRAINT [DF_Receiving_Type] DEFAULT ('') NOT NULL,
    [FactoryId]       VARCHAR (8)    CONSTRAINT [DF_Receiving_FactoryId] DEFAULT ('') NOT NULL,
    [ExportId]        VARCHAR (13)   CONSTRAINT [DF_Receiving_ExportId] DEFAULT ('') NULL,
    [ETA]             DATE           NULL,
    [Third]           BIT            CONSTRAINT [DF_Receiving_Third] DEFAULT ((0)) NULL,
    [PackingReceive]  DATE           NULL,
    [WhseArrival]     DATE           NULL,
    [Status]          VARCHAR (15)   CONSTRAINT [DF_Receiving_Status] DEFAULT ('') NULL,
    [Transfer2Taipei] DATE           NULL,
    [InspectionRate]  NUMERIC (5, 2) CONSTRAINT [DF_Receiving_InspectionRate] DEFAULT ((0)) NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_Receiving_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_Receiving_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    CONSTRAINT [PK_Receiving] PRIMARY KEY CLUSTERED ([Id] ASC) ON [SLAVE]
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發票號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠代', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作底稿編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'ExportId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'第三國', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'Third';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing收單日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'PackingReceive';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫收料日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'WhseArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'資料回傳台北日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'Transfer2Taipei';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗比率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'InspectionRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving', @level2type = N'COLUMN', @level2name = N'EditDate';

