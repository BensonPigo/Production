CREATE TABLE [dbo].[ThreadAllowanceScale] (
    [ID]         VARCHAR (3)    NOT NULL,
    [LowerBound] INT            CONSTRAINT [DF_ThreadAllowanceScale_LowerBound] DEFAULT ((0)) NOT NULL,
    [UpperBound] INT            CONSTRAINT [DF_ThreadAllowanceScale_UpperBound] DEFAULT ((0)) NOT NULL,
    [Allowance]  DECIMAL (5, 2) CONSTRAINT [DF_ThreadAllowanceScale_Allowance] DEFAULT ((0)) NOT NULL,
    [Remark]     NVARCHAR (MAX) CONSTRAINT [DF_ThreadAllowanceScale_Remark] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10)   CONSTRAINT [DF_ThreadAllowanceScale_AddName] DEFAULT ('') NOT NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   CONSTRAINT [DF_ThreadAllowanceScale_EditName] DEFAULT ('') NOT NULL,
    [EditDate]   DATETIME       NULL,
    CONSTRAINT [PK_ThreadAllowanceScale] PRIMARY KEY CLUSTERED ([ID] ASC)
);



