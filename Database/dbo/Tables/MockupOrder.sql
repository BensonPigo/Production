CREATE TABLE [dbo].[MockupOrder] (
    [ID]          VARCHAR (13)    CONSTRAINT [DF_MockupOrder_ID] DEFAULT ('') NOT NULL,
    [MockupID]    VARCHAR (15)    CONSTRAINT [DF_MockupOrder_MockupID] DEFAULT ('') NOT NULL,
    [Description] NVARCHAR (50)   CONSTRAINT [DF_MockupOrder_Description] DEFAULT ('') NOT NULL,
    [Cpu]         DECIMAL (5, 3)  CONSTRAINT [DF_MockupOrder_Cpu] DEFAULT ((0)) NOT NULL,
    [BrandID]     VARCHAR (8)     CONSTRAINT [DF_MockupOrder_BrandID] DEFAULT ('') NOT NULL,
    [StyleID]     VARCHAR (15)    CONSTRAINT [DF_MockupOrder_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]    VARCHAR (10)    CONSTRAINT [DF_MockupOrder_SeasonID] DEFAULT ('') NOT NULL,
    [ProgramID]   VARCHAR (12)    CONSTRAINT [DF_MockupOrder_ProgramID] DEFAULT ('') NOT NULL,
    [FactoryID]   VARCHAR (8)     CONSTRAINT [DF_MockupOrder_FactoryID] DEFAULT ('') NOT NULL,
    [Qty]         INT             CONSTRAINT [DF_MockupOrder_Qty] DEFAULT ((0)) NOT NULL,
    [CfmDate]     DATE            NULL,
    [SCIDelivery] DATE            NULL,
    [MRHandle]    VARCHAR (10)    CONSTRAINT [DF_MockupOrder_MRHandle] DEFAULT ('') NOT NULL,
    [SMR]         VARCHAR (10)    CONSTRAINT [DF_MockupOrder_SMR] DEFAULT ('') NOT NULL,
    [Junk]        BIT             CONSTRAINT [DF_MockupOrder_Junk] DEFAULT ((0)) NOT NULL,
    [Remark]      NVARCHAR (MAX)  CONSTRAINT [DF_MockupOrder_Remark] DEFAULT ('') NOT NULL,
    [CMPUnit]     VARCHAR (8)     CONSTRAINT [DF_MockupOrder_CMPUnit] DEFAULT ('') NOT NULL,
    [CMPPrice]    DECIMAL (16, 4) CONSTRAINT [DF_MockupOrder_CMPPrice] DEFAULT ((0)) NOT NULL,
    [FTYGroup]    VARCHAR (8)     CONSTRAINT [DF_MockupOrder_FTYGroup] DEFAULT ('') NOT NULL,
    [CPUFactor]   DECIMAL (3, 1)  CONSTRAINT [DF_MockupOrder_CPUFactor] DEFAULT ((0)) NOT NULL,
    [MDivisionID] VARCHAR (8)     CONSTRAINT [DF_MockupOrder_MDivisionID] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)    CONSTRAINT [DF_MockupOrder_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME        NULL,
    [EditName]    VARCHAR (10)    CONSTRAINT [DF_MockupOrder_EditName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME        NULL,
    CONSTRAINT [PK_MockupOrder] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mockup Order', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mocku', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'MockupID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mockup description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CPU', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'Cpu';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Season', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Program', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'ProgramID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'CfmDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'SCIDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'MRHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'SMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單是否drop掉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'cmp單位 - DZ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'CMPUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'cmp單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'CMPPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory Group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'FTYGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MockupOrder', @level2type = N'COLUMN', @level2name = N'MDivisionID';

