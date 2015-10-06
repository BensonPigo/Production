CREATE TABLE [dbo].[MiscAp_Detail] (
    [ID]           VARCHAR (13)    CONSTRAINT [DF_MiscAp_Detail_ID] DEFAULT ('') NOT NULL,
    [MiscPOID]     VARCHAR (13)    CONSTRAINT [DF_MiscAp_Detail_MiscPOID] DEFAULT ('') NOT NULL,
    [SEQ1]         VARCHAR (3)     CONSTRAINT [DF_MiscAp_Detail_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]         VARCHAR (2)     CONSTRAINT [DF_MiscAp_Detail_SEQ2] DEFAULT ('') NOT NULL,
    [MiscID]       VARCHAR (23)    CONSTRAINT [DF_MiscAp_Detail_MiscID] DEFAULT ('') NOT NULL,
    [Price]        NUMERIC (12, 4) CONSTRAINT [DF_MiscAp_Detail_Price] DEFAULT ((0)) NULL,
    [Qty]          NUMERIC (8, 2)  CONSTRAINT [DF_MiscAp_Detail_Qty] DEFAULT ((0)) NULL,
    [InQty]        NUMERIC (8, 2)  CONSTRAINT [DF_MiscAp_Detail_InQty] DEFAULT ((0)) NULL,
    [ApQty]        NUMERIC (8, 2)  CONSTRAINT [DF_MiscAp_Detail_ApQty] DEFAULT ((0)) NULL,
    [InspQty]      NUMERIC (8, 2)  CONSTRAINT [DF_MiscAp_Detail_InspQty] DEFAULT ((0)) NULL,
    [DepartmentID] VARCHAR (8)     CONSTRAINT [DF_MiscAp_Detail_DepartmentID] DEFAULT ('') NOT NULL,
    [MiscReqid]    VARCHAR (13)    CONSTRAINT [DF_MiscAp_Detail_MiscReqid] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MiscAp_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [MiscPOID] ASC, [SEQ1] ASC, [SEQ2] ASC, [MiscID] ASC, [DepartmentID] ASC, [MiscReqid] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous Account Payable(Local) Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'零件採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'MiscPOID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'零件編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'MiscID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'累計In Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'InQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'累計Ap Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'ApQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'累計檢驗 Qty', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'InspQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'DepartmentID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscAp_Detail', @level2type = N'COLUMN', @level2name = N'MiscReqid';

