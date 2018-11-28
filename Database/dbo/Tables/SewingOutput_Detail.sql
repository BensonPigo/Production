CREATE TABLE [dbo].[SewingOutput_Detail] (
    [ID]                   VARCHAR (13)   CONSTRAINT [DF_SewingOutput_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderId]              VARCHAR (13)   CONSTRAINT [DF_SewingOutput_Detail_OrderId] DEFAULT ('') NOT NULL,
    [ComboType]            VARCHAR (1)    CONSTRAINT [DF_SewingOutput_Detail_ComboType] DEFAULT ('') NULL,
    [Article]              VARCHAR (8)    CONSTRAINT [DF_SewingOutput_Detail_Article] DEFAULT ('') NULL,
    [Color]                VARCHAR (6)    CONSTRAINT [DF_SewingOutput_Detail_Color] DEFAULT ('') NULL,
    [TMS]                  INT            CONSTRAINT [DF_SewingOutput_Detail_TMS] DEFAULT ((0)) NULL,
    [HourlyStandardOutput] INT            CONSTRAINT [DF_SewingOutput_Detail_HourlyStandardOutput] DEFAULT ((0)) NULL,
    [WorkHour]             NUMERIC (6, 3) CONSTRAINT [DF_SewingOutput_Detail_WorkHour] DEFAULT ((0)) NOT NULL,
    [UKey]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [QAQty]                INT            CONSTRAINT [DF_SewingOutput_Detail_QAQty] DEFAULT ((0)) NULL,
    [DefectQty]            INT            CONSTRAINT [DF_SewingOutput_Detail_DefectQty] DEFAULT ((0)) NULL,
    [InlineQty]            INT            CONSTRAINT [DF_SewingOutput_Detail_InlineQty] DEFAULT ((0)) NULL,
    [OldDetailKey]         VARCHAR (13)   CONSTRAINT [DF_SewingOutput_Detail_OldDetailKey] DEFAULT ('') NULL,
    [AutoCreate]           BIT            NULL,
    [SewingReasonID] VARCHAR(5) NOT NULL DEFAULT (''), 
    [Remark] NVARCHAR(1000) NULL DEFAULT (''), 
    CONSTRAINT [PK_SewingOutput_Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);














GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Dailiy output(車縫日報明細檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SewingOutput Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組合型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Costing TMS (sec)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'TMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每小時標準產出', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'HourlyStandardOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作時數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'WorkHour';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產出數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'QAQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'DefectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上線數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'InlineQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'OldDetailKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Detail Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail', @level2type = N'COLUMN', @level2name = N'UKey';


GO
CREATE NONCLUSTERED INDEX [IX_SewingOutput_Detail_OrderID]
    ON [dbo].[SewingOutput_Detail]([OrderId] ASC);


GO
CREATE NONCLUSTERED INDEX [id]
    ON [dbo].[SewingOutput_Detail]([ID] ASC);


GO
CREATE NONCLUSTERED INDEX [OrderID_ComboType]
    ON [dbo].[SewingOutput_Detail]([OrderId] ASC, [ComboType] ASC);

