CREATE TABLE [dbo].[Order_SpecDetail] (
    [ID]         VARCHAR (13)   CONSTRAINT [DF_Order_SpecDetail_ID] DEFAULT ('') NOT NULL,
    [Qty]        NUMERIC (5)    CONSTRAINT [DF_Order_SpecDetail_Qty] DEFAULT ((0)) NOT NULL,
    [Amount]     NUMERIC (8, 2) CONSTRAINT [DF_Order_SpecDetail_Amount] DEFAULT ((0)) NULL,
    [respons]    VARCHAR (1)    CONSTRAINT [DF_Order_SpecDetail_respons] DEFAULT ('') NOT NULL,
    [responsdep] VARCHAR (4)    CONSTRAINT [DF_Order_SpecDetail_responsdep] DEFAULT ('') NOT NULL,
    [incharge]   VARCHAR (10)   CONSTRAINT [DF_Order_SpecDetail_incharge] DEFAULT ('') NOT NULL,
    [no]         VARCHAR (30)   CONSTRAINT [DF_Order_SpecDetail_no] DEFAULT ('') NULL,
    [remark]     VARCHAR (60)   CONSTRAINT [DF_Order_SpecDetail_remark] DEFAULT ('') NULL,
    [addr]       VARCHAR (100)  CONSTRAINT [DF_Order_SpecDetail_addr] DEFAULT ('') NULL,
    [brokers]    VARCHAR (4)    CONSTRAINT [DF_Order_SpecDetail_brokers] DEFAULT ('') NULL,
    [belong]     VARCHAR (15)   CONSTRAINT [DF_Order_SpecDetail_belong] DEFAULT ('') NULL,
    [Ukey]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [AddName]    VARCHAR (10)   CONSTRAINT [DF_Order_SpecDetail_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   CONSTRAINT [DF_Order_SpecDetail_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME       NULL
);

