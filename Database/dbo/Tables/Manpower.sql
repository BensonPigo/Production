CREATE TABLE [dbo].[Manpower] (
    [FactoryID]        VARCHAR (8)     CONSTRAINT [DF_Manpower_FactoryID] DEFAULT ('') NOT NULL,
    [Year]             VARCHAR (4)     CONSTRAINT [DF_Manpower_Year] DEFAULT ('') NOT NULL,
    [Month]            VARCHAR (2)     CONSTRAINT [DF_Manpower_Month] DEFAULT ('') NOT NULL,
    [ActiveManpower]   INT             CONSTRAINT [DF_Manpower_ActiveManpower] DEFAULT ((0)) NOT NULL,
    [Lines]            INT             CONSTRAINT [DF_Manpower_Lines] DEFAULT ((0)) NULL,
    [DirectManpower]   INT             CONSTRAINT [DF_Manpower_DirectManpower] DEFAULT ((0)) NULL,
    [ManpowerRatio]    NUMERIC (8, 2)  CONSTRAINT [DF_Manpower_ManpowerRatio] DEFAULT ((0)) NULL,
    [TotalWorkhour]    NUMERIC (7, 2)  CONSTRAINT [DF_Manpower_TotalWorkhour] DEFAULT ((0)) NULL,
    [TotalCPU]         INT             CONSTRAINT [DF_Manpower_TotalCPU] DEFAULT ((0)) NULL,
    [InHouse]          INT             CONSTRAINT [DF_Manpower_InHouse] DEFAULT ((0)) NULL,
    [OperatingCost]    INT             CONSTRAINT [DF_Manpower_OperatingCost] DEFAULT ((0)) NULL,
    [OperatingExpense] INT             CONSTRAINT [DF_Manpower_OperatingExpense] DEFAULT ((0)) NULL,
    [Rental]           INT             CONSTRAINT [DF_Manpower_Rental] DEFAULT ((0)) NULL,
    [TotalExpense]     INT             CONSTRAINT [DF_Manpower_TotalExpense] DEFAULT ((0)) NULL,
    [WorkDay]          NUMERIC (4, 2)  CONSTRAINT [DF_Manpower_WorkDay] DEFAULT ((0)) NULL,
    [AvgDayWorkhour]   NUMERIC (4, 2)  CONSTRAINT [DF_Manpower_AvgDayWorkhour] DEFAULT ((0)) NULL,
    [PPH]              NUMERIC (10, 2) CONSTRAINT [DF_Manpower_PPH] DEFAULT ((0)) NULL,
    [AddName]          VARCHAR (10)    CONSTRAINT [DF_Manpower_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME        NULL,
    [EditName]         VARCHAR (10)    CONSTRAINT [DF_Manpower_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME        NULL,
    CONSTRAINT [PK_Manpower] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [Year] ASC, [Month] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'Lines';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'直接人力', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'DirectManpower';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'直接間接人力比例', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'ManpowerRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總工作時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'TotalWorkhour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總產能', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'TotalCPU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'In-House費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'InHouse';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'營業成本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'OperatingCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'營業支出', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'OperatingExpense';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'租借費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'Rental';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總支出', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'TotalExpense';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'WorkDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'平均工時', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'AvgDayWorkhour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PPH', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'PPH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory Manpower (工廠人數)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'月', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'Month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'間接人力', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Manpower', @level2type = N'COLUMN', @level2name = N'ActiveManpower';

