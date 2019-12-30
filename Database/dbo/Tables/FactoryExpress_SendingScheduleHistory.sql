CREATE TABLE [dbo].[FactoryExpress_SendingScheduleHistory] (
    [Ukey]       BIGINT       NOT NULL,
    [Country]    VARCHAR (2)  CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_Country] DEFAULT ('') NOT NULL,
    [RegionCode] VARCHAR (3)  CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_FactoryID] DEFAULT ('') NOT NULL,
    [ToID]       VARCHAR (2)  CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_ToID] DEFAULT ('') NOT NULL,
    [ToAlias]    VARCHAR (30) CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_ToAlias] DEFAULT ('') NOT NULL,
    [BeginDate]  DATE         NOT NULL,
    [EndDate]    DATE         NULL,
    [SUN]        BIT          CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_SUN] DEFAULT ((0)) NOT NULL,
    [MON]        BIT          CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_MON] DEFAULT ((0)) NOT NULL,
    [TUE]        BIT          CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_TUE] DEFAULT ((0)) NOT NULL,
    [WED]        BIT          CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_WED] DEFAULT ((0)) NOT NULL,
    [THU]        BIT          CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_THU] DEFAULT ((0)) NOT NULL,
    [FRI]        BIT          CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_FRI] DEFAULT ((0)) NOT NULL,
    [SAT]        BIT          CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_SAT] DEFAULT ((0)) NOT NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_FactoryExpress_SendingScheduleHistory_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME     NULL,
    CONSTRAINT [PK_FactoryExpress_SendingScheduleHistory_1] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

