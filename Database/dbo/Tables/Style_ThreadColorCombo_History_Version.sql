		CREATE TABLE [dbo].[Style_ThreadColorCombo_History_Version] (
    [Ukey]                BIGINT       IDENTITY (1, 1) NOT NULL,
    [StyleUkey]           BIGINT       CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Version]             VARCHAR (5)  CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_Version] DEFAULT ('') NOT NULL,
    [UseRatioRule]        VARCHAR (1)  CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_UseRatioRule] DEFAULT ('') NOT NULL,
    [ThickFabricBulk]     BIT          CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_ThickFabricBulk] DEFAULT ((0)) NOT NULL,
    [FarmOutQuilting]     BIT          CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_FarmOutQuilting] DEFAULT ((0)) NOT NULL,
    [LockHandle]          VARCHAR (10) CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_LockHandle] DEFAULT ('') NOT NULL,
    [LockDate]            DATETIME     NULL,
    [Category]            VARCHAR (1)  CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_Category] DEFAULT ('') NOT NULL,
    [TPDate]              DATE         NULL,
    [IETMSID_Thread]      VARCHAR (10) CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_IETMSID_Thread] DEFAULT ('') NOT NULL,
    [IETMSVersion_Thread] VARCHAR (3)  CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_IETMSVersion_Thread] DEFAULT ('') NOT NULL,
    [AddName]             VARCHAR (10) CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME     NULL,
    [VersionCOO]          VARCHAR (2)  CONSTRAINT [DF_Style_ThreadColorCombo_History_Version_VersionCOO] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_ThreadColorCombo_History_Version] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


