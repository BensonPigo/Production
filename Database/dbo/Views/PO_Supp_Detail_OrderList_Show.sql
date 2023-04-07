

Create View [dbo].[PO_Supp_Detail_OrderList_Show] As
Select PO_Supp_Detail.ID, PO_Supp_Detail.Seq1, PO_Supp_Detail.Seq2
	 , IsNull(PO_Supp_Detail_OrderList.OrderList, '') as OrderList
  From dbo.PO_Supp_Detail with (nolock)
  Left Join (Select ID, Seq1, Seq2
				  , OrderList = (Select IIF(SubString(OrderID,1,10) = SubString(ID,1,10), SubString(OrderID,9,5), OrderID) + '/'
								   From dbo.PO_Supp_Detail_OrderList as tmp with (nolock)
								  Where tmp.ID = PO_Supp_Detail_OrderList.ID
									And tmp.Seq1 = PO_Supp_Detail_OrderList.Seq1
									And tmp.Seq2 = PO_Supp_Detail_OrderList.Seq2
								  Order by OrderID
									For XML path('')
								)
			   From dbo.PO_Supp_Detail_OrderList with (nolock)
			  Group by ID, Seq1, Seq2
			) as PO_Supp_Detail_OrderList
	On	   PO_Supp_Detail.ID = PO_Supp_Detail_OrderList.ID
	   And PO_Supp_Detail.Seq1 = PO_Supp_Detail_OrderList.Seq1
	   And PO_Supp_Detail.Seq2 = PO_Supp_Detail_OrderList.Seq2;