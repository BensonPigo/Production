CREATE TABLE [dbo].[MDCalibrationList] (
    [ID]              BIGINT       IDENTITY (1, 1) NOT NULL,
    [MachineID]       VARCHAR (10) CONSTRAINT [DF_MDCalibrationList_MachineID] DEFAULT ('') NOT NULL,
    [CalibrationTime] TIME (7)     NULL,
    [Point1]          BIT          CONSTRAINT [DF_MDCalibrationList_Point1] DEFAULT ((0)) NOT NULL,
    [Point2]          BIT          CONSTRAINT [DF_MDCalibrationList_Point2] DEFAULT ((0)) NOT NULL,
    [Point3]          BIT          CONSTRAINT [DF_MDCalibrationList_Point3] DEFAULT ((0)) NOT NULL,
    [Point4]          BIT          CONSTRAINT [DF_MDCalibrationList_Point4] DEFAULT ((0)) NOT NULL,
    [Point5]          BIT          CONSTRAINT [DF_MDCalibrationList_Point5] DEFAULT ((0)) NOT NULL,
    [Point6]          BIT          CONSTRAINT [DF_MDCalibrationList_Point6] DEFAULT ((0)) NOT NULL,
    [Point7]          BIT          CONSTRAINT [DF_MDCalibrationList_Point7] DEFAULT ((0)) NOT NULL,
    [Point8]          BIT          CONSTRAINT [DF_MDCalibrationList_Point8] DEFAULT ((0)) NOT NULL,
    [Point9]          BIT          CONSTRAINT [DF_MDCalibrationList_Point9] DEFAULT ((0)) NOT NULL,
    [CalibrationDate] DATE         NULL,
    [SubmitDate]      DATETIME     NULL,
    [Operator]        VARCHAR (10) CONSTRAINT [DF_MDCalibrationList_Operator] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MDCalibrationList] PRIMARY KEY CLUSTERED ([ID] ASC)
);

