using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    public partial class R01 : Sci.Win.Tems.PrintForm
    {
        int selectindex = 0;
        string factory, mdivision, orderby, spno1, spno2, artworktype,subcons;
        DateTime? sciDelivery1, sciDelivery2, buyerDelivery1, buyerDelivery2, sewinline1, sewinline2
            , cutinline1, cutinline2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();

        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            txtfactory1.Text = Sci.Env.User.Factory;
            cbxCategory.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox(" < SCI Delivery > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            sciDelivery1 = dateRange1.Value1;
            sciDelivery2 = dateRange1.Value2;
            
            #endregion
            mdivision = txtMdivision1.Text;
            factory = txtfactory1.Text;
            selectindex = cbxCategory.SelectedIndex;
           
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --
            
            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            #endregion

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"
;with styleData as (
Select ot.ArtworkTypeID
,o.FtyGroup
,o.styleid
,count(o.styleid) target_count_style
, sum(o.qty) target_total_order_qty
, min(o.SewInLine) target_min_sewinline
, sum(o.qty * ot.Qty) target_sum_stitch
,ot.ArtworkUnit
,sum(o.qty * ot.tms) target_sum_tms
from dbo.orders o
inner join dbo.Order_TmsCost ot on ot.ID = o.ID 
inner join dbo.ArtworkType a on a.id = ot.ArtworkTypeID
where ot.Price+ot.Qty+ot.TMS > 0
and a.IsSubprocess = 1"));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and o.SciDelivery between '{0}' and '{1}'",
                Convert.ToDateTime(sciDelivery1).ToString("d"), Convert.ToDateTime(sciDelivery2).ToString("d")));
            }


            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and o.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and o.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }

            switch (selectindex)
            {
                case 0:
                    sqlCmd.Append(@" and (Category = 'B' or Category = 'S')");
                    break;
                case 1:
                    sqlCmd.Append(@" and Category = 'B' ");
                    break;
                case 2:
                    sqlCmd.Append(@" and (Category = 'S')");
                    break;
                case 3:
                    sqlCmd.Append(@" and (Category = 'M' )");
                    break;
            }

            #endregion
            condition.Clear();
            condition.Append(string.Format(@"SCI Delivery : {0} ~ {1}"
                , Convert.ToDateTime(sciDelivery1).ToString("d")
                , Convert.ToDateTime(sciDelivery2).ToString("d")));

