CREATE TABLE [dbo].[SMNotice_Detail] (
    [ID]               VARCHAR (10)   CONSTRAINT [DF_SMNotice_Detail_ID] DEFAULT ('') NOT NULL,
    [Type]             VARCHAR (1)    CONSTRAINT [DF_SMNotice_Detail_Type] DEFAULT ('') NOT NULL,
    [UseFor]           VARCHAR (1)    CONSTRAINT [DF_SMNotice_Detail_UseFor] DEFAULT ('') NOT NULL,
    [PhaseID]          VARCHAR (10)   CONSTRAINT [DF_SMNotice_Detail_PhaseID] DEFAULT ('') NOT NULL,
    [RequireDate]      DATE           NULL,
    [Apv2SampleTime]   DATETIME       NULL,
    [Apv2SampleHandle] VARCHAR (10)   CONSTRAINT [DF_SMNotice_Detail_Apv2SampleHandle] DEFAULT ('') NOT NULL,
    [ApvName]          VARCHAR (10)   CONSTRAINT [DF_SMNotice_Detail_ApvName] DEFAULT ('') NOT NULL,
    [ApvDate]          DATETIME       NULL,
    [Factory]          VARCHAR (8)    CONSTRAINT [DF_SMNotice_Detail_Factory] DEFAULT ('') NOT NULL,
    [IEConfirmMR]      VARCHAR (10)   CONSTRAINT [DF_SMNotice_Detail_IEConfirmMR] DEFAULT ('') NOT NULL,
    [PendingStatus]    BIT            CONSTRAINT [DF_SMNotice_Detail_PendingStatus] DEFAULT ((0)) NOT NULL,
    [BasicPattern]     NVARCHAR (100) CONSTRAINT [DF_SMNotice_Detail_BasicPattern] DEFAULT ('') NOT NULL,
    [Remark1]          NVARCHAR (200) CONSTRAINT [DF_SMNotice_Detail_Remark1] DEFAULT ('') NOT NULL,
    [Remark2]          NVARCHAR (200) CONSTRAINT [DF_SMNotice_Detail_Remark2] DEFAULT ('') NOT NULL,
    [AddName]          VARCHAR (10)   CONSTRAINT [DF_SMNotice_Detail_AddName] DEFAULT ('') NOT NULL,
    [AddDate]          DATETIME       NULL,
    [EditName]         VARCHAR (10)   CONSTRAINT [DF_SMNotice_Detail_EditName] DEFAULT ('') NOT NULL,
    [EditDate]         DATETIME       NULL,
    CONSTRAINT [PK_SMNotice_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Type] ASC)
);





GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'申請單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'申請項目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'用途(Bulk/Costing)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'UseFor'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'樣別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'PhaseID'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'希望完成日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'RequireDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'Approve to Sample Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Apv2SampleTime'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'Appprove to Sample Handler' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Apv2SampleHandle'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'審核人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'ApvName'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'審核日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'ApvDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'指定工廠' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Factory'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'允許核准IEGSD' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'IEConfirmMR'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'Pending Status' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'PendingStatus'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'基本版' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'BasicPattern'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'進單備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Remark1'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'進單備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'Remark2'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'AddName'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'AddDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'EditName'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail', @level2type=N'COLUMN',@level2name=N'EditDate'
GO
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'樣衣製作申請_明細檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SMNotice_Detail'
GO