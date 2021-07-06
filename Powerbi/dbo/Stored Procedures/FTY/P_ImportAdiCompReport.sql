CREATE PROCEDURE [dbo].[P_ImportAdiCompReport]
As
Begin
	Set NoCount On;
		
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	declare @current_PMS_ServerName nvarchar(50) 
	= (
		select [value] = 
			CASE WHEN @current_ServerName= 'PHL-NEWPMS-02' THEN 'PHL-NEWPMS' -- PH1
				 WHEN @current_ServerName= 'VT1-PH2-PMS2b' THEN 'VT1-PH2-PMS2' -- PH2
				 WHEN @current_ServerName= 'system2017BK' THEN 'SYSTEM2017' -- SNP
				 WHEN @current_ServerName= 'SPS-SQL2' THEN 'SPS-SQL.spscd.com' -- SPS
				 WHEN @current_ServerName= 'SQLBK' THEN 'PMS-SXR' -- SPR
				 WHEN @current_ServerName= 'newerp-bak' THEN 'newerp' -- HZG		
				 WHEN @current_ServerName= 'SQL' THEN 'MainServer' -- HXG
				 when (select top 1 MDivisionID from Production.dbo.Factory) in ('VM2','VM1') then 'SYSTEM2016' -- ESP & SPT
			ELSE '' END
	)

	If Exists(Select * From POWERBIReportData.sys.tables Where Name = 'P_AdiCompReport')
	Begin
		Drop Table POWERBIReportData.dbo.P_AdiCompReport;
	End;
	Create Table POWERBIReportData.dbo.P_AdiCompReport
		(Year			 Varchar(4),
		 Month			 VarChar(2),
		 ID				 VarChar(13),
		 SalesID		 VarChar(5),
		 SalesName		 NVarChar(50),
		 Article		 VarChar(8),
		 ArticleName	 NVarChar(60),
		 ProductionDate	 Date,
		 DefectMainID	 VarChar(2),
		 DefectSubID	 VarChar(1),
		 FOB			 Numeric(5,2),
		 Qty			 Numeric(3,0),
		 ValueinUSD		 Numeric(8,4),
		 ValueINExRate	 Numeric(7,2),
		 OrderID		 VarChar(13),
		 RuleNo			 Numeric(1,0),
		 UKEY			 Bigint,
		 BrandID		 VarChar(8),
		 FactoryID		 VarChar(8),
		 SuppID			 VarChar(6),
		 Refno		     VarChar(23),
		 IsEM			 Bit,
		 StyleID		 VarChar(15),
		 ProgramID		 VarChar(12),
		 Supplier		 VarChar(6),
		 SupplierName	 NVarChar(12),
		 DefectMain		 NVarChar(260),
		 DefectSub		 NVarChar(260),
		 Responsibility	 VarChar(2),
		 MDivisionID	 VarChar(8),
		)
	;

	Declare @ExecSQL NVarChar(MAX);
	Set @ExecSQL =
		N'Insert into POWERBIReportData.dbo.P_AdiCompReport
				Select * From OpenQuery(['+@current_PMS_ServerName+'], 
				  ''Select Year = Year(a.StartDate)
		, Month = Right(''''00'''' + Cast(Month(A.StartDate) as varchar),2)
		, ad.ID
		, ad.SalesID
		, ad.SalesName
		, ad.Article
		, ad.ArticleName
		, ad.ProductionDate
		, ad.DefectMainID
		, ad.DefectSubID
		, FOB = Isnull(ad.FOB, 0)
		, Qty = Isnull(ad.Qty, 0)
		, ValueinUSD = Isnull(ad.ValueinUSD, 0)
		, ValueINExRate = Isnull(ad.ValueINExRate, 0)
		, ad.OrderID
		, ad.RuleNo
		, ad.UKEY
		, ad.BrandID
		, ad.FactoryID
		, ad.SuppID
		, ad.Refno
		, ad.IsEM 
		, StyleID = s.Id
		, s.ProgramID
		, Supplier = iif(ad.SuppID = '''''''', Isnull(po2.SuppID, ''''''''), ad.SuppID)
		, SupplierName = iif(ad.SuppID = '''''''', Isnull(supp.AbbCH, ''''''''), adSupp.AbbCH)
		, DefectMain = concat(d.ID, ''''-'''', d.Name)
		, DefectSub = concat(dd.ID, ''''-'''', dd.SubName)
		, ad.Responsibility
		, f.MDivisionID
	From Production.dbo.ADIDASComplain a With(Nolock)
	Left join Production.dbo.ADIDASComplain_Detail ad With(Nolock) on a.ID = ad.ID
	Left join Production.dbo.ADIDASComplainDefect d With(Nolock) on d.ID = ad.DefectMainID
	Left join Production.dbo.ADIDASComplainDefect_Detail dd With(Nolock) on dd.ID = d.ID and dd.SubID = ad.DefectSubID
	Left join Production.dbo.Orders o With(Nolock) on ad.OrderID = o.ID
	Left join Production.dbo.Style s With(Nolock) on o.StyleUkey = s.Ukey
	Left join Production.dbo.PO With(Nolock) on o.POID = PO.ID
	Left join Production.dbo.PO_Supp po2 With(Nolock) on po2.ID = po.ID and po2.SEQ1 = ''''01''''
	Left join Production.dbo.Supp With(Nolock) on Supp.ID = po2.SuppID
	Left join Production.dbo.Supp adSupp With(Nolock) on adSupp.ID = ad.SuppID
	Left join Production.dbo.Factory f With(Nolock) on f.id = ad.FactoryID
	Order by a.ID
	'');
			';
	Exec (@ExecSQL);

End


