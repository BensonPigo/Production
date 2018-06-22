CREATE TABLE [dbo].[ThreadColorComb_operation] (
    [Id] BIGINT CONSTRAINT [DF_ThreadColorComb_operation_ThreadColorCombUkey] DEFAULT ('') NOT NULL,
    [Operationid]         VARCHAR (20) CONSTRAINT [DF_ThreadColorComb_operation_Operationid] DEFAULT ('') NOT NULL,
    [ComboType] VARCHAR(1) NOT NULL DEFAULT (''), 
    [Seq] VARCHAR(4) NOT NULL DEFAULT (''), 
    [Frequency] NUMERIC(7, 2) NULL DEFAULT ((0)), 
    CONSTRAINT [PK_ThreadColorComb_operation] PRIMARY KEY CLUSTERED ([Id] ASC, [Operationid] ASC,[ComboType] ASC ,[Seq] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Color Combination 部位工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_operation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ThreadColorCombUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_operation', @level2type = N'COLUMN', @level2name = 'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小工段名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_operation', @level2type = N'COLUMN', @level2name = N'Operationid';

