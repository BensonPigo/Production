CREATE TABLE [dbo].[Style_ProductionKits] (
    [Ukey]                BIGINT         CONSTRAINT [DF_Style_ProductionKits_Ukey] DEFAULT ((0)) NOT NULL,
    [StyleUkey]           BIGINT         CONSTRAINT [DF_Style_ProductionKits_StyleUkey] DEFAULT ((0)) NOT NULL,
    [ProductionKitsGroup] VARCHAR (8)    CONSTRAINT [DF_Style_ProductionKits_ProductionKitsGroup] DEFAULT ('') NOT NULL,
    [MDivisionID]         VARCHAR (8)    CONSTRAINT [DF_Style_ProductionKits_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]           VARCHAR (8)    CONSTRAINT [DF_Style_ProductionKits_FactoryID] DEFAULT ('') NOT NULL,
    [Article]             NVARCHAR (MAX) CONSTRAINT [DF_Style_ProductionKits_Article] DEFAULT ('') NOT NULL,
    [DOC]                 VARCHAR (5)    CONSTRAINT [DF_Style_ProductionKits_DOC] DEFAULT ('') NOT NULL,
    [SendDate]            DATE           NULL,
    [ReceiveDate]         DATE           NULL,
    [ProvideDate]         DATE           NULL,
    [SendName]            VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_SendName] DEFAULT ('') NOT NULL,
    [FtyHandle]           VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_FtyHandle] DEFAULT ('') NOT NULL,
    [MRHandle]            VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_MRHandle] DEFAULT ('') NOT NULL,
    [SMR]                 VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_SMR] DEFAULT ('') NOT NULL,
    [PoHandle]            VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_PoHandle] DEFAULT ('') NOT NULL,
    [POSMR]               VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_POSMR] DEFAULT ('') NOT NULL,
    [OrderId]             VARCHAR (13)   CONSTRAINT [DF_Style_ProductionKits_OrderId] DEFAULT ('') NOT NULL,
    [SCIDelivery]         DATE           NULL,
    [IsPF]                BIT            CONSTRAINT [DF_Style_ProductionKits_IsPF] DEFAULT ((0)) NOT NULL,
    [BuyerDelivery]       DATE           NULL,
    [AddOrderId]          VARCHAR (13)   CONSTRAINT [DF_Style_ProductionKits_AddOrderId] DEFAULT ('') NOT NULL,
    [AddSCIDelivery]      DATE           NULL,
    [AddIsPF]             BIT            CONSTRAINT [DF_Style_ProductionKits_AddIsPF] DEFAULT ((0)) NOT NULL,
    [AddBuyerDelivery]    DATE           NULL,
    [MRLastDate]          DATETIME       NULL,
    [FtyLastDate]         DATETIME       NULL,
    [MRRemark]            NVARCHAR (MAX) CONSTRAINT [DF_Style_ProductionKits_MRRemark] DEFAULT ('') NOT NULL,
    [FtyRemark]           NVARCHAR (MAX) CONSTRAINT [DF_Style_ProductionKits_FtyRemark] DEFAULT ('') NOT NULL,
    [FtyList]             NVARCHAR (100) CONSTRAINT [DF_Style_ProductionKits_FtyList] DEFAULT ('') NOT NULL,
    [ReasonID]            VARCHAR (5)    CONSTRAINT [DF_Style_ProductionKits_ReasonID] DEFAULT ('') NOT NULL,
    [SendToQA]            DATE           NULL,
    [QAReceived]          DATE           NULL,
    [StyleCUkey1_Old]     VARCHAR (11)   CONSTRAINT [DF_Style_ProductionKits_StyleCUkey1_Old] DEFAULT ('') NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Style_ProductionKits_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [Reject]              BIT            CONSTRAINT [DF_Style_ProductionKits_Reject] DEFAULT ((0)) NOT NULL,
    [AWBNO]               VARCHAR (30)   CONSTRAINT [DF_Style_ProductionKits_AWBNO] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_ProductionKits] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production Kits', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'ProductionKitsGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄送文件代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'DOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'業務寄出日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'SendDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司規定寄出日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'ProvideDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Send date Approve 的人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'SendName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'FtyHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'業務負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'MRHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'業務主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'SMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'PoHandle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購主管', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'POSMR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI 交期最早的SP#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'SCIDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為Pull forward訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'IsPF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'BuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時的SP#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'AddOrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時的飛雁交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'AddSCIDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時是否為Pull forward 訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'AddIsPF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時的客戶交期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'AddBuyerDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MR最後修改日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'MRLastDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠最後更新日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'FtyLastDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'業務備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'MRRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'FtyRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄送工廠List (工廠Group)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'FtyList';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠MR PASS TO QA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'SendToQA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QA 收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'QAReceived';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'StyleCUkey1_Old';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'不寄送原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'ReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠紀錄是否有收到文件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ProductionKits', @level2type = N'COLUMN', @level2name = N'Reject';

