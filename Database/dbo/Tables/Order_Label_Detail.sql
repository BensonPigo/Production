CREATE TABLE [dbo].[Order_Label_Detail] (
    [Order_LabelUkey] BIGINT          NULL,
    [ID]              VARCHAR (13)    NULL,
    [LabelType]       VARCHAR (20)    NULL,
    [Seq]             INT             NULL,
    [RefNo]           VARCHAR (MAX)    NULL,
    [Description]     NVARCHAR (MAX)  NULL,
    [Position]        NVARCHAR (MAX)  NULL,
    [Order_BOAUkey]   BIGINT          NULL,
    [Ukey]            BIGINT          NOT NULL,
    [Junk]            BIT             NULL,
    [ConsPC]          NUMERIC (12, 4) NULL,
    CONSTRAINT [PK__Order_Label_Detail__Ukey] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);





