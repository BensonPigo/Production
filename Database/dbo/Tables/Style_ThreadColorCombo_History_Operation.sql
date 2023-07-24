
	CREATE TABLE [dbo].[Style_ThreadColorCombo_History_Operation] (
    [Style_ThreadColorCombo_HistoryUkey] BIGINT         NOT NULL,
    [Seq]                                VARCHAR (4)    NOT NULL,
    [OperationID]                        VARCHAR (20)   NOT NULL,
    [ComboType]                          VARCHAR (1)    CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_ComboType] DEFAULT ('') NOT NULL,
    [Frequency]                          DECIMAL (7, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_Frequency] DEFAULT ((0)) NOT NULL,
    [AddName]                            VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                            DATETIME       NULL,
    [EditName]                           VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                           DATETIME       NULL,
    [Ukey]                               BIGINT         NOT NULL,
    [MachineTypeHem]                     BIT            CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_MachineTypeHem] DEFAULT ((0)) NOT NULL,
    [OperationHem]                       BIT            CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_OperationHem] DEFAULT ((0)) NOT NULL,
    [Tubular]                            BIT            CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_Tubular] DEFAULT ((0)) NOT NULL,
    [Segment]                            INT            CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_Segment] DEFAULT ((0)) NOT NULL,
    [SeamLength]                         DECIMAL (9, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_Operation_SeamLength] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Style_ThreadColorCombo_HistoryUkey] ASC, [Seq] ASC, [OperationID] ASC)
);

