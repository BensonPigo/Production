CREATE TABLE [dbo].[ThreadRequisition] (
    [OrderID]       VARCHAR (13) CONSTRAINT [DF_ThreadRequisition_OrderID] DEFAULT ('') NOT NULL,
    [MDivisionid]   VARCHAR (8)  CONSTRAINT [DF_ThreadRequisition_MDivisionid] DEFAULT ('') NOT NULL,
    [StyleID]       VARCHAR (15) CONSTRAINT [DF_ThreadRequisition_StyleID] DEFAULT ('') NOT NULL,
    [FactoryID]     VARCHAR (8)  CONSTRAINT [DF_ThreadRequisition_FactoryID] DEFAULT ('') NOT NULL,
    [SeasonID]      VARCHAR (10) CONSTRAINT [DF_ThreadRequisition_SeasonID] DEFAULT ('') NOT NULL,
    [BrandID]       VARCHAR (8)  CONSTRAINT [DF_ThreadRequisition_BrandID] DEFAULT ('') NOT NULL,
    [EstBookDate]   DATE         NOT NULL,
    [EstArriveDate] DATE         NOT NULL,
    [Status]        VARCHAR (15) CONSTRAINT [DF_ThreadRequisition_Status] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_ThreadRequisition_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_ThreadRequisition_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_ThreadRequisition] PRIMARY KEY CLUSTERED ([OrderID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Requisition 表頭', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計訂貨日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'EstBookDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計出貨日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'EstArriveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadRequisition', @level2type = N'COLUMN', @level2name = N'EditDate';

