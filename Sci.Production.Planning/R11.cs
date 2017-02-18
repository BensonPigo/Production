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
    public partial class R11 : Sci.Win.Tems.PrintForm
    {
        int selectindex = 0;
        decimal months;
        string factory, mdivision;
        DateTime? sciDelivery1, sciDelivery2;
        DataTable printData, dtArtworkType;
        StringBuilder condition = new StringBuilder();
        StringBuilder artworktypes = new StringBuilder();

        public R11(ToolStripMenuItem menuitem)
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

            #region -- 必輸的條件 --
            sciDelivery1 = dateRange1.Value1;
            sciDelivery2 = dateRange1.Value2;
            #endregion
            mdivision = txtMdivision1.Text;
            factory = txtfactory1.Text;
            selectindex = cbxCategory.SelectedIndex;
            months = numericUpDown1.Value;

            DualResult result;
            if (!(result = DBProxy.Current.Select("", "select id from dbo.artworktype WITH (NOLOCK) where istms=1 or isprice= 1 order by seq", out dtArtworkType)))
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            if (dtArtworkType.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Artwork Type data not found, Please inform MIS to check !");
                return false;
            }

            artworktypes.Clear();
            for (int i = 0; i < dtArtworkType.Rows.Count;i++ )
            {
                artworktypes.Append(string.Format(@"[{0}],", dtArtworkType.Rows[i]["id"].ToString()));
            }
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
--依條件取得訂單資料
;with rawdata_order as 
(
SELECT a.FtyGroup,a.Styleid, a.CDCODEID, a.Qty,a.SciDelivery, a.CPU, a.SeasonID 
, a.id orderid, a.category,a.StyleUkey,a.OrderTypeID,a.ProgramID,a.BrandID
FROM dbo.Orders a WITH (NOLOCK)  
where 1=1 "));

            #region --- 條件組合  ---
            condition.Clear();
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and a.SciDelivery between '{0}' and '{1}'",
                Convert.ToDateTime(sciDelivery1).ToString("d"), Convert.ToDateTime(sciDelivery2).ToString("d")));
                condition.Append(string.Format(@"SCI Delivery : {0} ~ {1}"
                , Convert.ToDateTime(sciDelivery1).ToString("d")
                , Convert.ToDateTime(sciDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
                condition.Append(string.Format(@"    M : {0}", mdivision));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and a.FtyGroup = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
                condition.Append(string.Format(@"    Factory : {0}", factory));
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
            condition.Append(string.Format(@"    Category : {0}", cbxCategory.SelectedText));

            #endregion

            sqlCmd.Append(string.Format(@")
, rawdata_output as
(
SELECT a.StyleUkey, a.Styleid, a.CDCODEID, a.Qty,a.SciDelivery, a.CPU, a.SeasonID , a.orderid, a.category
,b.WorkHour,b.QAQty,c.ManHour,c.Manpower,c.ID,c.OutputDate,c.FactoryID,c.SewingLineID
,iif(a.Category='S',round(a.cpu*s.StdTMS,0),round(a.cpu*e.rate/100.00*s.StdTMS,0)) set_tms
FROM dbo.System s WITH (NOLOCK)  
,rawdata_order a
inner join dbo.SewingOutput_Detail b on b.OrderId = a.orderid
inner join dbo.SewingOutput c on c.id = b.ID 
left join dbo.style_location e on e.StyleUkey = a.StyleUkey and e.Location = iif(b.ComboType='' or b.ComboType is null,'T',b.ComboType)
cross apply (select * from dbo.GetCPURate(a.OrderTypeID,a.ProgramID,a.Category,a.BrandID,'O')) g
where c.FactoryID = a.FtyGroup"));
            if (!MyUtility.Check.Empty(sciDelivery1))
            {
                sqlCmd.Append(string.Format(@" and  dateadd(month,{0},a.sciDelivery) > c.OutputDate", 0 - months));
                condition.Append(string.Format(@"    New Style base on {0} month(s)", months));
            }

            sqlCmd.Append(string.Format(@")
-- 依找到的output資料，先將最新的output日資料整理
,max_output as
(
	select distinct styleukey,outputdate,SewingLineID from rawdata_output M 
				where exists(select StyleUkey,max(outputdate) as mmm 
								from rawdata_output 
								where Category = 'B'
								group by styleUkey 
								having M.StyleUkey = StyleUkey and m.OutputDate = max(outputdate)  )
)
--依取得的訂單資料取得訂單的 TMS Cost
,rawdata_tmscost as
(
select aa.StyleUkey,bb.ArtworkTypeID,max(iif(cc.IsTMS=1,bb.tms,bb.price)) price_tms 
from rawdata_order aa 
inner join dbo.Order_TmsCost bb WITH (NOLOCK) on bb.id = aa.orderid
inner join dbo.ArtworkType cc WITH (NOLOCK) on cc.id = bb.ArtworkTypeID
where IsTMS =1 or IsPrice = 1
group by aa.StyleUkey,bb.ArtworkTypeID
)"));

            sqlCmd.Append(string.Format(@"--將取得Tms Cost做成樞紐表
, tmscost_pvt as 
(
    select * from rawdata_tmscost
    pivot
    (
        sum(price_tms)
        for artworktypeid in ( {0})
    )as pvt )",artworktypes.ToString().Substring(0,artworktypes.ToString().Length-1)));

            sqlCmd.Append(string.Format(@"
select a.FtyGroup,a.StyleID,a.SeasonID,a.CdCodeID,a.CPU,a.ttl_qty,a.ttl_cpu
,round(B.ttl_output/ NULLIF(ttl_target,0),4) [Avg. Eff.]
,iif(xxx.outputdate is null,'Only Sample',xxx.sewinglineid+' ('+convert(varchar,xxx.outputdate)+')')remark
,(select 'Y' from dbo.Style_TmsCost WITH (NOLOCK) where StyleUkey = a.StyleUkey and ArtworkTypeID = 'GMT WASH' and price > 0) wash
,pvt.*
from
(select FtyGroup,styleukey,Styleid,seasonid,cdcodeid,cpu,sum(qty) ttl_qty,sum(qty*cpu) ttl_cpu from rawdata_order group by FtyGroup,styleukey,Styleid,seasonid,cdcodeid,cpu) a
inner join tmscost_pvt pvt on pvt.StyleUkey = a.StyleUkey
left join 
(select StyleUkey
    , nullif(sum(set_tms*qaqty),0) ttl_tms 
    , nullif(sum(WorkHour*Manpower),0) ttl_manhours
    , sum(qaqty) ttl_output
    , sum(round(3600*WorkHour/ NULLIF(set_tms*Manpower,0),0)) ttl_target
from rawdata_output group by StyleUkey) b on b.StyleUkey = a.StyleUkey
outer apply (select outputdate,SewingLineID from max_output M 
				where m.StyleUkey = a.StyleUkey) xxx
order by a.FtyGroup, a.StyleID,a.SeasonID
 "));


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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Planning_R11.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Planning_R11.xltx", 3, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            
            for (int i = 0; i < dtArtworkType.Rows.Count; i++)  //列印動態欄位的表頭
            {
                objSheets.Cells[3, 12+i] = dtArtworkType.Rows[i]["id"].ToString();
            }

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
