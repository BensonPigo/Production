CREATE TABLE [dbo].[ThreadInventory_Detail] (
    [ID]               VARCHAR (13) CONSTRAINT [DF_ThreadInventory_Detail_ID] DEFAULT ('') NOT NULL,
    [Refno]            VARCHAR (36) CONSTRAINT [DF_ThreadInventory_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID]    VARCHAR (15) CONSTRAINT [DF_ThreadInventory_Detail_ThreadColorID] DEFAULT ('') NOT NULL,
    [NewConeBook]      NUMERIC (5)  CONSTRAINT [DF_ThreadInventory_Detail_NewConeBook] DEFAULT ((0)) NULL,
    [UsedConeBook]     NUMERIC (5)  CONSTRAINT [DF_ThreadInventory_Detail_UsedConeBook] DEFAULT ((0)) NULL,
    [NewCone]          NUMERIC (5)  CONSTRAINT [DF_ThreadInventory_Detail_NewCone] DEFAULT ((0)) NULL,
    [UsedCone]         NUMERIC (5)  CONSTRAINT [DF_ThreadInventory_Detail_UsedCone] DEFAULT ((0)) NULL,
    [ThreadLocationID] VARCHAR (10) CONSTRAINT [DF_ThreadInventory_Detail_ThreadLocationID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ThreadInventory_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [ThreadColorID] ASC, [ThreadLocationID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread inventory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳面新完整Cone 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'NewConeBook';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳面非完整Cone 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'UsedConeBook';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際完整Cone 數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'NewCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際非完整Cone 數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'UsedCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadInventory_Detail', @level2type = N'COLUMN', @level2name = N'ThreadLocationID';

