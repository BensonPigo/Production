CREATE TABLE [dbo].[Order_Finish] (
    [ID]      VARCHAR (13) NOT NULL,
    [FOCQty]  INT          NOT NULL,
    [AddName] VARCHAR (10) NULL,
    [AddDate] DATETIME     NULL,
    [CurrentFOCQty] INT NOT NULL DEFAULT ((0)), 
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_Order_Finish] PRIMARY KEY CLUSTERED ([ID])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Finish', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Finish', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FOC入庫數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Finish', @level2type = N'COLUMN', @level2name = N'FOCQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_Finish', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'當前FOC庫存',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_Finish',
    @level2type = N'COLUMN',
    @level2name = N'CurrentFOCQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修改時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Order_Finish',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'