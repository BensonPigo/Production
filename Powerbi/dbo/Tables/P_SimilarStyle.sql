Create Table P_SimilarStyle
(
OutputDate date,
FactoryID varchar(8),
StyleID varchar(15),
BrandID varchar(8),
Remark nvarchar(200),
[Remark(Similar Style)] nvarchar(MAX),
Type varchar(10)
 PRIMARY KEY (OutputDate, FactoryID, StyleID, BrandID) NOT NULL
)