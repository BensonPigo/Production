CREATE TABLE [dbo].[ThreadColorComb_Detail] (
    [Id]               BIGINT       CONSTRAINT [DF_ThreadColorComb_Detail_ThreadColorCombUkey] DEFAULT ('') NOT NULL,
    [Machinetypeid]    VARCHAR (10) CONSTRAINT [DF_ThreadColorComb_Detail_Machinetypeid] DEFAULT ('') NOT NULL,
    [ThreadCombid]     VARCHAR (10) CONSTRAINT [DF_ThreadColorComb_Detail_ThreadComboid] DEFAULT ('') NOT NULL,
    [Refno]            VARCHAR (36) CONSTRAINT [DF_ThreadColorComb_Detail_Refno] DEFAULT ('') NOT NULL,
    [Article]          VARCHAR (8)  CONSTRAINT [DF_ThreadColorComb_Detail_Article] DEFAULT ('') NOT NULL,
    [ThreadColorid]    VARCHAR (15) CONSTRAINT [DF_ThreadColorComb_Detail_ThreadColorid] DEFAULT ('') NULL,
    [SEQ]              VARCHAR (2)  CONSTRAINT [DF_ThreadColorComb_Detail_SEQ] DEFAULT ('') NULL,
    [ThreadLocationID] VARCHAR (20) CONSTRAINT [DF_ThreadColorComb_Detail_ThreadLocationID] DEFAULT ('') NULL,
    [Ukey]             BIGINT       IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ThreadColorComb_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Color Combination Refno, Article,Clolor 配對', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ThreadColorCombUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = 'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器工段種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = N'Machinetypeid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線布種組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = 'ThreadCombid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = N'ThreadLocationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadColorComb_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';

