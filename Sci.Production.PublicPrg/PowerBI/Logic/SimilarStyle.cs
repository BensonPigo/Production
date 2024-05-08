using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class SimilarStyle
    {
        /// <inheritdoc/>
        public SimilarStyle()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSimilarStyleData(DateTime sdate)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region Where

            string where = $@"and so.OutputDate >= '{sdate.ToString("yyyy-MM-dd")}'";

            #endregion

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
SELECT o.FtyGroup
	,o.Styleid
	,o.SeasonID
	,o.CdCodeID
	,o.Qty
	,o.CPU
	,o.Category
	,o.StyleUkey
	,o.ID
    ,SciDelivery = min(o.SciDelivery)over(partition by o.Styleid)
    ,oa.Article
	,o.brandid
	,sty.ProductType
	,sty.FabricType
	,sty.Lining
	,sty.Gender
	,sty.Construction
	,sty.CDCodeNew
into #tmpo
FROM Orders o WITH (NOLOCK)
outer apply(
	select Article= STUFF((select distinct CONCAT(',',Article) 
					from Order_Article oa WITH (NOLOCK)
					where oa.id=o.ID
					for xml path('')),1,1,'')
) oa
Outer apply (
	SELECT ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
		, s.CDCodeNew
	FROM Style s WITH(NOLOCK)
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	where s.Ukey = o.StyleUkey
)sty
inner join SewingOutput_Detail sod  WITH (NOLOCK) on sod.OrderId = o.ID
inner join SewingOutput so  WITH (NOLOCK) on sod.id = so.id  {where}
where not exists (select 1 from Factory f WITH (NOLOCK) where f.ID = o.FactoryID and f.IsProduceFty = 0)

select o.ID
    , o.FtyGroup
	,o.Styleid
	,oa.Article
	,oa.Brand
	,o.SeasonID
	,o.CdCodeID
	,o.CPU,TQty = sum(o.Qty)
	,TCPU = sum(o.CPU * o.Qty)
	,o.CDCodeNew
	,o.ProductType
	,o.FabricType
	,o.Lining
	,o.Gender
	,o.Construction
into #tmpol
from #tmpo o
outer apply(
	select Article= STUFF((select distinct CONCAT(',',oa.Article) 
					from #tmpo oa
					where oa.styleid=o.styleid
					for xml path('')),1,1,''),
           Brand = STUFF((select distinct CONCAT(',',oa.Brandid) 
					from #tmpo oa
					where oa.styleid=o.styleid
					for xml path('')) ,1,1,'')   
) oa
group by o.ID, o.FtyGroup,o.Styleid,oa.Article,oa.Brand,o.SeasonID,o.CdCodeID,o.CPU
	, o.CDCodeNew, o.ProductType, o.FabricType, o.Lining, o.Gender, o.Construction

select distinct StyleID,OldStyleID,SciDelivery
into #tmpStyle_SimilarStyle
From 
(
		Select StyleID = MasterStyleID, BrandID = MasterBrandID, OldStyleID = t.StyleID,t.StyleUkey,t.SciDelivery
		From Style_SimilarStyle	s WITH(NOLOCK)
		inner join #tmpo t on (t.StyleID = s.MasterStyleID and t.BrandID = s.MasterBrandID) or (t.StyleID = s.ChildrenStyleID and t.BrandID = s.ChildrenBrandID)
		Union 
		Select StyleID = ChildrenStyleID, BrandID = ChildrenBrandID, OldStyleID = t.StyleID,t.StyleUkey,t.SciDelivery
		From Style_SimilarStyle	s WITH(NOLOCK)
		inner join #tmpo t on (t.StyleID = s.MasterStyleID and t.BrandID = s.MasterBrandID) or (t.StyleID = s.ChildrenStyleID and t.BrandID = s.ChildrenBrandID)
) main
Left join Style s WITH(NOLOCK) on s.ID = main.StyleID And s.BrandID = main.BrandID
Where s.Ukey <> main.StyleUkey
and exists(
	select 1 from #tmpo t
	where t.StyleID = main.OldStyleID
	and t.BrandID = main.BrandID
)

select 
	o.StyleID
	,S = sum(iif(o.Category = 'S',1,0)) over(partition by o.StyleID)
	,B = sum(iif(o.Category = 'B',1,0)) over(partition by o.StyleID)
	,OutputDate
	,SewingLineID
into #tmp_AR_Basic
from (select distinct StyleID,CdCodeID,SciDelivery from  #tmpo) o2
inner join Orders o WITH (NOLOCK) on o2.StyleID = o.StyleID and o2.CdCodeID = o.CdCodeID
inner join SewingOutput_Detail sod  WITH (NOLOCK) on sod.OrderId = o.ID
inner join SewingOutput so  WITH (NOLOCK) on sod.id = so.id {where}
where 1=1

select 
	o.StyleID,o.BrandID,o2.OldStyleID
	--,qty = sod.QAQty
	,S = sum(iif(o.Category = 'S',1,0)) over(partition by o.StyleID)
	,B = sum(iif(o.Category = 'B',1,0)) over(partition by o.StyleID)
	,OutputDate
	,SewingLineID
into #tmp_AR_Basic_S
from (
	select t.StyleID,t.OldStyleID,SciDelivery = min(orders.SciDelivery)
	from  #tmpStyle_SimilarStyle t
	inner join Orders on orders.StyleID = t.StyleID
	group by t.StyleID,t.OldStyleID
) o2
inner join Orders o WITH (NOLOCK) on o2.StyleID = o.StyleID
inner join SewingOutput_Detail sod  WITH (NOLOCK) on sod.OrderId = o.ID
inner join SewingOutput so  WITH (NOLOCK) on sod.id = so.id {where}
where 1=1

select tmp.StyleID
	,R =    case    when max_OutputDate is null then ''
                    when tmp.S > 0 AND tmp.B = 0 then ''
                    else concat(min(tmp.SewingLineID),'(',format(b.max_OutputDate,'yyyy/MM/dd'),')')
                    end
	,b.max_OutputDate
into #tmp_R
from #tmp_AR_Basic tmp 
inner join (select StyleID,max_OutputDate = max(OutputDate) from #tmp_AR_Basic group by StyleID) b
on tmp.StyleID = b.StyleID and tmp.OutputDate = b.max_OutputDate
group by tmp.StyleID,b.max_OutputDate,S,B

select a.StyleID,a.OldStyleID
	,R_Similar =    case    when max_OutputDate is null then ''
                    when a.S > 0 AND a.B = 0 then ''
                    else  concat(b.StyleID,'→',min(a.SewingLineID),'(',format(b.max_OutputDate,'yyyy/MM/dd'),')')
                    end
	,b.max_OutputDate
into #tmp_R_Similar
from #tmp_AR_Basic_S a 
inner join (select StyleID,max_OutputDate = max(OutputDate) from #tmp_AR_Basic_S group by StyleID) b
on a.StyleID = b.StyleID and a.OutputDate = b.max_OutputDate
group by a.StyleID,b.max_OutputDate,S,B,b.StyleID,a.OldStyleID

Select distinct
so.outPutDate,
so.FactoryID,
ol.StyleID,
[BrandID] = case 
		when o.BrandID != 'SUBCON-I' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end,
ReMark = isnull(r.R,''),
RS = ISNULL(rs.RS,''),
Type = iif((r.R is not null or rs.RS is not null) and so.outPutDate between DateADD(month, -3, Getdate()) and GetDate(), 'Repeat', 'New Style' )
From SewingOutput so  WITH(NOLOCK)
Inner join  SewingOutput_Detail sd  WITH(NOLOCK) on so.ID = sd.ID
Inner join Orders o  WITH(NOLOCK) on o.ID = sd.OrderId
Inner join #tmpol ol on o.ID = ol.ID and o.StyleID = ol.StyleID and o.SeasonID = ol.SeasonID
Left join #tmp_R r on r.StyleID = o.StyleID
Outer apply(	
	Select RS = Stuff(( Select  concat(',',R_Similar)
	                    From #tmp_R_Similar s
	                    Where s.OldStyleID = o.StyleID
	                    And R_Similar !=''
                        And s.StyleID != o.StyleID -- 排除自己StyleID
	                    For XML path('')),1,1,'')
)rs
outer apply( select BrandID from orders o1  WITH(NOLOCK) where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from Style  WITH(NOLOCK) where id = o.StyleID 
    and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
Where 1=1
{where}
Order by FactoryID, StyleID,OutputDate

drop table #tmpo
drop table #tmpol
drop table #tmpStyle_SimilarStyle
drop table #tmp_AR_Basic
drop table #tmp_AR_Basic_S
drop table #tmp_R
drop table #tmp_R_Similar
");

            #endregion

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlCmd.ToString(), out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }
    }
}
