CREATE TABLE [dbo].[TransLog] (
    [ukey]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [FunctionName] NVARCHAR (100) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [StartTime]    DATETIME       NOT NULL,
    [EndTime]      DATETIME       NULL,
    [TransID]      NCHAR (20)     NULL,
    [GroupID]      INT            NULL,
    [RegionID]     VARCHAR (50)   NULL,
    [TransCode]    INT            NULL,
    [Is_Export]    BIT            NULL,
    CONSTRAINT [PK_TransLog] PRIMARY KEY CLUSTERED ([ukey] ASC)
);



