CREATE TABLE [dbo].[FIR_Grade] (
    [WeaveTypeID]      VARCHAR (20)  CONSTRAINT [DF_FIR_Grade_WeaveType] DEFAULT ('') NOT NULL,
    [Percentage]       NUMERIC (3)   CONSTRAINT [DF_FIR_Grade_Percentage] DEFAULT ((0)) NOT NULL,
    [Grade]            VARCHAR (10)  CONSTRAINT [DF_FIR_Grade_Grade] DEFAULT ('') NOT NULL,
    [Result]           VARCHAR (1)   CONSTRAINT [DF_FIR_Grade_Result] DEFAULT ('') NOT NULL,
    [BrandID]          VARCHAR (8)   DEFAULT ('') NOT NULL,
    [InspectionGroup]  VARCHAR (1)   CONSTRAINT [DF_FIR_Grade_InspectionGroup] DEFAULT ('') NOT NULL,
    [isFormatInP01]    BIT           CONSTRAINT [DF_FIR_Grade_isFormatInP01] DEFAULT ((0)) NOT NULL,
    [isResultNotInP01] BIT           CONSTRAINT [DF_FIR_Grade_isResultNotInP01] DEFAULT ((0)) NOT NULL,
    [Description]      VARCHAR (300) CONSTRAINT [DF_FIR_Grade_Description] DEFAULT ('') NOT NULL,
    [ShowGrade]        VARCHAR (10)  CONSTRAINT [DF_FIR_Grade_ShowGrade] DEFAULT ('') NOT NULL,
    [IsColorFormat] BIT CONSTRAINT [DF_FIR_Grade_IsColorFormat] DEFAULT ((0)) NOT NULL, 
    CONSTRAINT [PK_FIR_Grade] PRIMARY KEY CLUSTERED ([InspectionGroup] ASC, [Percentage] ASC, [BrandID] ASC, [WeaveTypeID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FIR 評比基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Grade';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'織法', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Grade', @level2type = N'COLUMN', @level2name = 'WeaveTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Percentage', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Grade', @level2type = N'COLUMN', @level2name = N'Percentage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'評等', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Grade', @level2type = N'COLUMN', @level2name = N'Grade';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Result', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Grade', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'變色判斷',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FIR_Grade',
    @level2type = N'COLUMN',
    @level2name = N'IsColorFormat'