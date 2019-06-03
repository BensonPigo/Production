CREATE TABLE [dbo].[ShippingMarkPic_Detail] (
    [ShippingMarkPicUkey] BIGINT         NOT NULL,
    [SCICtnNo]            VARCHAR (15)   NOT NULL,
    [FileName]            VARCHAR (30)   CONSTRAINT [DF_ShippingMarkPic_Detail_FileName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ShippingMarkPic_Detail] PRIMARY KEY CLUSTERED ([ShippingMarkPicUkey] ASC, [SCICtnNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCICtnNo+Seq+Side', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ShippingMarkPic_Detail', @level2type = N'COLUMN', @level2name = N'FileName';


GO