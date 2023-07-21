
CREATE TABLE [dbo].[Operation_His] (
    [Ukey]        BIGINT         NOT NULL,
    [Type]        VARCHAR (10)   CONSTRAINT [DF_Operation_His_Type] DEFAULT ('') NOT NULL,
    [TypeName]    NVARCHAR (40)  CONSTRAINT [DF_Operation_His_TypeName] DEFAULT ('') NOT NULL,
    [OperationID] VARCHAR (20)   CONSTRAINT [DF_Operation_His_OperationID] DEFAULT ('') NOT NULL,
    [OldValue]    NVARCHAR (200) CONSTRAINT [DF_Operation_His_OldValue] DEFAULT ('') NOT NULL,
    [NewValue]    NVARCHAR (200) CONSTRAINT [DF_Operation_His_NewValue] DEFAULT ('') NOT NULL,
    [Remark]      NVARCHAR (MAX) CONSTRAINT [DF_Operation_His_Remark] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME       NOT NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_Operation_His_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Operation_His] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UKEY' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分類代號完整名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'TypeName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'OperationID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'舊值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'OldValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'NewValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備誰' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operation 歷史檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Operation_His'
GO