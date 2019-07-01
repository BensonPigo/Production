CREATE TABLE [dbo].[VNContractQtyAdjust] (
    [ID]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [CDate]        DATE          NULL,
    [VNContractID] VARCHAR (15)  CONSTRAINT [DF_VNContractQtyAdjust_ContractID] DEFAULT ('') NULL,
    [DeclareNo]    VARCHAR (25)  CONSTRAINT [DF_VNContractQtyAdjust_DeclareNo] DEFAULT ('') NULL,
    [Remark]       VARCHAR (150) CONSTRAINT [DF_VNContractQtyAdjust_Remark] DEFAULT ('') NULL,
    [Status]       VARCHAR (15)  CONSTRAINT [DF_VNContractQtyAdjust_Status] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)  CONSTRAINT [DF_VNContractQtyAdjust_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [EditName]     VARCHAR (10)  CONSTRAINT [DF_VNContractQtyAdjust_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    [ReasonID]     VARCHAR (5)   NULL,
    [WKNo]         VARCHAR (13)  NULL,
    CONSTRAINT [PK_VNContractQtyAdjust] PRIMARY KEY CLUSTERED ([ID] ASC)
);



