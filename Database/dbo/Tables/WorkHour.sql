CREATE TABLE [dbo].[WorkHour] (
    [SewingLineID] VARCHAR (5)    CONSTRAINT [DF_WorkHour_SewingLineID] DEFAULT ('') NOT NULL,
    [FactoryID]    VARCHAR (8)    CONSTRAINT [DF_WorkHour_FactoryID] DEFAULT ('') NOT NULL,
    [Date]         DATE           NOT NULL,
    [Remark]       NVARCHAR (50)  CONSTRAINT [DF_WorkHour_Remark] DEFAULT ('') NULL,
    [Hours]        NUMERIC (3, 1) CONSTRAINT [DF_WorkHour_Hours] DEFAULT ((0)) NULL,
    [Holiday]      BIT            CONSTRAINT [DF_WorkHour_Holiday] DEFAULT ((0)) NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_WorkHour_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_WorkHour_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    CONSTRAINT [PK_WorkHour] PRIMARY KEY CLUSTERED ([SewingLineID] ASC, [FactoryID] ASC, [Date] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Working Hours Setup (工作時數設定檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'Date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'時數/天', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'Hours';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為假日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'Holiday';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkHour', @level2type = N'COLUMN', @level2name = N'EditDate';

