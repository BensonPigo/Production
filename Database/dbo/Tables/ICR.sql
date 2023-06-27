﻿CREATE TABLE [dbo].[ICR] (
    [Id]                  VARCHAR (13)    DEFAULT ('') NOT NULL,
    [Responsible]         VARCHAR (1)     DEFAULT ('') NOT NULL,
    [Department]          NVARCHAR (40)   DEFAULT ('') NOT NULL,
    [OrderID]             VARCHAR (13)    DEFAULT ('') NOT NULL,
    [Status]              VARCHAR (15)    DEFAULT ('') NOT NULL,
    [StatusUpdate]        DATETIME        NULL,
    [Handle]              VARCHAR (10)    DEFAULT ('') NOT NULL,
    [SMR]                 VARCHAR (10)    DEFAULT ('') NOT NULL,
    [ReceiveHandle]       VARCHAR (10)    DEFAULT ('') NOT NULL,
    [ReceiveDate]         DATETIME        NULL,
    [CFMDate]             DATE            NULL,
    [CFMHandle]           VARCHAR (10)    CONSTRAINT [DF_ICR_CFMHandle] DEFAULT ('') NOT NULL,
    [DutyHandle]          VARCHAR (10)    DEFAULT ('') NOT NULL,
    [DutySMR]             VARCHAR (10)    DEFAULT ('') NOT NULL,
    [DutyManager]         VARCHAR (10)    DEFAULT ('') NOT NULL,
    [unpayable]           BIT             DEFAULT ((0)) NOT NULL,
    [Deadline]            DATE            NULL,
    [DutyStatus]          VARCHAR (15)    DEFAULT ('') NOT NULL,
    [DutyStatusUpdate]    DATETIME        NULL,
    [RMtlAmt]             NUMERIC (11, 2) DEFAULT ((0)) NOT NULL,
    [EstFreight]          NUMERIC (11, 2) DEFAULT ((0)) NOT NULL,
    [ActFreight]          NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [OtherAmt]            NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [RMtlAmtUSD]          NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [EstFreightUSD]       NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [ActFreightUSD]       NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [OtherAmtUSD]         NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [Exchange]            NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [IrregularPOCostID]   VARCHAR (5)     DEFAULT ('') NOT NULL,
    [Description]         NVARCHAR (MAX)  DEFAULT ('') NOT NULL,
    [Suggestion]          NVARCHAR (MAX)  DEFAULT ('') NOT NULL,
    [Remark]              NVARCHAR (MAX)  DEFAULT ('') NOT NULL,
    [AddName]             VARCHAR (10)    DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME        NULL,
    [EditName]            VARCHAR (10)    DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME        NULL,
    [VoucherID]           VARCHAR (16)    DEFAULT ('') NOT NULL,
    [VoucherDate]         DATE            NULL,
    [RespDeptConfirmDate] DATETIME        NULL,
    [RespDeptConfirmName] VARCHAR (10)    CONSTRAINT [DF_ICR_RespDeptConfirmName] DEFAULT ('') NOT NULL,
    [BulkFTY]             VARCHAR (8)     CONSTRAINT [DF_ICR_BulkFTY] DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當 ICR 責任歸屬為 Sample Room 時，ICR 會轉給 Sample Room 的工廠，且 Sample Room 的工廠可能與訂單的工廠不在同一個國家 / 系統中，因此新增 BulkFTY 用已確認該訂單所屬工廠。', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ICR', @level2type = N'COLUMN', @level2name = N'BulkFTY';

