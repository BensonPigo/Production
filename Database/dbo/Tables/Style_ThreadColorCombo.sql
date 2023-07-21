CREATE TABLE [dbo].[Style_ThreadColorCombo] (
    [StyleUkey]      BIGINT         NOT NULL,
    [Thread_ComboID] VARCHAR (10)   NOT NULL,
    [MachineTypeID]  VARCHAR (10)   NOT NULL,
    [SeamLength]     DECIMAL (8, 2) CONSTRAINT [DF_Style_ThreadColorCombo_SeamLength] DEFAULT ((0)) NOT NULL,
    [ConsPC]         DECIMAL (8, 2) CONSTRAINT [DF_Style_ThreadColorCombo_ConsPC] DEFAULT ((0)) NOT NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_AddName] DEFAULT ('') NOT NULL,
    [AddDate]        DATETIME       NULL,
    [EditName]       VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_EditName] DEFAULT ('') NOT NULL,
    [EditDate]       DATETIME       NULL,
    [Ukey]           BIGINT         NOT NULL,
    PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Thread_ComboID] ASC, [MachineTypeID] ASC)
);

