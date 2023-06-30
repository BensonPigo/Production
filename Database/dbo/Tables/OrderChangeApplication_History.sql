CREATE TABLE [dbo].[OrderChangeApplication_History] (
    [Ukey]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [ID]         VARCHAR (13) CONSTRAINT [DF_OrderChangeApplication_History_ID] DEFAULT ('') NOT NULL,
    [Status]     VARCHAR (15) CONSTRAINT [DF_OrderChangeApplication_History_Status] DEFAULT ('') NOT NULL,
    [StatusUser] VARCHAR (10) CONSTRAINT [DF_OrderChangeApplication_History_StatusUser] DEFAULT ('') NOT NULL,
    [StatusDate] DATETIME     NULL,
    CONSTRAINT [PK_OrderChangeApplication_History] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



