CREATE TABLE [dbo].[Order_Label_Detail] (
    [Order_LabelUkey] BIGINT          CONSTRAINT [DF_Order_Label_Detail_Order_LabelUkey] DEFAULT ((0)) NOT NULL,
    [ID]              VARCHAR (13)    CONSTRAINT [DF_Order_Label_Detail_ID] DEFAULT ('') NOT NULL,
    [LabelType]       VARCHAR (20)    CONSTRAINT [DF_Order_Label_Detail_LabelType] DEFAULT ('') NOT NULL,
    [Seq]             INT             CONSTRAINT [DF_Order_Label_Detail_Seq] DEFAULT ((0)) NOT NULL,
    [RefNo]           VARCHAR (MAX)   CONSTRAINT [DF_Order_Label_Detail_RefNo] DEFAULT ('') NOT NULL,
    [Description]     NVARCHAR (MAX)  CONSTRAINT [DF_Order_Label_Detail_Description] DEFAULT ('') NOT NULL,
    [Position]        NVARCHAR (MAX)  CONSTRAINT [DF_Order_Label_Detail_Position] DEFAULT ('') NOT NULL,
    [Order_BOAUkey]   BIGINT          CONSTRAINT [DF_Order_Label_Detail_Order_BOAUkey] DEFAULT ((0)) NOT NULL,
    [Ukey]            BIGINT          NOT NULL,
    [Junk]            BIT             CONSTRAINT [DF_Order_Label_Detail_Junk] DEFAULT ((0)) NOT NULL,
    [ConsPC]          DECIMAL (12, 4) CONSTRAINT [DF_Order_Label_Detail_ConsPC] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Order_Label_Detail__Ukey] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);







