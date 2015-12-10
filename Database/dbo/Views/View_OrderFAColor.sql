create view [dbo].[View_OrderFAColor]
as select a.ID,a.Article,isnull(oc.ColorID ,'') as ColorID
from (select distinct o.ID,o.POID,oq.Article 
	  from Orders o, Order_Qty oq
	  where o.ID = oq.ID) a
left join Order_ColorCombo oc on oc.Id = a.POID 
							  and oc.Article = a.Article 
							  and oc.PatternPanel = 'FA' 
							  and oc.LectraCode = (select min(LectraCode) 
												   from Order_ColorCombo 
												   where id = oc.Id and  Article = a.Article and PatternPanel = 'FA')