CREATE TABLE [dbo].[Lack_Detail] (
    [ID]              VARCHAR (13)    CONSTRAINT [DF_Lack_Detail_ID] DEFAULT ('') NOT NULL,
    [Seq1]            VARCHAR (3)     CONSTRAINT [DF_Lack_Detail_Seq] DEFAULT ('') NOT NULL,
    [Seq2]            VARCHAR (2)     CONSTRAINT [DF_Lack_Detail_Seq2] DEFAULT ('') NOT NULL,
    [WhseInQty]       NUMERIC (10, 2) CONSTRAINT [DF_Lack_Detail_WhseInQty] DEFAULT ((0)) NULL,
    [FTYLastRecvDate] DATE            NULL,
    [FTYInQty]        NUMERIC (10, 2) CONSTRAINT [DF_Lack_Detail_FTYInQty] DEFAULT ((0)) NULL,
    [RequestQty]      NUMERIC (10, 2) CONSTRAINT [DF_Lack_Detail_RequestQty] DEFAULT ((0)) NULL,
    [PPICReasonID]    VARCHAR (5)     CONSTRAINT [DF_Lack_Detail_PPICReasonID] DEFAULT ('') NOT NULL,
    [RejectQty]       INT             CONSTRAINT [DF_Lack_Detail_RejectQty] DEFAULT ((0)) NULL,
    [Process]         VARCHAR (30)    CONSTRAINT [DF_Lack_Detail_Process] DEFAULT ('') NOT NULL,
    [IssueQty]        NUMERIC (10, 2) CONSTRAINT [DF_Lack_Detail_IssueQty] DEFAULT ((0)) NULL,
    [Remark] NVARCHAR(60) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_Lack_Detail] PRIMARY KEY CLUSTERED ([ID], [Seq1], [Seq2], [PPICReasonID], [Process])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lacking & Replacement', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原倉庫收料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'WhseInQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠最後收料日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'FTYLastRecvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠使用數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'FTYInQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'RequestQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'PPICReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reject的數量(PCS)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'RejectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Process', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'Process';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack_Detail', @level2type = N'COLUMN', @level2name = N'IssueQty';

