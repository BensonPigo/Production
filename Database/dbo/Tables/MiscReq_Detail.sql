CREATE TABLE [dbo].[MiscReq_Detail] (
    [ID]          VARCHAR (13)    CONSTRAINT [DF_MiscReq_Detail_ID] DEFAULT ('') NOT NULL,
    [MiscID]      VARCHAR (23)    CONSTRAINT [DF_MiscReq_Detail_MiscID] DEFAULT ('') NOT NULL,
    [Qty]         NUMERIC (9)     CONSTRAINT [DF_MiscReq_Detail_Qty] DEFAULT ((0)) NULL,
    [Price]       NUMERIC (12, 4) CONSTRAINT [DF_MiscReq_Detail_Price] DEFAULT ((0)) NULL,
    [Amount]      NUMERIC (12, 4) CONSTRAINT [DF_MiscReq_Detail_Amount] DEFAULT ((0)) NULL,
    [ProjectID]   VARCHAR (8)     CONSTRAINT [DF_MiscReq_Detail_ProjectID] DEFAULT ('') NULL,
    [Suppid]      VARCHAR (6)     CONSTRAINT [DF_MiscReq_Detail_Suppid] DEFAULT ('') NULL,
    [MiscBrandid] VARCHAR (10)    CONSTRAINT [DF_MiscReq_Detail_MiscBrandid] DEFAULT ('') NULL,
    [MiscPOID]    VARCHAR (13)    CONSTRAINT [DF_MiscReq_Detail_MiscPOID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MiscReq_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [MiscID] ASC, [MiscPOID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous Requisition  Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'雜項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'MiscID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'專案號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'ProjectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'Suppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'MiscBrandid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscReq_Detail', @level2type = N'COLUMN', @level2name = N'MiscPOID';

