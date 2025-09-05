CREATE TABLE [dbo].[VNConsumption] (
    [ID]           VARCHAR (13)   NOT NULL,
    [CustomSP]     VARCHAR (12)    CONSTRAINT [DF_VNConsumption_CustomSP] DEFAULT ('') NOT NULL,
    [VNContractID] VARCHAR (15)   CONSTRAINT [DF_VNConsumption_VNContractID] DEFAULT ('') NOT NULL,
    [CDate]        DATE           NULL,
    [StyleID]      VARCHAR (15)   CONSTRAINT [DF_VNConsumption_StyleID] DEFAULT ('') NULL,
    [StyleUKey]    BIGINT         NOT NULL,
    [SeasonID]     VARCHAR (10)   CONSTRAINT [DF_VNConsumption_SeasonID] DEFAULT ('') NULL,
    [BrandID]      VARCHAR (8)    CONSTRAINT [DF_VNConsumption_BrandID] DEFAULT ('') NULL,
    [Category]     VARCHAR (1)    CONSTRAINT [DF_VNConsumption_Category] DEFAULT ('') NULL,
    [SizeCode]     VARCHAR (8)    CONSTRAINT [DF_VNConsumption_SizeCode] DEFAULT ('') NULL,
    [Qty]          INT            CONSTRAINT [DF_VNConsumption_Qty] DEFAULT ((0)) NULL,
    [PulloutQty]   INT            CONSTRAINT [DF_VNConsumption_PulloutQty] DEFAULT ((0)) NULL,
    [Version]      VARCHAR (3)    CONSTRAINT [DF_VNConsumption_Version] DEFAULT ('') NULL,
    [CPU]          NUMERIC (5, 3) CONSTRAINT [DF_VNConsumption_CPU] DEFAULT ((0)) NULL,
    [VNMultiple]   NUMERIC (4, 2) CONSTRAINT [DF_VNConsumption_Multiple] DEFAULT ((0)) NULL,
    [Status]       VARCHAR (15)   CONSTRAINT [DF_VNConsumption_Status] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_VNConsumption_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   NULL,
    [EditDate]     DATETIME       NULL,
    CONSTRAINT [PK_VNConsumption] PRIMARY KEY CLUSTERED ([CustomSP] ASC, [VNContractID] ASC)
);

