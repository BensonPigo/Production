CREATE TABLE [dbo].[ChgOver] (
    [ID]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrderID]         VARCHAR (13)   CONSTRAINT [DF_ChgOver_OrderID] DEFAULT ('') NULL,
    [ComboType]       VARCHAR (1)    CONSTRAINT [DF_ChgOver_ComboType] DEFAULT ('') NULL,
    [FactoryID]       VARCHAR (8)    CONSTRAINT [DF_ChgOver_FactoryID] DEFAULT ('') NULL,
    [StyleID]         VARCHAR (15)   CONSTRAINT [DF_ChgOver_StyleID] DEFAULT ('') NULL,
    [SeasonID]        VARCHAR (10)   CONSTRAINT [DF_ChgOver_SeasonID] DEFAULT ('') NULL,
    [SewingLineID]    VARCHAR (5)    CONSTRAINT [DF_ChgOver_SewingLineID] DEFAULT ('') NULL,
    [CDCodeID]        VARCHAR (6)    CONSTRAINT [DF_ChgOver_CDCodeID] DEFAULT ('') NULL,
    [Inline]          DATETIME       NULL,
    [TotalSewingTime] INT            CONSTRAINT [DF_ChgOver_TotalSewingTime] DEFAULT ((0)) NULL,
    [AlloQty]         INT            CONSTRAINT [DF_ChgOver_AlloQty] DEFAULT ((0)) NULL,
    [StandardOutput]  INT            CONSTRAINT [DF_ChgOver_StandardOutput] DEFAULT ((0)) NULL,
    [Category]        VARCHAR (1)    CONSTRAINT [DF_ChgOver_Category] DEFAULT ('') NULL,
    [COPT]            NUMERIC (8, 2) CONSTRAINT [DF_ChgOver_COPT] DEFAULT ((0)) NULL,
    [COT]             NUMERIC (8, 2) CONSTRAINT [DF_ChgOver_COT] DEFAULT ((0)) NULL,
    [FirstOutputTime] VARCHAR (4)    CONSTRAINT [DF_ChgOver_FirstOutputTime] DEFAULT ('') NULL,
    [LastOutputTime]  VARCHAR (4)    CONSTRAINT [DF_ChgOver_LastOutputTime] DEFAULT ('') NULL,
    [Remark]          NVARCHAR (60)  CONSTRAINT [DF_ChgOver_Remark] DEFAULT ('') NULL,
    [Type]            VARCHAR (1)    CONSTRAINT [DF_ChgOver_Type] DEFAULT ('') NULL,
    [ApvDate]         DATETIME       NULL,
    [APSNo]           INT            CONSTRAINT [DF_ChgOver_APSSewID] DEFAULT ((0)) NULL,
    [Status]          VARCHAR (15)   CONSTRAINT [DF_ChgOver_Status] DEFAULT ('') NULL,
    [MDivisionID]     VARCHAR (8)    CONSTRAINT [DF_ChgOver_MDivisionID] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_ChgOver_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_ChgOver_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    CONSTRAINT [PK_ChgOver] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SP No.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'套裝類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CD Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'CDCodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'Inline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單件生產所需時數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'TotalSewingTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'AlloQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每小時標轉產出', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'StandardOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Category', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'Category';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'COPT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'COPT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'COT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'COT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time of First Good Output', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'FirstOutputTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time of Last Good Output', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'LastOutputTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Remark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'Remark';


GO



GO



GO



GO



GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'New/Repeat', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS系統Planning Board的ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'APSNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver', @level2type = N'COLUMN', @level2name = N'MDivisionID';

