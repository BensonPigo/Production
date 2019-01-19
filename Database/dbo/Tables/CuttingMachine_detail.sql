CREATE TABLE [dbo].[CuttingMachine_detail] (
    [ID]              VARCHAR (10)   NOT NULL,
    [MtlTypeID]       VARCHAR (20)   NOT NULL,
    [LayerLowerBound] INT            NOT NULL,
    [LayerUpperBound] INT            NOT NULL,
    [ActualSpeed]     NUMERIC (5, 3) NULL,
    [remark]          NVARCHAR (MAX) NULL,
    [AddName]         VARCHAR (10)   NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   NULL,
    [EditDate]        DATETIME       NULL,
    CONSTRAINT [PK_CuttingMachine_detail] PRIMARY KEY CLUSTERED ([ID] ASC, [MtlTypeID] ASC, [LayerLowerBound] ASC, [LayerUpperBound] ASC)
);



