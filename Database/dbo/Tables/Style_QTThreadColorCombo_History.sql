
CREATE TABLE [dbo].[Style_QTThreadColorCombo_History] (
    [Ukey]                     BIGINT         NOT NULL,
    [StyleUkey]                BIGINT         NOT NULL,
    [Thread_Quilting_SizeUkey] BIGINT         NOT NULL,
    [FabricPanelCode]          VARCHAR (2)    NOT NULL,
    [AddName]                  VARCHAR (10)   CONSTRAINT [DF_Style_QTThreadColorCombo_History_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                  DATETIME       NULL,
    [EditName]                 VARCHAR (10)   CONSTRAINT [DF_Style_QTThreadColorCombo_History_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                 DATETIME       NULL,
    [LockDate]                 DATETIME       NOT NULL,
    [HSize]                    NUMERIC (5, 2) CONSTRAINT [DF_Style_QTThreadColorCombo_History_HSize] DEFAULT ((0)) NOT NULL,
    [VSize]                    NUMERIC (5, 2) CONSTRAINT [DF_Style_QTThreadColorCombo_History_VSize] DEFAULT ((0)) NOT NULL,
    [ASize]                    NUMERIC (5, 2) CONSTRAINT [DF_Style_QTThreadColorCombo_History_ASize] DEFAULT ((0)) NOT NULL,
    [NeedleDistance]           NUMERIC (5, 2) CONSTRAINT [DF_Style_QTThreadColorCombo_History_NeedleDistance] DEFAULT ((0)) NOT NULL,
    [FabricCode]               VARCHAR (3)    NOT NULL,
    [SCIRefno]                 VARCHAR (30)   NOT NULL,
    [Width]                    NUMERIC (5, 2) NOT NULL,
    [Version]                  VARCHAR (5)    CONSTRAINT [DF_Style_QTThreadColorCombo_History_Version] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_QTThreadColorCombo_History] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Thread_Quilting_SizeUkey] ASC, [FabricPanelCode] ASC, [LockDate] ASC)
);


GO


GO


GO


GO


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo_History', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

