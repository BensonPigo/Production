CREATE TABLE [dbo].[Express_Detail] (
    [ID]            VARCHAR (13)    CONSTRAINT [DF_Express_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13)    CONSTRAINT [DF_Express_Detail_OrderID] DEFAULT ('') NOT NULL,
    [Seq1]          VARCHAR (3)     CONSTRAINT [DF_Express_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]          VARCHAR (2)     CONSTRAINT [DF_Express_Detail_Seq2] DEFAULT ('') NOT NULL,
    [SeasonID]      VARCHAR (10)    CONSTRAINT [DF_Express_Detail_SeasonID] DEFAULT ('') NULL,
    [StyleID]       VARCHAR (15)    CONSTRAINT [DF_Express_Detail_StyleID] DEFAULT ('') NULL,
    [Description]   NVARCHAR (MAX)  CONSTRAINT [DF_Express_Detail_Description] DEFAULT ('') NULL,
    [SuppID]        VARCHAR (6)     CONSTRAINT [DF_Express_Detail_SuppID] DEFAULT ('') NULL,
    [CTNNo]         VARCHAR (10)    CONSTRAINT [DF_Express_Detail_CTNNo] DEFAULT ('') NOT NULL,
    [NW]            NUMERIC (9, 3)  CONSTRAINT [DF_Express_Detail_NW] DEFAULT ((0)) NULL,
    [Price]         NUMERIC (10, 4) CONSTRAINT [DF_Express_Detail_Price] DEFAULT ((0)) NULL,
    [Qty]           NUMERIC (8, 2)  CONSTRAINT [DF_Express_Detail_Qty] DEFAULT ((0)) NULL,
    [UnitID]        VARCHAR (8)     CONSTRAINT [DF_Express_Detail_UnitID] DEFAULT ('') NULL,
    [Category]      VARCHAR (1)     CONSTRAINT [DF_Express_Detail_Category] DEFAULT ('') NOT NULL,
    [DutyNo]        VARCHAR (13)    CONSTRAINT [DF_Express_Detail_DutyNo] DEFAULT ('') NULL,
    [InCharge]      VARCHAR (10)    CONSTRAINT [DF_Express_Detail_InCharge] DEFAULT ('') NULL,
    [Receiver]      NVARCHAR (20)   CONSTRAINT [DF_Express_Detail_Receiver] DEFAULT ('') NULL,
    [Leader]        VARCHAR (10)    CONSTRAINT [DF_Express_Detail_Leader] DEFAULT ('') NULL,
    [BrandID]       VARCHAR (8)     CONSTRAINT [DF_Express_Detail_BrandID] DEFAULT ('') NULL,
    [Remark]        NVARCHAR (3000) CONSTRAINT [DF_Express_Detail_Remark] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_Express_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_Express_Detail_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME        NULL,
    [PackingListID] VARCHAR (13)    CONSTRAINT [DF__Express_D__Packi__089BAC90] DEFAULT ('') NULL,
    CONSTRAINT [PK_Express_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [OrderID] ASC, [Seq1] ASC, [Seq2] ASC, [CTNNo] ASC, [Category] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'International Express Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HC No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SP#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'CTNNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'NW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'責任歸屬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'異常單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'DutyNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'In Charge', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'InCharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Receiver';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Team Leader', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Leader';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';

