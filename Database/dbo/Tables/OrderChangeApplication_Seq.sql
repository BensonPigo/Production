﻿CREATE TABLE [dbo].[OrderChangeApplication_Seq] (
    [Ukey]           BIGINT         NOT NULL,
    [ID]             VARCHAR (13)   NOT NULL,
    [Seq]            VARCHAR (2)    NOT NULL,
    [NewSeq]         VARCHAR (2)    CONSTRAINT [DF_OrderChangeApplication_Seq_NewSeq] DEFAULT ('') NOT NULL,
    [ShipmodeID]     VARCHAR (10)   NOT NULL,
    [BuyerDelivery]  DATE           NOT NULL,
    [FtyKPI]         DATE           NULL,
    [ReasonID]       VARCHAR (5)    CONSTRAINT [DF_OrderChangeApplication_Seq_ReasonID] DEFAULT ('') NOT NULL,
    [ReasonRemark]   NVARCHAR (150) CONSTRAINT [DF_OrderChangeApplication_Seq_ReasonRemark] DEFAULT ('') NOT NULL,
    [ShipModeRemark] NVARCHAR (150) CONSTRAINT [DF_OrderChangeApplication_Seq_ShipModeRemark] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_OrderChangeApplication_Seq] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改客戶交期原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderChangeApplication_Seq', @level2type=N'COLUMN',@level2name=N'ReasonID'
GO