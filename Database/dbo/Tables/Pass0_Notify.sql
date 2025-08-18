CREATE TABLE [dbo].[Pass0_Notify](
	[Pass0_Ukey] [bigint] NOT NULL,
	[NotificationList_Ukey] [bigint] NOT NULL,
	[SystemNotify] [bit] NOT NULL,
	[AddName] [varchar](10) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_Pass0_Notify] PRIMARY KEY CLUSTERED 
(
	[Pass0_Ukey] ASC,
	[NotificationList_Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pass0_Notify] ADD  DEFAULT ('') FOR [EditName]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統模組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pass0_Notify', @level2type=N'COLUMN',@level2name=N'NotificationList_Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能是否需要顯示在提醒視窗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pass0_Notify', @level2type=N'COLUMN',@level2name=N'SystemNotify'
GO


