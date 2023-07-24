CREATE TABLE [dbo].[Style_ThreadColorCombo_Operation] (
    [Style_ThreadColorComboUkey] BIGINT         NOT NULL,
    [Seq]                        VARCHAR (4)    NOT NULL,
    [OperationID]                VARCHAR (20)   NOT NULL,
    [ComboType]                  VARCHAR (1)    CONSTRAINT [DF_Style_ThreadColorCombo_Operation_ComboType] DEFAULT ('') NOT NULL,
    [Frequency]                  DECIMAL (7, 2) CONSTRAINT [DF_Style_ThreadColorCombo_Operation_Frequency] DEFAULT ((0)) NOT NULL,
    [AddName]                    VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_Operation_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                    DATETIME       NULL,
    [EditName]                   VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_Operation_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                   DATETIME       NULL,
    [Ukey]                       BIGINT         NOT NULL,
    PRIMARY KEY CLUSTERED ([Style_ThreadColorComboUkey] ASC, [Seq] ASC, [OperationID] ASC)
);


GO


