CREATE TABLE [dbo].[MarkerReq_Detail_CutRef] (
    [MarkerReqDetailUkey] BIGINT       NOT NULL,
    [ID]                  VARCHAR (13) NOT NULL,
    [CutRef]              VARCHAR (10) NOT NULL,
    [MarkerName]          VARCHAR (20) CONSTRAINT [DF_MarkerReq_Detail_CutRef_MarkerName] DEFAULT ('') NOT NULL,
    [Layer]               NUMERIC (5)  CONSTRAINT [DF_MarkerReq_Detail_CutRef_Layer] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MarkerReq_Detail_CutRef] PRIMARY KEY CLUSTERED ([MarkerReqDetailUkey] ASC, [ID] ASC, [CutRef] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail_CutRef', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail_CutRef', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail_CutRef', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MarkerReq_Detail.ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail_CutRef', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MarkerReq_Detail.Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MarkerReq_Detail_CutRef', @level2type = N'COLUMN', @level2name = N'MarkerReqDetailUkey';

