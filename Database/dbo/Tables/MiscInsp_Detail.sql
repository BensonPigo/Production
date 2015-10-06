CREATE TABLE [dbo].[MiscInsp_Detail] (
    [ID]       VARCHAR (13)   CONSTRAINT [DF_MiscInsp_Detail_ID] DEFAULT ('') NOT NULL,
    [MiscPOID] VARCHAR (13)   CONSTRAINT [DF_MiscInsp_Detail_MiscPOID] DEFAULT ('') NOT NULL,
    [SEQ1]     VARCHAR (3)    CONSTRAINT [DF_MiscInsp_Detail_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]     VARCHAR (2)    CONSTRAINT [DF_MiscInsp_Detail_SEQ2] DEFAULT ('') NOT NULL,
    [MiscID]   VARCHAR (23)   CONSTRAINT [DF_MiscInsp_Detail_MiscID] DEFAULT ('') NOT NULL,
    [PassQty]  NUMERIC (8, 2) CONSTRAINT [DF_MiscInsp_Detail_PassQty] DEFAULT ((0)) NULL,
    [Remark]   NVARCHAR (MAX) CONSTRAINT [DF_MiscInsp_Detail_Remark] DEFAULT ('') NULL,
    CONSTRAINT [PK_MiscInsp_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [MiscPOID] ASC, [SEQ1] ASC, [SEQ2] ASC, [MiscID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inspect In-coming Miscellaneous', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail', @level2type = N'COLUMN', @level2name = N'MiscPOID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MISC 編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail', @level2type = N'COLUMN', @level2name = N'MiscID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pass 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail', @level2type = N'COLUMN', @level2name = N'PassQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscInsp_Detail', @level2type = N'COLUMN', @level2name = N'Remark';

