CREATE TABLE [dbo].[POShippingList_Line] (
    [POShippingList_Ukey] BIGINT          NOT NULL,
    [Export_Detail_Ukey]  BIGINT          NULL,
    [QRCode]              VARCHAR (500)   NOT NULL,
    [Line]                VARCHAR (2)     NOT NULL,
    [RefNo]               VARCHAR (20)    NULL,
    [Description]         NVARCHAR (MAX)  NULL,
    [MaterialColor]       VARCHAR (MAX)   NULL,
    [Weight]              VARCHAR (20)    NULL,
    [WeightUnitID]        VARCHAR (8)     NULL,
    [Width]               VARCHAR (20)    NULL,
    [WidthUnitID]         VARCHAR (8)     NULL,
    [Length]              VARCHAR (20)    NULL,
    [LengthUnitID]        VARCHAR (8)     NULL,
    [Height]              VARCHAR (20)    NULL,
    [HeightUnitID]        VARCHAR (8)     NULL,
    [Thickness]           VARCHAR (20)    NULL,
    [ThicknessUnitID]     VARCHAR (8)     NULL,
    [SizeSpec]            VARCHAR (15)    NULL,
    [Price]               NUMERIC (16, 4) NULL,
    [BatchNo]             VARCHAR (50)    NULL,
    [PackageNo]           VARCHAR (50)    NULL,
    [ShipQty]             NUMERIC (12, 2) NULL,
    [ShipQtyUnitID]       VARCHAR (8)     NULL,
    [FOC]                 NUMERIC (12, 2) NULL,
    [FOCUnitID]           VARCHAR (8)     NULL,
    [NW]                  NUMERIC (10, 2) NULL,
    [NWUnitID]            VARCHAR (8)     NULL,
    [GW]                  NUMERIC (10, 2) NULL,
    [GWUnitID]            VARCHAR (8)     NULL,
    [AdditionalOptional1] NVARCHAR (MAX)  NULL,
    [AdditionalOptional2] NVARCHAR (MAX)  NULL,
    [AdditionalOptional3] NVARCHAR (MAX)  NULL,
    [AdditionalOptional4] NVARCHAR (MAX)  NULL,
    [AdditionalOptional5] NVARCHAR (MAX)  NULL,
    [AddName]             VARCHAR (10)    NULL,
    [AddDate]             DATETIME        NULL,
    CONSTRAINT [PK_POShippingList_Line] PRIMARY KEY CLUSTERED ([POShippingList_Ukey] ASC, [QRCode] ASC, [Line] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AdditionalOptional5', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'AdditionalOptional5';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AdditionalOptional4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'AdditionalOptional4';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AdditionalOptional3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'AdditionalOptional3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AdditionalOptional2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'AdditionalOptional2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AdditionalOptional1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'AdditionalOptional1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for Shipment Gross Weight)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'GWUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Gross Weight', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'GW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for Shipment Net Weight)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'NWUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Net Weight', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'NW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for Compensate for defect)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'FOCUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Compensate for defect quantity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'FOC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for Shipped Quantity)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'ShipQtyUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipped Quantity', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Package no. ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'PackageNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Batch no.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unit Price', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Size', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for thickness)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'ThicknessUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thickness', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Thickness';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for height)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'HeightUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Height', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Height';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for length)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'LengthUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Length', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Length';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for width)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'WidthUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Width', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UOM (for weight)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'WeightUnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Weight', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Weight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Material Color', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'MaterialColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description / Supplier Material Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ref#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'RefNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Line #', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'Line';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QR Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'POShippingList_Line', @level2type = N'COLUMN', @level2name = N'QRCode';

