CREATE TABLE [dbo].[Order_PFHis] (
    [Id]             VARCHAR (13)   CONSTRAINT [DF_Order_PFHis_Id] DEFAULT ('') NULL,
    [NewSciDelivery] DATE           NULL,
    [OldSciDelivery] DATE           NULL,
    [LETA]           DATE           NULL,
    [Reamrk]         NVARCHAR (100) CONSTRAINT [DF_Order_PFHis_Reamrk] DEFAULT ('') NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_Order_PFHis_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME       NULL,
    [Ukey]           BIGINT         CONSTRAINT [DF_Order_PFHis_Ukey] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Order_PFHis] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pull Forward History', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'New SCI delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'NewSciDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Old SCI delivery', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'OldSciDelivery';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Last ETA', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'LETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'Reamrk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_PFHis', @level2type = N'COLUMN', @level2name = N'Ukey';

