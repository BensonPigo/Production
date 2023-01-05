CREATE TABLE [dbo].[AIR] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [POID]                VARCHAR (13)    CONSTRAINT [DF_AIR_POID] DEFAULT ('') NOT NULL,
    [SEQ1]                VARCHAR (3)     CONSTRAINT [DF_AIR_SEQ] DEFAULT ('') NOT NULL,
    [Suppid]              VARCHAR (6)     CONSTRAINT [DF_AIR_Suppid] DEFAULT ('') NOT NULL,
    [SCIRefno]            VARCHAR (30)    CONSTRAINT [DF_AIR_SCIRefno] DEFAULT ('') NOT NULL,
    [SEQ2]                VARCHAR (2)     CONSTRAINT [DF__tmp_ms_xx___Seq2__2568315E] DEFAULT ('') NULL,
    [Refno]               VARCHAR (36)    CONSTRAINT [DF_AIR_BrandRefno] DEFAULT ('') NOT NULL,
    [ArriveQty]           NUMERIC (11, 2) CONSTRAINT [DF_AIR_ArriveQty] DEFAULT ((0)) NULL,
    [InspQty]             NUMERIC (10, 2) CONSTRAINT [DF_AIR_InspQty] DEFAULT ((0)) NULL,
    [RejectQty]           NUMERIC (10, 2) CONSTRAINT [DF_AIR_RejectQty] DEFAULT ((0)) NULL,
    [Inspdeadline]        DATE            NULL,
    [InspDate]            DATE            NULL,
    [Inspector]           VARCHAR (10)    CONSTRAINT [DF_AIR_Inspector] DEFAULT ('') NULL,
    [Defect]              VARCHAR (60)    CONSTRAINT [DF_AIR_Defect] DEFAULT ('') NULL,
    [Result]              VARCHAR (10)     CONSTRAINT [DF_AIR_Result] DEFAULT ('') NULL,
    [Remark]              NVARCHAR (100)  CONSTRAINT [DF_AIR_Remark] DEFAULT ('') NULL,
    [ReplacementReportID] VARCHAR (13)    CONSTRAINT [DF_AIR_ReplacementReportID] DEFAULT ('') NULL,
    [ReceivingID]         VARCHAR (13)    CONSTRAINT [DF_AIR_ReceivingID] DEFAULT ('') NOT NULL,
    [Status]              VARCHAR (15)    CONSTRAINT [DF_AIR_Status] DEFAULT ('') NULL,
    [AddName]             VARCHAR (10)    CONSTRAINT [DF_AIR_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME        NULL,
    [EditName]            VARCHAR (10)    CONSTRAINT [DF_AIR_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME        NULL,
    [OldFabricUkey] VARCHAR(10) NULL DEFAULT (''), 
    [OldFabricVer] VARCHAR(2) NULL DEFAULT (''), 
    CONSTRAINT [PK_AIR] PRIMARY KEY CLUSTERED ([ID] ASC, [ReceivingID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Accessory Inspection Report', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Suppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'ArriveQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'InspQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'有問題數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'RejectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計檢驗日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Inspdeadline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'InspDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Defect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'ReplacementReportID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'ReceivingID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand Refno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR', @level2type = N'COLUMN', @level2name = N'Refno';


GO
CREATE NONCLUSTERED INDEX [PO_Seq]
    ON [dbo].[AIR]([POID] ASC, [SEQ1] ASC, [SEQ2] ASC);

