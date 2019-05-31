CREATE TABLE [dbo].[RFIDReader_Panel] (
    [RFIDReaderID] VARCHAR (24) NOT NULL,
    [PanelNo]      VARCHAR (24) NOT NULL,
    [CutCellID]    VARCHAR (10) CONSTRAINT [DF_RFIDReader_Panel_CutCellID] DEFAULT ('') NOT NULL,
    [AddDate]      DATETIME     NULL,
    [AddName]      VARCHAR (10) CONSTRAINT [DF_RFIDReader_Panel_AddName] DEFAULT ('') NOT NULL,
    [EditDate]     DATETIME     NULL,
    [EditName]     VARCHAR (10) CONSTRAINT [DF_RFIDReader_Panel_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_RFIDReader_Panel] PRIMARY KEY CLUSTERED ([RFIDReaderID] ASC, [PanelNo] ASC)
);