sqlCmd.Append(string.Format(@"group by ot.ArtworkTypeID,o.FtyGroup,o.StyleID,ot.ArtworkUnit

),

targetData as (
Select t.ArtworkTypeID
,t.FtyGroup
,sum(t.target_count_style) target_count_style
,sum(t.target_total_order_qty) target_total_order_qty 
,sum(t.target_sum_stitch) target_sum_stitch
,t.ArtworkUnit
,sum(t.target_sum_tms) target_sum_tms
from styleData t
group by t.ArtworkTypeID,t.FtyGroup,t.ArtworkUnit
)
, orderData_basic as (
select o.FtyGroup
,o.StyleID
,count(1) order_count_style
,min(SewInLine) order_min_sewinline
,sum(o.qty) order_total_qty
,sum(o.qty * t.CpuRate) order_total_cpu
from  dbo.orders o 
inner join styleData s on s.FtyGroup = o.FtyGroup and s.StyleID = o.StyleID
cross apply (select * from dbo.GetCPURate(o.OrderTypeID,o.ProgramID,o.Category,o.BrandID,'O')
where o.Junk=0 ) t
group by o.FtyGroup,o.StyleID
)
, orderDataCount as
(
	select a.id artworktypeid,x.* from dbo.artworktype a 
cross join (select * from (select FtyGroup,sum(order_count_style) order_count_style
	,sum(order_total_qty) order_total_qty
	,sum(order_total_cpu) order_total_cpu
	from orderData_basic
	group by FtyGroup)x1) x
	where a.IsSubprocess=1
)
, orderData as (
select a.id artworktypeid,x.* from dbo.artworktype a 
cross join (select * from  orderData_basic) x
where a.IsSubprocess=1 
)
, combine (ArtworkTypeID,FtyGroup,order_count_style,new_styles,order_total_qty,order_total_cpu,target_count_style,
			target_total_order_qty,ArtworkUnit,target_sum_stitch,target_sum_tms) as 
(
select orderDataCount.ArtworkTypeID,orderDataCount.FtyGroup
,orderDataCount.order_count_style
,sum(y.new_styles) new_styles
,orderDataCount.order_total_qty
,orderDataCount.order_total_cpu
,sum(y.target_count_style) target_count_style
,sum(y.target_total_order_qty) target_total_order_qty
,y.ArtworkUnit
,sum(y.target_sum_stitch) target_sum_stitch
,sum(y.target_sum_tms) target_sum_tms
from orderDataCount left join  (
	select a.FtyGroup,a.StyleID,a.order_count_style,iif(a.order_min_sewinline = b.target_min_sewinline,b.target_count_style,0) new_styles
	,a.order_total_qty,a.order_total_cpu,b.ArtworkTypeID,b.target_count_style,b.target_total_order_qty
	,b.target_sum_stitch,b.ArtworkUnit,b.target_sum_tms
	,a.order_min_sewinline , b.target_min_sewinline
	from orderData a 
	inner join styleData b on b.FtyGroup = a.FtyGroup and b.StyleID = a.StyleID and b.ArtworkTypeID = a.artworktypeid
	) y
on orderDataCount.FtyGroup = y.FtyGroup and orderDataCount.artworktypeid = y.ArtworkTypeID
group by orderDataCount.ArtworkTypeID,orderDataCount.FtyGroup,y.ArtworkUnit,orderDataCount.order_count_style,orderDataCount.order_total_qty
,orderDataCount.order_total_cpu
)
, subtotal (ArtworkTypeID,FtyGroup,order_count_style,new_styles,order_total_qty,order_total_cpu,target_count_style,
			target_total_order_qty,ArtworkUnit,target_sum_stitch,target_sum_tms,[BO %],[BS %],O) AS
(
select ArtworkTypeID,'Subtotal',sum(order_count_style) ,sum(new_styles),sum(order_total_qty),sum(order_total_cpu),sum(target_count_style),
			sum(target_total_order_qty),'',sum(target_sum_stitch),sum(target_sum_tms) ,null,null,'ZZZZZZZ1' as o
from combine w
group by ArtworkTypeID
)
select FtyGroup,order_count_style,new_styles,order_total_qty
,order_total_cpu,ArtworkTypeID,target_count_style,target_total_order_qty
,target_sum_stitch,ArtworkUnit,target_sum_tms 
,[BO %]
,[BS %]
from (

select FtyGroup,order_count_style,new_styles,order_total_qty
,order_total_cpu,ArtworkTypeID,target_count_style,target_total_order_qty
,ArtworkUnit,target_sum_stitch,target_sum_tms 
,round(z.target_total_order_qty*100.00 / (select sum(orderData_basic.order_total_qty) from orderData_basic where FtyGroup = z.FtyGroup),2) [BO %]
,round(z.target_total_order_qty*100.00 / iif(sum(z.target_total_order_qty) over (partition by z.FtyGroup) = 0,1,sum(z.target_total_order_qty) over (partition by z.FtyGroup)),2) [BS %]
,FtyGroup as o
,ArtworkTypeID as q
 from combine z
union all
select FtyGroup,order_count_style,new_styles,order_total_qty
,order_total_cpu,ArtworkTypeID,target_count_style,target_total_order_qty
,ArtworkUnit,target_sum_stitch,target_sum_tms
,[BO %],[BS %]
,O
,ArtworkTypeID as q
from subtotal w
union all
select 'Percentage',null,new_styles*100.00/order_count_style,null
,order_total_cpu*100.00/order_total_qty ,'[CPU/Pcs]',target_count_style*100.00/order_count_style ,target_total_order_qty*100.00/order_total_qty
,null,null,null
,null,null,'ZZZZZZZ2' as o
,ArtworkTypeID as q
from subtotal
 ) as p
 order by q,o
 "));

            //o1.BuyerDelivery between '20150101' and '20150731' AND O1.Finished = 0 AND O2.Price > 0 and o1.FtyGroup = 'MWI' 
            //	and o1.id='15040605CW002'
            


            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }
            
            if (printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R01.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Planning_R01.xltx", 4, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            objSheets.Cells[3, 2] = DateTime.Now.ToString();  // 列印日期寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
