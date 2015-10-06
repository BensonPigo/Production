CREATE TABLE [dbo].[GMTBooking_CTNR] (
    [ID]       VARCHAR (25) CONSTRAINT [DF_GMTBooking_CTNR_ID] DEFAULT ('') NOT NULL,
    [Type]     VARCHAR (10) CONSTRAINT [DF_GMTBooking_CTNR_Type] DEFAULT ('') NULL,
    [CTNRNo]   VARCHAR (10) CONSTRAINT [DF_GMTBooking_CTNR_CTNRNo] DEFAULT ('') NOT NULL,
    [SealNo]   VARCHAR (10) CONSTRAINT [DF_GMTBooking_CTNR_SealNo] DEFAULT ('') NULL,
    [TruckNo]  VARCHAR (20) CONSTRAINT [DF_GMTBooking_CTNR_TruckNo] DEFAULT ('') NOT NULL,
    [AddName]  VARCHAR (10) CONSTRAINT [DF_GMTBooking_CTNR_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) CONSTRAINT [DF_GMTBooking_CTNR_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME     NULL,
    CONSTRAINT [PK_GMTBooking_CTNR] PRIMARY KEY CLUSTERED ([ID] ASC, [CTNRNo] ASC, [TruckNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Container與Truck#資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨櫃類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨櫃編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'CTNRNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'封條編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'SealNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'卡車車號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'TruckNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_CTNR', @level2type = N'COLUMN', @level2name = N'EditDate';

