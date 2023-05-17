CREATE TABLE [dbo].[StyleFPSetting] (
    [StyleID]        VARCHAR (15) NULL,
    [SeasonID]       VARCHAR (10) NULL,
    [BrandID]        VARCHAR (8)  NULL,
    [CmdTime]        DATETIME     NULL,
    [SunriseUpdated] BIT          DEFAULT ((0)) NULL,
    [Pressing1]      INT          DEFAULT ((1)) NULL,
    [Pressing2]      INT          DEFAULT ((0)) NULL,
    [Folding1]       INT          DEFAULT ((0)) NULL,
    [Folding2]       INT          DEFAULT ((0)) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'折衣設定2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StyleFPSetting', @level2type = N'COLUMN', @level2name = N'Folding2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'折衣設定1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StyleFPSetting', @level2type = N'COLUMN', @level2name = N'Folding1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'整燙設定2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StyleFPSetting', @level2type = N'COLUMN', @level2name = N'Pressing2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'整燙設定1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StyleFPSetting', @level2type = N'COLUMN', @level2name = N'Pressing1';

