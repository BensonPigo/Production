CREATE PROCEDURE [dbo].[P_Import_AdiCompReport]
As
Begin
	Set NoCount On;

	If Exists(Select * From POWERBIReportData.sys.tables Where Name = 'P_AdiCompReport')
	Begin
		Truncate Table POWERBIReportData.dbo.P_AdiCompReport;
	End
	;
	Insert into POWERBIReportData.dbo.P_AdiCompReport
	Select 
	[Year] = Year(a.StartDate)
	, Month = Right('00' + Cast(Month(A.StartDate) as varchar),2)
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
	, Supplier = iif(ad.SuppID = '', Isnull(po2.SuppID, ''), ad.SuppID)
	, SupplierName = iif(ad.SuppID = '', Isnull(supp.AbbCH, ''), adSupp.AbbCH)
	, DefectMain = concat(d.ID, '-', d.Name)
	, DefectSub = concat(dd.ID, '-', dd.SubName)
	, ad.Responsibility
	, f.MDivisionID
	From [MainServer].Production.dbo.ADIDASComplain a With(Nolock)
	Left join [MainServer].Production.dbo.ADIDASComplain_Detail ad With(Nolock) on a.ID = ad.ID
	Left join [MainServer].Production.dbo.ADIDASComplainDefect d With(Nolock) on d.ID = ad.DefectMainID
	Left join [MainServer].Production.dbo.ADIDASComplainDefect_Detail dd With(Nolock) on dd.ID = d.ID and dd.SubID = ad.DefectSubID
	Left join [MainServer].Production.dbo.Orders o With(Nolock) on ad.OrderID = o.ID
	Left join [MainServer].Production.dbo.Style s With(Nolock) on o.StyleUkey = s.Ukey
	Left join [MainServer].Production.dbo.PO With(Nolock) on o.POID = PO.ID
	Left join [MainServer].Production.dbo.PO_Supp po2 With(Nolock) on po2.ID = po.ID and po2.SEQ1 = '01'
	Left join [MainServer].Production.dbo.Supp With(Nolock) on Supp.ID = po2.SuppID
	Left join [MainServer].Production.dbo.Supp adSupp With(Nolock) on adSupp.ID = ad.SuppID
	Left join [MainServer].Production.dbo.SCIFty f With(Nolock) on f.id = ad.FactoryID
	Order by a.ID
			


	if exists (select 1 from BITableInfo b where b.id = 'P_AdiCompReport')
	begin
		update b
			set b.TransferDate = getdate()
		from BITableInfo b
		where b.id = 'P_AdiCompReport'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_AdiCompReport', getdate())
	end
End
