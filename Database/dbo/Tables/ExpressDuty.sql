CREATE TABLE [dbo].[ExpressDuty]
(
    ID varchar(1) not null CONSTRAINT [PK_ExpressDuty] PRIMARY KEY,
    Name nvarchar(100) not null CONSTRAINT [CONSTRAINT_ExpressDuty_Name] DEFAULT (''),
    Description nvarchar(400) not null CONSTRAINT [CONSTRAINT_ExpressDuty_Description] DEFAULT (''),
    Remark nvarchar(1000) not null CONSTRAINT [CONSTRAINT_ExpressDuty_Remark] DEFAULT (''),
    Mail bit not null CONSTRAINT [CONSTRAINT_ExpressDuty_Mail] DEFAULT (0),
    Junk bit not null CONSTRAINT [CONSTRAINT_ExpressDuty_Junk] DEFAULT (0),
    IsTransferExport bit not null CONSTRAINT [CONSTRAINT_ExpressDuty_IsTransferExport] DEFAULT (0),
    NeedTaskTeamApprove bit not null CONSTRAINT [CONSTRAINT_ExpressDuty_NeedTaskTeamApprove] DEFAULT (0),
    AddName varchar(10) not null CONSTRAINT [CONSTRAINT_ExpressDuty_AddName] DEFAULT (''),
    AddDate datetime,
    EditName varchar(10) not null CONSTRAINT [CONSTRAINT_ExpressDuty_EditName] DEFAULT (''),
    EditDate datetime
)