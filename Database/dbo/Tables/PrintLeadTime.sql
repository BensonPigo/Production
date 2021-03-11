CREATE TABLE [dbo].[PrintLeadTime] (
    [InkType]          VARCHAR (50)   NOT NULL,
    [SmallLogo]        BIT            NOT NULL,
    [RTLQtyLowerBound] INT            NOT NULL,
    [RTLQtyUpperBound] INT            NOT NULL,
    [LeadTime]         DECIMAL (5, 2) NOT NULL,
    [IsDefault]        BIT            NOT NULL,
    [ColorsLowerBound] INT            NOT NULL,
    [ColorsUpperBound] INT            NOT NULL,
    [AddLeadTime]      DECIMAL (5, 2) NOT NULL,
    CONSTRAINT [PK_PrintLeadTime] PRIMARY KEY CLUSTERED ([InkType] ASC, [SmallLogo] ASC, [RTLQtyLowerBound] ASC, [ColorsLowerBound] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'套用 print color 範圍後, 要增加的 LeadTime天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'AddLeadTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'print color 數對應上限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'ColorsUpperBound';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'print color 數對應下限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'ColorsLowerBound';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預設套用 InkType種類 (High Density)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'IsDefault';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'印製的 LeadTime 天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'LeadTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依據RLT Qty 要套用的上限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'RTLQtyUpperBound';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'依據RLT Qty 要套用的下限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'RTLQtyLowerBound';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Logo 是 small / big', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'SmallLogo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'印製類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PrintLeadTime', @level2type = N'COLUMN', @level2name = N'InkType';

