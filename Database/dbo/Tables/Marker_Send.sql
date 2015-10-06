CREATE TABLE [dbo].[Marker_Send] (
    [ID]             VARCHAR (10)   CONSTRAINT [DF_Marker_Send_ID] DEFAULT ('') NOT NULL,
    [SEQ]            VARCHAR (2)    CONSTRAINT [DF_Marker_Send_SEQ] DEFAULT ('') NOT NULL,
    [MarkerVersion]  VARCHAR (3)    CONSTRAINT [DF_Marker_Send_MarkerVersion] DEFAULT ('') NOT NULL,
    [MarkerNo]       VARCHAR (10)   CONSTRAINT [DF_Marker_Send_MarkerNo] DEFAULT ('') NOT NULL,
    [PatternSMID]    VARCHAR (10)   CONSTRAINT [DF_Marker_Send_PatternSMID] DEFAULT ('') NOT NULL,
    [PatternVersion] VARCHAR (3)    CONSTRAINT [DF_Marker_Send_PatternVersion] DEFAULT ('') NULL,
    [ToFactory]      NVARCHAR (100) CONSTRAINT [DF_Marker_Send_ToFactory] DEFAULT ('') NOT NULL,
    [TransLate]      BIT            CONSTRAINT [DF_Marker_Send_TransLate] DEFAULT ((0)) NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_Marker_Send_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME       NULL,
    CONSTRAINT [PK_Marker_Send] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ] ASC, [MarkerVersion] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克寄送記錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'MarkerVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'MarkerNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'PatternSMID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'打版版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'PatternVersion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'寄送工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'ToFactory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉換語系', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'TransLate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Marker_Send', @level2type = N'COLUMN', @level2name = N'AddDate';

