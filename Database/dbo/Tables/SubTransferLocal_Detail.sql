CREATE TABLE [dbo].[SubTransferLocal_Detail] (
    [ID]           VARCHAR (13)    CONSTRAINT [DF__SubTransferL__ID__180E3640] DEFAULT ('') NOT NULL,
    [MDivisionID]  VARCHAR (8)     CONSTRAINT [DF__SubTransf__MDivi__19025A79] DEFAULT ('') NOT NULL,
    [Poid]         VARCHAR (13)    CONSTRAINT [DF__SubTransfe__Poid__19F67EB2] DEFAULT ('') NOT NULL,
    [Refno]        VARCHAR (21)    CONSTRAINT [DF__SubTransf__Refno__1AEAA2EB] DEFAULT ('') NOT NULL,
    [Color]        VARCHAR (15)    CONSTRAINT [DF__SubTransf__Color__1BDEC724] DEFAULT ('') NOT NULL,
    [FromLocation] VARCHAR (100)   CONSTRAINT [DF__SubTransf__FromL__1CD2EB5D] DEFAULT ('') NULL,
    [ToLocation]   VARCHAR (100)   CONSTRAINT [DF__SubTransf__ToLoc__1DC70F96] DEFAULT ('') NULL,
    [Qty]          NUMERIC (11, 2) CONSTRAINT [DF__SubTransfer__Qty__1EBB33CF] DEFAULT ((0)) NULL,
    [Ukey]         BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_SubTransferLocal_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


