CREATE TABLE [dbo].[MarkerReq_Detail] (
    [ukey]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [ID]            VARCHAR (13) CONSTRAINT [DF_MarkerReq_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13) CONSTRAINT [DF_MarkerReq_Detail_OrderID] DEFAULT ('') NOT NULL,
    [SizeRatio]     NVARCHAR(MAX) CONSTRAINT [DF_MarkerReq_Detail_SizeRatio] DEFAULT ('') NOT NULL,
    [MarkerName]    VARCHAR (20)  CONSTRAINT [DF_MarkerReq_Detail_MarkerName] DEFAULT ('') NOT NULL,
    [Layer]         NUMERIC (5)  CONSTRAINT [DF_MarkerReq_Detail_Layer] DEFAULT ((0)) NOT NULL,
    [FabricCombo]   VARCHAR (2)  CONSTRAINT [DF_MarkerReq_Detail_FabricCombo] DEFAULT ('') NOT NULL,
    [ReqQty]        NUMERIC (2)  CONSTRAINT [DF_MarkerReq_Detail_ReqQty] DEFAULT ((0)) NOT NULL,
    [ReleaseQty]    NUMERIC (2)  CONSTRAINT [DF_MarkerReq_Detail_ReleaseQty] DEFAULT ((0)) NULL,
    [ReleaseDate]   DATE         NULL,
    [MarkerNo]      VARCHAR (10) CONSTRAINT [DF_MarkerReq_Detail_MarkerNo] DEFAULT ('') NULL,
    [WorkOrderForOutputUkey] BIGINT       CONSTRAINT [DF_MarkerReq_Detail_WorkOrderForOutputUkey] DEFAULT ((0)) NULL,
    [CuttingWidth] VARCHAR(8) NULL DEFAULT (''), 
    [PatternPanel] VARCHAR(120) NULL DEFAULT (''), 
    [CutRef]       Varchar(10) CONSTRAINT [DF_MarkerReq_Detail_CutRef] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MarkerReq_Detail] PRIMARY KEY CLUSTERED ([ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bulk Marker Request Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸BreakDown', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'SizeRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'FabricCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'ReqQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'ReleaseQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'ReleaseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'MarkerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WorkOrderForOutput', @level0type = N'SCHEMA',@level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail', @level2type = N'COLUMN', @level2name = N'WorkOrderForOutputUkey';


GO
EXECUTE sp_addextendedproperty @name=N'MS_Description', @value=N'WorkOrderForOutput 裁次' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MarkerReq_Detail', @level2type=N'COLUMN',@level2name=N'CutRef';

