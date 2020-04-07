CREATE TABLE [dbo].[FIR_Grade] (
    [WeaveTypeID]  VARCHAR (20) CONSTRAINT [DF_FIR_Grade_WeaveType] DEFAULT ('') NOT NULL,
    [Percentage] NUMERIC (3)  CONSTRAINT [DF_FIR_Grade_Percentage] DEFAULT ((0)) NOT NULL,
    [Grade]      VARCHAR (1)  CONSTRAINT [DF_FIR_Grade_Grade] DEFAULT ('') NULL,
    [Result]     VARCHAR (1)  CONSTRAINT [DF_FIR_Grade_Result] DEFAULT ('') NULL,
	[BrandID]	Varchar(8) NOT NULL  DEFAULT(''),
    CONSTRAINT [PK_FIR_Grade] PRIMARY KEY CLUSTERED ([WeaveTypeID] ASC, [Percentage] ASC, [BrandID] ASC)
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

