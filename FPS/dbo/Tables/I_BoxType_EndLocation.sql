CREATE TABLE [dbo].[I_BoxType_EndLocation] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [BoxNum]       NVARCHAR (50)  NOT NULL,
    [Location]     INT            NOT NULL,
    [DG_CreatedAt] DATETIME       NOT NULL,
    [DG_UpataedAt] DATETIME       NULL,
    [DG_Status]    INT            NOT NULL,
    [WMS_Status]   INT            NOT NULL,
    [Sus_Status]   INT            NOT NULL,
    [WMS_Updated]  DATETIME       NULL,
    [WMS_Error]    NVARCHAR (255) NULL,
    [DG_Error]     NVARCHAR (255) NULL,
    [Str1]         NVARCHAR (255) NULL,
    [Str2]         NVARCHAR (255) NULL,
    [Str3]         NVARCHAR (255) NULL,
    [Str4]         NVARCHAR (255) NULL,
    [Str5]         NVARCHAR (255) NULL,
    [AllNum]       INT            CONSTRAINT [DF_I_BoxType_EndLocation_AllNum] DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

