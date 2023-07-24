CREATE TABLE [dbo].[Color_SuppColor] (
    [ID]              VARCHAR (6)    CONSTRAINT [DF_Color_SuppColor_ID] DEFAULT ('') NOT NULL,
    [Ukey]            BIGINT         CONSTRAINT [DF_Color_SuppColor_Ukey] DEFAULT ((0)) NOT NULL,
    [BrandId]         VARCHAR (8)    CONSTRAINT [DF_Color_SuppColor_BrandId] DEFAULT ('') NOT NULL,
    [ColorUkey]       BIGINT         CONSTRAINT [DF_Color_SuppColor_ColorUkey] DEFAULT ((0)) NOT NULL,
    [SeasonID]        VARCHAR (10)   CONSTRAINT [DF_Color_SuppColor_SeasonID] DEFAULT ('') NOT NULL,
    [SuppID]          VARCHAR (6)    CONSTRAINT [DF_Color_SuppColor_SuppID] DEFAULT ('') NOT NULL,
    [SuppColor]       VARCHAR (30)   CONSTRAINT [DF_Color_SuppColor_SuppColor] DEFAULT ('') NOT NULL,
    [ProgramID]       VARCHAR (12)   CONSTRAINT [DF_Color_SuppColor_ProgramID] DEFAULT ('') NOT NULL,
    [StyleID]         VARCHAR (15)   CONSTRAINT [DF_Color_SuppColor_StyleID] DEFAULT ('') NOT NULL,
    [Refno]           VARCHAR (36)   CONSTRAINT [DF_Color_SuppColor_Refno] DEFAULT ('') NOT NULL,
    [Remark]          NVARCHAR (120) CONSTRAINT [DF_Color_SuppColor_Remark] DEFAULT ('') NOT NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_Color_SuppColor_AddName] DEFAULT ('') NOT NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_Color_SuppColor_EditName] DEFAULT ('') NOT NULL,
    [EditDate]        DATETIME       NULL,
    [SuppGroupFabric] VARCHAR (8)    NULL,
    [MtlTypeId]       VARCHAR (20)   NULL,
    CONSTRAINT [PK_Color_SuppColor] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO

CREATE INDEX [Boa_Expand] ON [dbo].[Color_SuppColor] ([ColorUkey],[SeasonID],[SuppID],[ProgramID],[StyleID],[Refno])
