CREATE TABLE [dbo].[MachineType_Detail] (
    [ID]              VARCHAR (10) NOT NULL,
    [FactoryID]       VARCHAR (8)  NOT NULL,
    [IsSubprocess]    BIT          CONSTRAINT [DF_MachineType_Detail_IsSubprocess] DEFAULT ((0)) NOT NULL,
    [IsNonSewingLine] BIT          CONSTRAINT [DF_MachineType_Detail_IsNonSewingLine] DEFAULT ((0)) NOT NULL,
    [IsNotShownInP01] BIT          CONSTRAINT [DF_MachineType_Detail_IsNotShownInP01] DEFAULT ((0)) NOT NULL,
    [IsNotShownInP03] BIT          CONSTRAINT [DF_MachineType_Detail_IsNotShownInP03] DEFAULT ((0)) NOT NULL,
    [IsNotShownInP05] BIT          CONSTRAINT [DF_MachineType_Detail_IsNotShownInP05]     DEFAULT ((0)) NOT NULL, 
    [IsNotShownInP06] BIT          CONSTRAINT [DF_MachineType_Detail_IsNotShownInP06]     DEFAULT ((0)) NOT NULL, 
    CONSTRAINT [PK_MachineType_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'有勾選時，IE P03 Detail表身不顯示該operation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_Detail', @level2type = N'COLUMN', @level2name = N'IsNotShownInP03';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'有勾選時，IE P01Detail表身不顯示該operation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_Detail', @level2type = N'COLUMN', @level2name = N'IsNotShownInP01';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'有勾選時，IE P05 Detail表身不顯示該operation',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MachineType_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IsNotShownInP05'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'有勾選時，IE P06 Detail表身不顯示該operation',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MachineType_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IsNotShownInP06'