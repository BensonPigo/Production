CREATE TABLE [dbo].[Color_SuppColor] (
    [ID]        VARCHAR (6)   CONSTRAINT [DF_Color_SuppColor_ID] DEFAULT ('') NULL,
    [Ukey]      BIGINT        CONSTRAINT [DF_Color_SuppColor_Ukey] DEFAULT ((0)) NOT NULL,
    [BrandId]   VARCHAR (8)   CONSTRAINT [DF_Color_SuppColor_BrandId] DEFAULT ('') NULL,
    [ColorUkey] BIGINT        CONSTRAINT [DF_Color_SuppColor_ColorUkey] DEFAULT ((0)) NULL,
    [SeasonID]  VARCHAR (10)  CONSTRAINT [DF_Color_SuppColor_SeasonID] DEFAULT ('') NULL,
    [SuppID]    VARCHAR (6)   CONSTRAINT [DF_Color_SuppColor_SuppID] DEFAULT ('') NULL,
    [SuppColor] VARCHAR (30)  CONSTRAINT [DF_Color_SuppColor_SuppColor] DEFAULT ('') NULL,
    [ProgramID] VARCHAR (12)  CONSTRAINT [DF_Color_SuppColor_ProgramID] DEFAULT ('') NULL,
    [StyleID]   VARCHAR (15)  CONSTRAINT [DF_Color_SuppColor_StyleID] DEFAULT ('') NULL,
    [Refno]     VARCHAR (36)  CONSTRAINT [DF_Color_SuppColor_Refno] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (60) CONSTRAINT [DF_Color_SuppColor_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)  CONSTRAINT [DF_Color_SuppColor_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME      NULL,
    [EditName]  VARCHAR (10)  CONSTRAINT [DF_Color_SuppColor_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME      NULL,
    CONSTRAINT [PK_Color_SuppColor] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO

CREATE INDEX [Boa_Expand] ON [dbo].[Color_SuppColor] ([ColorUkey],[SeasonID],[SuppID],[ProgramID],[StyleID],[Refno])
