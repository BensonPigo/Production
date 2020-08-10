CREATE TABLE [dbo].[Style_BOF] (
    [StyleUkey]         BIGINT         CONSTRAINT [DF_Style_BOF_StyleUkey] DEFAULT ((0)) NOT NULL,
    [FabricCode]        VARCHAR (3)    CONSTRAINT [DF_Style_BOF_FabricCode] DEFAULT ('') NOT NULL,
    [Refno]             VARCHAR (20)   CONSTRAINT [DF_Style_BOF_Refno] DEFAULT ('') NULL,
    [SCIRefno]          VARCHAR (30)   CONSTRAINT [DF_Style_BOF_SCIRefno] DEFAULT ('') NULL,
    [Kind]              VARCHAR (1)    CONSTRAINT [DF_Style_BOF_Kind] DEFAULT ('') NULL,
    [Ukey]              BIGINT         CONSTRAINT [DF_Style_BOF_Ukey] DEFAULT ((0)) NOT NULL,
    [SuppIDBulk]        VARCHAR (6)    CONSTRAINT [DF_Style_BOF_SuppIDBulk] DEFAULT ('') NULL,
    [SuppIDSample]      VARCHAR (6)    CONSTRAINT [DF_Style_BOF_SuppIDSample] DEFAULT ('') NULL,
    [ConsPc]            NUMERIC (8, 4) NULL,
    [MatchFabric]       VARCHAR (1)    NULL,
    [HRepeat]           NUMERIC (7, 4) NULL,
    [VRepeat]           NUMERIC (7, 4) NULL,
    [OneTwoWay]         VARCHAR (1)    NULL,
    [HorizontalCutting] BIT            NULL,
    [VRepeat_C]         NUMERIC (7, 4) NULL,
    CONSTRAINT [PK_Style_BOF_1] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客人料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用途', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF', @level2type = N'COLUMN', @level2name = N'Kind';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style - Bill of Fabric', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF', @level2type = N'COLUMN', @level2name = N'StyleUkey';

