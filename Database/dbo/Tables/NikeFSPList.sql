CREATE TABLE NikeFSPList (
    FSPCode varchar(4) NOT NULL,
    FSPDesc varchar(15) NOT NULL CONSTRAINT DF_NikeFSPList_FSPDesc DEFAULT (''),
    Junk bit NOT NULL CONSTRAINT DF_NikeFSPList_Junk DEFAULT (0),
    AddName varchar(10) NOT NULL CONSTRAINT DF_NikeFSPList_AddName DEFAULT (''),
    AddDate datetime NULL,
    EditName varchar(10) NOT NULL CONSTRAINT DF_NikeFSPList_EditName DEFAULT (''),
    EditDate datetime NULL,
    CONSTRAINT PK_NikeFSPList PRIMARY KEY (FSPCode)
);
