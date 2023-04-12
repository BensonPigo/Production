CREATE TABLE [dbo].[SubProWorkTime_Detail]
(
	[SubProWOrkTimeID] BIGINT NOT NULL, 
    [SubProMachineID] VARCHAR(50) NOT NULL, 
    [Target] INT CONSTRAINT [DF_SubProWorkTime_Detail_Target] DEFAULT (0) NOT NULL, 
    CONSTRAINT [PK_SubProWorkTime_Detail] PRIMARY KEY CLUSTERED ([SubProWOrkTimeID], [SubProMachineID], [Target] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'目標數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'SubProWorkTime_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Target'