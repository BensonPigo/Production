CREATE TABLE [dbo].[MiscIn_Detail] (
    [ID]           VARCHAR (13)   CONSTRAINT [DF_MiscIn_Detail_ID] DEFAULT ('') NOT NULL,
    [MiscPOID]     VARCHAR (13)   CONSTRAINT [DF_MiscIn_Detail_MiscPOID] DEFAULT ('') NOT NULL,
    [SEQ1]         VARCHAR (3)    CONSTRAINT [DF_MiscIn_Detail_SEQ1] DEFAULT ('') NOT NULL,
    [SEQ2]         VARCHAR (2)    CONSTRAINT [DF_MiscIn_Detail_SEQ2] DEFAULT ('') NOT NULL,
    [MiscID]       VARCHAR (23)   CONSTRAINT [DF_MiscIn_Detail_MiscID] DEFAULT ('') NOT NULL,
    [POQty]        NUMERIC (8, 2) CONSTRAINT [DF_MiscIn_Detail_POQty] DEFAULT ((0)) NULL,
    [POPrice]      NUMERIC (8, 2) CONSTRAINT [DF_MiscIn_Detail_POPrice] DEFAULT ((0)) NULL,
    [Qty]          NUMERIC (8, 2) CONSTRAINT [DF_MiscIn_Detail_Qty] DEFAULT ((0)) NULL,
    [OnRoad]       NUMERIC (8, 2) CONSTRAINT [DF_MiscIn_Detail_OnRoad] DEFAULT ((0)) NULL,
    [InspDeadline] DATE           NULL,
    [MiscReqID]    VARCHAR (13)   CONSTRAINT [DF_MiscIn_Detail_MiscReqID] DEFAULT ('') NOT NULL,
    [DepartmentID] VARCHAR (8)    CONSTRAINT [DF_MiscIn_Detail_DepartmentID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MiscIn_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [MiscPOID] ASC, [SEQ1] ASC, [SEQ2] ASC, [MiscID] ASC, [MiscReqID] ASC, [DepartmentID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous in coming', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'MiscPOID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'SEQ2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'雜項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'MiscID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'POQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'POPrice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'入庫數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'在途數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'OnRoad';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗Deadline', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'InspDeadline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需求單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'MiscReqID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MiscIn_Detail', @level2type = N'COLUMN', @level2name = N'DepartmentID';

