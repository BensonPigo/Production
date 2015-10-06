CREATE TABLE [dbo].[Factory_WorkHour] (
    [ID]         VARCHAR (8) CONSTRAINT [DF_Factory_WorkHour_ID] DEFAULT ('') NOT NULL,
    [Year]       VARCHAR (4) CONSTRAINT [DF_Factory_WorkHour_Year] DEFAULT ('') NOT NULL,
    [Month]      VARCHAR (2) CONSTRAINT [DF_Factory_WorkHour_Month] DEFAULT ('') NOT NULL,
    [HalfMonth1] TINYINT     CONSTRAINT [DF_Factory_WorkHour_HalfMonth1] DEFAULT ((0)) NULL,
    [HalfMonth2] TINYINT     CONSTRAINT [DF_Factory_WorkHour_HalfMonth2] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Factory_WorkHour] PRIMARY KEY CLUSTERED ([ID] ASC, [Year] ASC, [Month] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠工作天數work hours', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_WorkHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_WorkHour', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_WorkHour', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_WorkHour', @level2type = N'COLUMN', @level2name = N'Month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上半月天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_WorkHour', @level2type = N'COLUMN', @level2name = N'HalfMonth1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'下半月天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_WorkHour', @level2type = N'COLUMN', @level2name = N'HalfMonth2';

