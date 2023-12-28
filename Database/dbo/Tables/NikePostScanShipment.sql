CREATE TABLE NikePostScanShipment (
  InvNo varchar(25) NOT NULL,
  ShipmentNo varchar(100) NOT NULL CONSTRAINT DF_NikePostScanShipment_ShipmentNo DEFAULT '',
  ShippingDate date NULL,
  TCPCode varchar(10) NOT NULL CONSTRAINT DF_NikePostScanShipment_TCPCode DEFAULT '',
  LoadIndicator varchar(100) NOT NULL CONSTRAINT DF_NikePostScanShipment_LoadIndicator DEFAULT '',
  TrackingNo varchar(100) NOT NULL CONSTRAINT DF_NikePostScanShipment_TrackingNo DEFAULT '',
  LSPBookingNumber varchar(100) NOT NULL CONSTRAINT DF_NikePostScanShipment_LSPBookingNumber DEFAULT '',
  FactoryInvoiceNbr varchar(100) NOT NULL CONSTRAINT DF_NikePostScanShipment_FactoryInvoiceNbr DEFAULT '',
  FactoryInvoiceDate date NULL,
  FSPCode varchar(4) NOT NULL CONSTRAINT DF_NikePostScanShipment_FSPCode DEFAULT '',
  LCReferenceNbr varchar(100) NOT NULL CONSTRAINT DF_NikePostScanShipment_LCReferenceNbr DEFAULT '',
  QAReferenceNbr varchar(100) NOT NULL CONSTRAINT DF_NikePostScanShipment_QAReferenceNbr DEFAULT '',
  CONSTRAINT PK_NikePostScanShipment PRIMARY KEY (InvNo)
);