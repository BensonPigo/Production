CREATE TABLE [dbo].[Pass1] (
    [ID]            VARCHAR (10)   CONSTRAINT [DF_Pass1_ID] DEFAULT ('') NOT NULL,
    [Name]          NVARCHAR (30)  CONSTRAINT [DF_Pass1_Name] DEFAULT ('') NULL,
    [Password]      VARCHAR (10)   CONSTRAINT [DF_Pass1_Password] DEFAULT ('') NULL,
    [Position]      VARCHAR (20)   CONSTRAINT [DF_Pass1_Position] DEFAULT ('') NULL,
    [FKPass0]       BIGINT         CONSTRAINT [DF_Pass1_FKPass0] DEFAULT ((0)) NULL,
    [IsAdmin]       BIT            CONSTRAINT [DF_Pass1_IsAdmin] DEFAULT ((0)) NULL,
    [IsMIS]         BIT            CONSTRAINT [DF_Pass1_IsMIS] DEFAULT ((0)) NULL,
    [OrderGroup]    VARCHAR (2)    CONSTRAINT [DF_Pass1_OrderGroup] DEFAULT ('') NULL,
    [EMail]         VARCHAR (50)   CONSTRAINT [DF_Pass1_EMail] DEFAULT ('') NULL,
    [ExtNo]         VARCHAR (6)    CONSTRAINT [DF_Pass1_ExtNo] DEFAULT ('') NULL,
    [OnBoard]       DATETIME       NULL,
    [Resign]        DATETIME       NULL,
    [Supervisor]    VARCHAR (10)   CONSTRAINT [DF_Pass1_Supervisor] DEFAULT ('') NULL,
    [Manager]       VARCHAR (10)   CONSTRAINT [DF_Pass1_Manager] DEFAULT ('') NULL,
    [Deputy]        VARCHAR (10)   CONSTRAINT [DF_Pass1_Deputy] DEFAULT ('') NULL,
    [Factory]       VARCHAR (100)  CONSTRAINT [DF_Pass1_Factory] DEFAULT ('') NULL,
    [CodePage]      VARCHAR (6)    CONSTRAINT [DF_Pass1_CodePage] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_Pass1_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_Pass1_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME       NULL,
    [LastLoginTime] DATETIME       NULL,
    [ESignature]    NVARCHAR (60)  NULL,
    [Remark]        NVARCHAR (100) CONSTRAINT [DF_Pass1_Remark] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Pass1_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);









