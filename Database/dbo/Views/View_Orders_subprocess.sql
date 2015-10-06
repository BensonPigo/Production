
create view [dbo].[View_Orders_subprocess]
as
select orders.ID ,StyleID,FactoryID,CutOffLine,CutInLine,SciDelivery,orders.SewLine,SewInLine,SewOffLine,BuyerDelivery
,Dest,qty,orders.BrandID,orders.ProgramID,SeasonID,Category,orders.CurrencyID,orders.CustCDID
from Orders inner join 
(select order_tmscost.ID from Order_TmsCost, ArtworkType where (qty > 0 or tms > 0) and ArtworkType.IsSubprocess = 1 and Order_TmsCost.ArtworkTypeID = artworktype.id group by Order_TmsCost.id having count(1) > 0) a
on Orders.id = a.id
where Orders.Finished = 0 and orders.IsForecast = 0
