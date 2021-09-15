CREATE TABLE [dbo].[GMTBooking_Detail] (
    [ID]       VARCHAR (25) CONSTRAINT [DF_GMTBooking_Detail_ID] DEFAULT ('') NOT NULL,
    [PLFromRgCode]     VARCHAR (3) CONSTRAINT [DF_GMTBooking_Detail_PLFromRgCode] DEFAULT ('') NOT NULL,
    [PackingListID]   VARCHAR (13) CONSTRAINT [DF_GMTBooking_Detail_PackingListID] DEFAULT ('') NOT NULL,
	[PulloutDate]   date  NULL
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GMTBooking_Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GB#主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_Detail', @level2type = N'COLUMN', @level2name = N'ID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表身資料從哪個DB來', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_Detail', @level2type = N'COLUMN', @level2name = N'PLFromRgCode';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝箱清單主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking_Detail', @level2type = N'COLUMN', @level2name = N'PackingListID';

