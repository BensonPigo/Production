CREATE TABLE [dbo].[MiscPO_Detail] (
    [ID]           VARCHAR (13)    CONSTRAINT [DF_MiscPO_Detail_ID] DEFAULT ('') NOT NULL,
    [SEQ1]         VARCHAR (3)     CONSTRAINT [DF_MiscPO_Detail_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]         VARCHAR (2)     CONSTRAINT [DF_MiscPO_Detail_SEQ2] DEFAULT ('') NOT NULL,
    [MiscID]       VARCHAR (23)    CONSTRAINT [DF_MiscPO_Detail_MiscID] DEFAULT ('') NOT NULL,
    [UnitID]       VARCHAR (8)     CONSTRAINT [DF_MiscPO_Detail_UnitID] DEFAULT ('') NULL,
    [Price]        NUMERIC (12, 4) CONSTRAINT [DF_MiscPO_Detail_Price] DEFAULT ((0)) NULL,
    [Qty]          NUMERIC (8, 2)  CONSTRAINT [DF_MiscPO_Detail_Qty] DEFAULT ((0)) NULL,
    [InQty]        NUMERIC (8, 2)  CONSTRAINT [DF_MiscPO_Detail_InQty] DEFAULT ((0)) NULL,
    [ApQty]        NUMERIC (8, 2)  CONSTRAINT [DF_MiscPO_Detail_ApQty] DEFAULT ((0)) NULL,
    [InspQty]      NUMERIC (8, 2)  CONSTRAINT [DF_MiscPO_Detail_InspQty] DEFAULT ((0)) NULL,
    [MiscBrandID]  VARCHAR (10)    CONSTRAINT [DF_MiscPO_Detail_MiscBrandID] DEFAULT ('') NULL,
    [Suppid]       VARCHAR (6)     CONSTRAINT [DF_MiscPO_Detail_Suppid] DEFAULT ('') NULL,
    [TPEPOID]      VARCHAR (13)    CONSTRAINT [DF_MiscPO_Detail_TPEPOID] DEFAULT ('') NULL,
    [Junk]         BIT             CONSTRAINT [DF_MiscPO_Detail_Junk] DEFAULT ((0)) NULL,
    [MiscReqID]    VARCHAR (13)    CONSTRAINT [DF_MiscPO_Detail_MiscReqID] DEFAULT ('') NOT NULL,
    [projectid]    VARCHAR (10)    CONSTRAINT [DF_MiscPO_Detail_projectid] DEFAULT ('') NULL,
    [DepartmentID] VARCHAR (8)     CONSTRAINT [DF_MiscPO_Detail_DepartmentID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MiscPO_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ1] ASC, [SEQ2] ASC, [MiscID] ASC, [MiscReqID] ASC, [DepartmentID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous Purchase Order(Detail)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'雜項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'MiscID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'InQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'ApQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'InspQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'MiscBrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'Suppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'TPEPOID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'MiscReqID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'專案', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'projectid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscPO_Detail', @level2type = N'COLUMN', @level2name = N'DepartmentID';

