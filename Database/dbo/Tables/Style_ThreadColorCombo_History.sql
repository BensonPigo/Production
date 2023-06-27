
	CREATE TABLE [dbo].[Style_ThreadColorCombo_History] (
    [StyleUkey]           BIGINT         NOT NULL,
    [Thread_ComboID]      VARCHAR (10)   NOT NULL,
    [MachineTypeID]       VARCHAR (10)   NOT NULL,
    [SeamLength]          DECIMAL (8, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_SeamLength] DEFAULT ((0)) NOT NULL,
    [ConsPC]              DECIMAL (8, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_ConsPC] DEFAULT ((0)) NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_History_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_History_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [Ukey]                BIGINT         NOT NULL,
    [LockDate]            DATETIME       NOT NULL,
    [Category]            VARCHAR (1)    CONSTRAINT [DF_Style_ThreadColorCombo_History_Category] DEFAULT ('') NOT NULL,
    [TPDate]              DATE           NULL,
    [IETMSID_Thread]      VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_History_IETMSID_Thread] DEFAULT ('') NOT NULL,
    [IETMSVersion_Thread] VARCHAR (3)    CONSTRAINT [DF_Style_ThreadColorCombo_History_IETMSVersion_Thread] DEFAULT ('') NOT NULL,
    [Version]             VARCHAR (5)    CONSTRAINT [DF_Style_ThreadColorCombo_History_Version] DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [LockDate] ASC, [Thread_ComboID] ASC, [MachineTypeID] ASC)
);

