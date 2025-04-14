CREATE TABLE [dbo].[Pass1](
	[ID] [varchar](10) NOT NULL,
	[Name] [nvarchar](30) NULL,
	[Password] [varchar](10) NULL,
	[Position] [varchar](20) NULL,
	[FKPass0] [bigint] NULL,
	[IsAdmin] [bit] NULL,
	[IsMIS] [bit] NULL,
	[OrderGroup] [varchar](2) NULL,
	[EMail] [varchar](50) NULL,
	[ExtNo] [varchar](6) NULL,
	[OnBoard] [datetime] NULL,
	[Resign] [datetime] NULL,
	[Supervisor] [varchar](10) NULL,
	[Manager] [varchar](10) NULL,
	[Deputy] [varchar](10) NULL,
	[Factory] [varchar](100) NULL,
	[CodePage] [varchar](6) NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[LastLoginTime] [datetime] NULL,
	[ESignature] [nvarchar](60) NULL,
	[Remark] [nvarchar](100) NOT NULL,
	[ADAccount] [varchar](40) NOT NULL,
 [IsNeedOTP] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Pass1_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_Name]  DEFAULT ('') FOR [Name]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_Password]  DEFAULT ('') FOR [Password]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_Position]  DEFAULT ('') FOR [Position]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_FKPass0]  DEFAULT ((0)) FOR [FKPass0]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_IsAdmin]  DEFAULT ((0)) FOR [IsAdmin]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_IsMIS]  DEFAULT ((0)) FOR [IsMIS]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_OrderGroup]  DEFAULT ('') FOR [OrderGroup]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_EMail]  DEFAULT ('') FOR [EMail]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_ExtNo]  DEFAULT ('') FOR [ExtNo]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_Supervisor]  DEFAULT ('') FOR [Supervisor]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_Manager]  DEFAULT ('') FOR [Manager]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_Deputy]  DEFAULT ('') FOR [Deputy]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_Factory]  DEFAULT ('') FOR [Factory]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_CodePage]  DEFAULT ('') FOR [CodePage]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[Pass1] ADD  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[Pass1] ADD  CONSTRAINT [DF_Pass1_ADAccount]  DEFAULT ('') FOR [ADAccount]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AD帳號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pass1', @level2type=N'COLUMN',@level2name=N'ADAccount'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該人員是否需要OTP驗證',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Pass1',
    @level2type = N'COLUMN',
    @level2name = N'IsNeedOTP'