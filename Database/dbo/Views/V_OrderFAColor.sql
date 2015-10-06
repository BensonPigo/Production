create view V_OrderFAColor
as select o.ID,oa.Article,occ.ColorID
 from Order_ColorCombo occ, Orders o, Order_Article oa
 where occ.LectraCode = (select min(LectraCode) 
                         from Order_ColorCombo 
                         where id = occ.Id and  Article = oa.Article and PatternPanel = 'FA') 
 and occ.Id = o.POID
 and oa.id = o.POID
 and occ.Article = oa.Article 
 and occ.PatternPanel = 'FA'

