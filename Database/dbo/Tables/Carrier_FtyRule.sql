CREATE TABLE [dbo].[Carrier_FtyRule] (
    [ID]          VARCHAR (4) CONSTRAINT [DF_Carrier_FtyRule_ID] DEFAULT ('') NULL,
    [FromCountry] VARCHAR (2) CONSTRAINT [DF_Carrier_FtyRule_FromCountry] DEFAULT ('') NOT NULL,
    [FromSite]    VARCHAR (8) CONSTRAINT [DF_Carrier_FtyRule_FromSite] DEFAULT ('') NOT NULL,
    [ToCountry]   VARCHAR (2) CONSTRAINT [DF_Carrier_FtyRule_ToCountry] DEFAULT ('') NOT NULL,
    [ToSite]      VARCHAR (8) CONSTRAINT [DF_Carrier_FtyRule_ToSite] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Carrier_FtyRule] PRIMARY KEY CLUSTERED ([FromCountry] ASC, [FromSite] ASC, [ToCountry] ASC, [ToSite] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'To Site', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_FtyRule', @level2type = N'COLUMN', @level2name = N'ToSite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'To Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_FtyRule', @level2type = N'COLUMN', @level2name = N'ToCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'From Site', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_FtyRule', @level2type = N'COLUMN', @level2name = N'FromSite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'From Country', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_FtyRule', @level2type = N'COLUMN', @level2name = N'FromCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_FtyRule', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Carrier_FtyRule';

