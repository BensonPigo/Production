CREATE TABLE [dbo].[ThreadAdjust_Detail] (
    [ID]               VARCHAR (13)   CONSTRAINT [DF_ThreadAdjust_Detail_ID] DEFAULT ('') NOT NULL,
    [Refno]            VARCHAR (36)   CONSTRAINT [DF_ThreadAdjust_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID]    VARCHAR (15)   CONSTRAINT [DF_ThreadAdjust_Detail_ThreadColorID] DEFAULT ('') NOT NULL,
    [NewConeBook]      NUMERIC (5)    NULL,
    [UsedConeBook]     NUMERIC (5)    NULL,
    [NewCone]          NUMERIC (5)    NULL,
    [UsedCone]         NUMERIC (5)    NULL,
    [ThreadLocationid] VARCHAR (10)   CONSTRAINT [DF_ThreadAdjust_Detail_ThreadLocationid] DEFAULT ('') NOT NULL,
    [remark]           NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ThreadAdjust_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [ThreadColorID] ASC, [ThreadLocationid] ASC)
);



