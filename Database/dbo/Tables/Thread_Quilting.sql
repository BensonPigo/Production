
CREATE TABLE [dbo].[Thread_Quilting] (
    [Shape]    VARCHAR (25)  NOT NULL,
    [Picture1] NVARCHAR (60) CONSTRAINT [DF_Thread_Quilting_Picture1] DEFAULT ('') NOT NULL,
    [Picture2] NVARCHAR (60) CONSTRAINT [DF_Thread_Quilting_Picture2] DEFAULT ('') NOT NULL,
    [Junk]     BIT           CONSTRAINT [DF_Thread_Quilting_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_Thread_Quilting_AddName] DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_Thread_Quilting_EditName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME      NULL,
    CONSTRAINT [PK_Thread_Quilting] PRIMARY KEY CLUSTERED ([Shape] ASC)
);


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Thread_Quilting', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

