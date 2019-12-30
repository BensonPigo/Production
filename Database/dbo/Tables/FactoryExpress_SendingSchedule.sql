CREATE TABLE [dbo].[FactoryExpress_SendingSchedule] (
    [Seq]        INT          NOT NULL,
    [Country]    VARCHAR (2)  CONSTRAINT [DF_FactoryExpress_SendingSchedule_Country] DEFAULT ('') NOT NULL,
    [RegionCode] VARCHAR (3)  CONSTRAINT [DF_FactoryExpress_SendingSchedule_FactoryID] DEFAULT ('') NOT NULL,
    [ToID]       VARCHAR (2)  CONSTRAINT [DF_FactoryExpress_SendingSchedule_ToID] DEFAULT ('') NOT NULL,
    [ToAlias]    VARCHAR (30) CONSTRAINT [DF_FactoryExpress_SendingSchedule_ToAlias] DEFAULT ('') NOT NULL,
    [BeginDate]  DATE         NOT NULL,
    [SUN]        BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_SUN] DEFAULT ((0)) NOT NULL,
    [MON]        BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_MON] DEFAULT ((0)) NOT NULL,
    [TUE]        BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_TUE] DEFAULT ((0)) NOT NULL,
    [WED]        BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_WED] DEFAULT ((0)) NOT NULL,
    [THU]        BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_THU] DEFAULT ((0)) NOT NULL,
    [FRI]        BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_FRI] DEFAULT ((0)) NOT NULL,
    [SAT]        BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_SAT] DEFAULT ((0)) NOT NULL,
    [Junk]       BIT          CONSTRAINT [DF_FactoryExpress_SendingSchedule_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_FactoryExpress_SendingSchedule_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_FactoryExpress_SendingSchedule_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME     NULL,
    CONSTRAINT [PK_FactoryExpress_SendingSchedule] PRIMARY KEY CLUSTERED ([RegionCode] ASC, [ToID] ASC)
);

