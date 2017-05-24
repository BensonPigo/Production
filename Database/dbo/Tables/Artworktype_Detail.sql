CREATE TABLE [dbo].[Artworktype_Detail] (
    [ArtworktypeID] VARCHAR (20) CONSTRAINT [DF_Artworktype_Detail_ArtworktypeID] DEFAULT ('') NOT NULL,
    [MachineTypeID] VARCHAR (10) CONSTRAINT [DF_Artworktype_Detail_MachineTypeID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Artworktype_Detail] PRIMARY KEY CLUSTERED ([ArtworktypeID] ASC, [MachineTypeID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MachineTypeID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Artworktype_Detail', @level2type = N'COLUMN', @level2name = N'MachineTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ArtworktypeID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Artworktype_Detail', @level2type = N'COLUMN', @level2name = N'ArtworktypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Artworktype Special', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Artworktype_Detail';

