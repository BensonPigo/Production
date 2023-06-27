CREATE TABLE [dbo].[Fabric_Supp] (
    [SCIRefno]      VARCHAR (30)   NOT NULL,
    [BrandID]       VARCHAR (8)    CONSTRAINT [DF_Fabric_Supp_BrandID] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (36)   CONSTRAINT [DF_Fabric_Supp_Refno] DEFAULT ('') NOT NULL,
    [SuppRefno]     VARCHAR (50)   CONSTRAINT [DF_Fabric_Supp_SuppRefno] DEFAULT ('') NOT NULL,
    [NOForecast]    BIT            CONSTRAINT [DF_Fabric_Supp_NOForecast] DEFAULT ((0)) NOT NULL,
    [IsECFA]        BIT            CONSTRAINT [DF_Fabric_Supp_IsECFA] DEFAULT ((0)) NOT NULL,
    [SuppID]        VARCHAR (6)    NOT NULL,
    [Lock]          BIT            CONSTRAINT [DF_Fabric_Supp_Lock] DEFAULT ((0)) NOT NULL,
    [SeasonID]      VARCHAR (10)   CONSTRAINT [DF_Fabric_Supp_SeasonID] DEFAULT ('') NOT NULL,
    [Remark]        NVARCHAR (MAX) CONSTRAINT [DF_Fabric_Supp_Remark] DEFAULT ('') NOT NULL,
    [POUnit]        VARCHAR (8)    NOT NULL,
    [Delay]         DATE           NULL,
    [DelayMemo]     NVARCHAR (MAX) CONSTRAINT [DF_Fabric_Supp_DelayMemo] DEFAULT ('') NOT NULL,
    [ShowSuppColor] BIT            CONSTRAINT [DF_Fabric_Supp_ShowSuppColor] DEFAULT ((0)) NOT NULL,
    [ItemType]      VARCHAR (1)    CONSTRAINT [DF_Fabric_Supp_ItemType] DEFAULT ('') NOT NULL,
    [OrganicCotton] BIT            CONSTRAINT [DF_Fabric_Supp_OrganicCotton] DEFAULT ((0)) NOT NULL,
    [LTDay]         DECIMAL (1)    CONSTRAINT [DF_Fabric_Supp_LTDay] DEFAULT ((0)) NOT NULL,
    [AbbCH]         NVARCHAR (70)  CONSTRAINT [DF_Fabric_Supp_AbbCH] DEFAULT ('') NOT NULL,
    [AbbEN]         NVARCHAR (80)  CONSTRAINT [DF_Fabric_Supp_AbbEN] DEFAULT ('') NOT NULL,
    [AllowanceType] DECIMAL (1)    CONSTRAINT [DF_Fabric_Supp_AllowanceType] DEFAULT ((0)) NOT NULL,
    [AllowanceRate] DECIMAL (2)    CONSTRAINT [DF_Fabric_Supp_AllowanceRate] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_Fabric_Supp_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   CONSTRAINT [DF_Fabric_Supp_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME       NULL,
    [ukey]          BIGINT         NOT NULL,
    [PreShrink]     BIT            NULL,
    [Junk]          BIT            CONSTRAINT [DF_Fabric_Supp_Junk] DEFAULT ((0)) NOT NULL,
    [IsDefault]     BIT            CONSTRAINT [DF_Fabric_Supp_IsDefault] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Fabric_Supp] PRIMARY KEY CLUSTERED ([SCIRefno] ASC, [SuppID] ASC)
);










GO



GO



GO



GO



GO



GO



GO



GO



GO



GO



GO



GO



GO



GO


