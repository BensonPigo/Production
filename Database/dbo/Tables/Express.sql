CREATE TABLE [dbo].[Express] (
    [ID]                  VARCHAR (13)   CONSTRAINT [DF_Express_ID] DEFAULT ('') NOT NULL,
    [MDivisionID]         VARCHAR (8)    CONSTRAINT [DF_Express_MDivisionID] DEFAULT ('') NOT NULL,
    [ShipMark]            VARCHAR (20)   CONSTRAINT [DF_Express_ShipMark] DEFAULT ('') NOT NULL,
    [FromTag]             VARCHAR (1)    CONSTRAINT [DF_Express_FromTag] DEFAULT ('') NOT NULL,
    [FromSite]            VARCHAR (8)    CONSTRAINT [DF_Express_FromSite] DEFAULT ('') NOT NULL,
    [ToTag]               VARCHAR (1)    CONSTRAINT [DF_Express_ToTag] DEFAULT ('') NOT NULL,
    [ToSite]              VARCHAR (8)    CONSTRAINT [DF_Express_ToSite] DEFAULT ('') NOT NULL,
    [Dest]                VARCHAR (2)    CONSTRAINT [DF_Express_Dest] DEFAULT ('') NOT NULL,
    [PortAir]             VARCHAR (20)   CONSTRAINT [DF_Express_PortAir] DEFAULT ('') NOT NULL,
    [ShipDate]            DATE           NULL,
    [ETD]                 DATE           NULL,
    [ETA]                 DATE           NULL,
    [CTNQty]              SMALLINT       CONSTRAINT [DF_Express_CTNQty] DEFAULT ((0)) NOT NULL,
    [Handle]              VARCHAR (10)   CONSTRAINT [DF_Express_Handle] DEFAULT ('') NOT NULL,
    [Manager]             VARCHAR (10)   CONSTRAINT [DF_Express_Manager] DEFAULT ('') NOT NULL,
    [NW]                  DECIMAL (9, 3) CONSTRAINT [DF_Express_NW] DEFAULT ((0)) NOT NULL,
    [CTNNW]               DECIMAL (8, 2) CONSTRAINT [DF_Express_CTNNW] DEFAULT ((0)) NOT NULL,
    [VW]                  DECIMAL (8, 2) CONSTRAINT [DF_Express_VW] DEFAULT ((0)) NOT NULL,
    [CarrierID]           VARCHAR (4)    CONSTRAINT [DF_Express_CarrierID] DEFAULT ('') NOT NULL,
    [ExpressACNo]         VARCHAR (20)   CONSTRAINT [DF_Express_ExpressACNo] DEFAULT ('') NOT NULL,
    [BLNo]                VARCHAR (20)   CONSTRAINT [DF_Express_BLNo] DEFAULT ('') NOT NULL,
    [Remark]              NVARCHAR (100) CONSTRAINT [DF_Express_Remark] DEFAULT ('') NOT NULL,
    [FtyInvNo]            VARCHAR (25)   CONSTRAINT [DF_Express_FtyInvNo] DEFAULT ('') NOT NULL,
    [Status]              VARCHAR (15)   CONSTRAINT [DF_Express_Status] DEFAULT ('') NOT NULL,
    [StatusUpdateDate]    DATETIME       NULL,
    [SendDate]            DATETIME       NULL,
    [PayDate]             DATE           NULL,
    [CurrencyID]          VARCHAR (8)    CONSTRAINT [DF_Express_CurrencyID] DEFAULT ('') NOT NULL,
    [Amount]              DECIMAL (9, 2) CONSTRAINT [DF_Express_Amount] DEFAULT ((0)) NOT NULL,
    [InvNo]               VARCHAR (25)   CONSTRAINT [DF_Express_InvNo] DEFAULT ('') NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Express_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Express_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [FreightBy]           VARCHAR (4)    CONSTRAINT [DF_Express_FreightBy] DEFAULT ('') NOT NULL,
    [ByCustomerCarrier]   VARCHAR (15)   CONSTRAINT [DF_Express_ByCustomerCarrier] DEFAULT ('') NOT NULL,
    [ByCustomerAccountID] VARCHAR (15)   CONSTRAINT [DF_Express_ByCustomerAccountID] DEFAULT ('') NOT NULL,
    [ByFtyCarrier]        VARCHAR (8)    CONSTRAINT [DF_Express_ByFtyCarrier] DEFAULT ('') NOT NULL,
    [IsSpecialSending]    BIT            DEFAULT ((0)) NOT NULL,
    [Testing_Center]      BIT            CONSTRAINT [DF_Express_Testing_Center] DEFAULT ((0)) NOT NULL,
    [OrderCompanyID] NUMERIC(2) NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Express] PRIMARY KEY CLUSTERED ([ID] ASC)
);
















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'International Express', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HC No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨嘜頭', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ShipMark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'FromTag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨地點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'FromSite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ToTag';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達地點', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ToSite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'Dest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Port of Air', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'PortAir';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ShipDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ETD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ETD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'Manager';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'NW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所有空箱子重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'CTNNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'體積重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'VW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'快遞付款', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'CarrierID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'快遞帳號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ExpressACNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'BLNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'FtyInvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態最後更新日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'StatusUpdateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Send to SCI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'SendDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'PayDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Handle', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代碼(Payer為工廠)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ByFtyCarrier';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'快遞付款(Payer為客人)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ByCustomerCarrier';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'快遞付款會計科目(Payer為客人)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'ByCustomerAccountID';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'特殊寄件(在非常規的工作日寄出)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'IsSpecialSending';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HC歸屬(3RD,FTY,CUST,HAND)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Express', @level2type = N'COLUMN', @level2name = N'FreightBy';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'有無測試中心衍生的稅收',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Express',
    @level2type = N'COLUMN',
    @level2name = N'Testing_Center'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為訂單公司別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Express',
    @level2type = N'COLUMN',
    @level2name = N'OrderCompanyID'