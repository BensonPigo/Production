CREATE TABLE [dbo].[Thread_AllowanceScale] (
    [ID]                             VARCHAR (3)    NOT NULL,
    [LowerBound]                     INT            NULL,
    [UpperBound]                     INT            NULL,
    [Allowance]                      NUMERIC (5, 2) NULL,
    [Remark]                         NVARCHAR (MAX) NULL,
    [AddName]                        VARCHAR (10)   NULL,
    [AddDate]                        DATETIME       NULL,
    [EditName]                       VARCHAR (10)   NULL,
    [EditDate]                       DATETIME       NULL,
    [Allowance_UserQtyBelowStandard] NUMERIC (5, 2) NULL,
    [Type]                           VARCHAR (1)    CONSTRAINT [DF_Thread_AllowanceScale_Type] DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC, [Type] ASC)
);

