CREATE TABLE [dbo].[Fabric_Supp] (
    [SCIRefno]      VARCHAR (30)   NOT NULL,
    [BrandID]       VARCHAR (8)    NULL,
    [Refno]         VARCHAR (20)   NULL,
    [SuppRefno]     VARCHAR (30)   NULL,
    [NOForecast]    BIT            NULL,
    [IsECFA]        BIT            NULL,
    [SuppID]        VARCHAR (6)    NOT NULL,
    [Lock]          BIT            NULL,
    [SeasonID]      VARCHAR (10)   NULL,
    [Remark]        NVARCHAR (MAX) NULL,
    [POUnit]        VARCHAR (8)    NOT NULL,
    [Delay]         DATE           NULL,
    [DelayMemo]     NVARCHAR (MAX) NULL,
    [ShowSuppColor] BIT            NULL,
    [ItemType]      VARCHAR (1)    NULL,
    [OrganicCotton] BIT            NULL,
    [LTDay]         NUMERIC (1)    NULL,
    [AbbCH]         NVARCHAR (70)  NULL,
    [AbbEN]         NVARCHAR (80)  NULL,
    [AllowanceType] NUMERIC (1)    NULL,
    [AllowanceRate] NUMERIC (2)    NULL,
    [AddName]       VARCHAR (10)   NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   NULL,
    [EditDate]      DATETIME       NULL,
    [OldSys_Ukey]   VARCHAR (10)   NULL,
    [OldSys_Ver]    VARCHAR (2)    NULL,
    [ukey]          BIGINT         NOT NULL,
    [Keyword]       VARCHAR (MAX)  NULL,
    [PreShrink]     BIT            NULL,
    [Junk]          BIT            NULL,
	[SustainableMateria] nvarchar(150) NULL,
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


