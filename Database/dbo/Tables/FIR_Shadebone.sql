CREATE TABLE [dbo].[FIR_Shadebone] (
    [ID]                     BIGINT         CONSTRAINT [DF_FIR_Shadebone_ID] DEFAULT ((0)) NOT NULL,
    [Roll]                   VARCHAR (8)    CONSTRAINT [DF_FIR_Shadebone_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]                 VARCHAR (8)    CONSTRAINT [DF_FIR_Shadebone_Dyelot] DEFAULT ('') NOT NULL,
    [Scale]                  VARCHAR (5)    CONSTRAINT [DF_FIR_Shadebone_Scale] DEFAULT ('') NULL,
    [Inspdate]               DATE           NULL,
    [Inspector]              VARCHAR (10)   CONSTRAINT [DF_FIR_Shadebone_Inspector] DEFAULT ('') NULL,
    [Result]                 VARCHAR (5)    CONSTRAINT [DF_FIR_Shadebone_Result] DEFAULT ('') NULL,
    [Remark]                 NVARCHAR (100) CONSTRAINT [DF_FIR_Shadebone_Remark] DEFAULT ('') NULL,
    [AddName]                VARCHAR (10)   CONSTRAINT [DF_FIR_Shadebone_AddName] DEFAULT ('') NULL,
    [AddDate]                DATETIME       NULL,
    [EditName]               VARCHAR (10)   CONSTRAINT [DF_FIR_Shadebone_EditName] DEFAULT ('') NULL,
    [EditDate]               DATETIME       NULL,
    [TicketYds]              NUMERIC (8, 2) DEFAULT ((0)) NULL,
    [CutTime]                DATETIME       NULL,
    [PasteTime]              DATETIME       NULL,
    [PassQATime]             DATETIME       NULL,
    [ShadebandDocLocationID] VARCHAR (10)   NULL,
    [CutBy]                  VARCHAR (10)   CONSTRAINT [DF_FIR_Shadebone_CutBy] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_FIR_Shadebone] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Inspection - Shade bone Inspection', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'灰階分數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'Scale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'Inspdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ŤU�����ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'CutTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�K����W���ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'PasteTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�����浹QA���ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'PassQATime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���s�񪺦�m', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'ShadebandDocLocationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責剪 Shadeband 的人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Shadebone', @level2type = N'COLUMN', @level2name = N'CutBy';

