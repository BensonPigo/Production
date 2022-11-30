CREATE TABLE [dbo].[Marker_ML_SizeCode] (
    [ID]         VARCHAR (10) CONSTRAINT [DF_Marker_ML_SizeCode_ID] DEFAULT ('') NOT NULL,
    [Version]    VARCHAR (3)  CONSTRAINT [DF_Marker_ML_SizeCode_Version] DEFAULT ('') NOT NULL,
    [MarkerUkey] BIGINT       CONSTRAINT [DF_Marker_ML_SizeCode_MarkerUkey] DEFAULT ((0)) NOT NULL,
    [MarkerName] VARCHAR (10) CONSTRAINT [DF_Marker_ML_SizeCode_MarkerName] DEFAULT ('') NOT NULL,
    [SizeCode]   VARCHAR (8)  CONSTRAINT [DF_Marker_ML_SizeCode_SizeCode] DEFAULT ('') NOT NULL,
    [Qty]        NUMERIC (4)  CONSTRAINT [DF_Marker_ML_SizeCode_Qty] DEFAULT ((0)) NOT NULL,
    [UKey_Old]   VARCHAR (10) CONSTRAINT [DF_Marker_ML_SizeCode_UKey_Old] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Marker_ML_SizeCode] PRIMARY KEY CLUSTERED ([ID] ASC, [Version] ASC, [MarkerName] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKEY_OLD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML_SizeCode', @level2type = N'COLUMN', @level2name = N'UKey_Old';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML_SizeCode', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML_SizeCode', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML_SizeCode', @level2type = N'COLUMN', @level2name = N'MarkerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Marker Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML_SizeCode', @level2type = N'COLUMN', @level2name = N'MarkerUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMNotice ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_ML_SizeCode', @level2type = N'COLUMN', @level2name = N'ID';

