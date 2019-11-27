CREATE TABLE [dbo].[OrderChangeApplication_History] (
    [Ukey]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [ID]         VARCHAR (13) NULL,
    [Status]     VARCHAR (15) NULL,
    [StatusUser] VARCHAR (10) NULL,
    [StatusDate] DATETIME     NULL,
    CONSTRAINT [PK_OrderChangeApplication_History] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

