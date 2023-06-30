
CREATE TABLE [dbo].[Style_QTThreadColorCombo] (
    [Ukey]                     BIGINT       NOT NULL,
    [StyleUkey]                BIGINT       NOT NULL,
    [Thread_Quilting_SizeUkey] BIGINT       NOT NULL,
    [FabricPanelCode]          VARCHAR (2)  NOT NULL,
    [AddName]                  VARCHAR (10) CONSTRAINT [DF_Style_QTThreadColorCombo_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                  DATETIME     NULL,
    [EditName]                 VARCHAR (10) CONSTRAINT [DF_Style_QTThreadColorCombo_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                 DATETIME     NULL,
    CONSTRAINT [PK_Style_QTThreadColorCombo] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Add Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Edit Date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Style_QTThreadColorCombo', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

