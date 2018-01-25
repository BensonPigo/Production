CREATE TABLE [dbo].[Style_Feature_SMV]
(
	[StyleFeatureUkey] BIGINT NOT NULL , 
    [Seq] VARCHAR(4) NULL DEFAULT (''), 
    [OperationID] VARCHAR(20) NULL DEFAULT (''), 
    [Annotation] NVARCHAR(MAX) NULL DEFAULT (''), 
    [MachineTypeID] VARCHAR(20) NULL DEFAULT (''), 
    [Mold] NVARCHAR(65) NULL DEFAULT (''), 
    [IETMSSMV] NUMERIC(9, 4) NULL DEFAULT ((0))
)
