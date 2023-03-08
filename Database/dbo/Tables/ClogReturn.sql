CREATE TABLE [dbo].[ClogReturn] (
    [ID]            BIGINT       IDENTITY (1, 1) NOT NULL,
    [ReturnDate]    DATE         NOT NULL,
    [MDivisionID]   VARCHAR (8)  CONSTRAINT [DF_ClogReturn_Detail_MDivisionID] DEFAULT ('') NOT NULL,
    [PackingListID] VARCHAR (13) CONSTRAINT [DF_ClogReturn_Detail_PackingListId] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13) CONSTRAINT [DF_ClogReturn_Detail_OrderId] DEFAULT ('') NOT NULL,
    [CTNStartNo]    VARCHAR (6)  CONSTRAINT [DF_ClogReturn_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [OldID]         VARCHAR (13) CONSTRAINT [DF_ClogReturn_Detail_OldID] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10) NULL,
    [CompleteTime]  DATETIME     NULL,
    [SCICtnNo] VARCHAR(16) CONSTRAINT [DF_ClogReturn_Detail_SCICtnNo] DEFAULT ('') NOT NULL, 
    [ClogReasonID] VARCHAR(5) CONSTRAINT [DF_ClogReturn_ClogReasonID] NOT NULL DEFAULT (''), 
    [ClogReasonRemark] NVARCHAR(200) CONSTRAINT [DF_ClogReturn_ClogReasonRemark] NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_ClogReturn_Detail_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton Return Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'AddDate';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'PackingListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'串到ClogReason.ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReturn',
    @level2type = N'COLUMN',
    @level2name = N'ClogReasonID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'紀錄Clog Return的原因備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ClogReturn',
    @level2type = N'COLUMN',
    @level2name = N'ClogReasonRemark'