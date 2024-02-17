CREATE TABLE [dbo].[MaterialDocument_Brand](
[DocumentName] [varchar](100) NOT NULL,
[BrandID] [varchar](8) NOT NULL,
[MergedBrand] [varchar](8) NOT NULL,
CONSTRAINT [PK_MaterialDocument_Brand] PRIMARY KEY CLUSTERED 
(
[DocumentName] ASC,
[BrandID] ASC,
[MergedBrand] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO