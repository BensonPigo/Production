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

namespace Sci.Production.Subcon
{
    public partial class R15 : Sci.Win.Tems.PrintForm
    {
        string artworktype,factory,subcon,spno,style,orderby,mdivision;
        DateTime? issuedate1, issuedate2;
        DataTable printData;
        StringBuilder condition = new StringBuilder();
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory", out factory);
            MyUtility.Tool.SetupCombox(cbbFactory, 1, factory);
            cbbFactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(cbbOrderBy, 1, 1, "Issue date,Supplier");
            cbbOrderBy.SelectedIndex = 0;
            txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Issue Date can't empty!!");
                return false;
            }
            issuedate1 = dateRange1.Value1;
            issuedate2 = dateRange1.Value2;

            artworktype = txtartworktype_fty1.Text;
            mdivision = txtMdivision1.Text;
            factory = cbbFactory.Text;
            subcon = txtsubcon1.TextBox1.Text;
            spno = txtSPNO.Text;
            style = txtstyle1.Text;
            orderby = cbbOrderBy.Text;

            condition.Clear();
            condition.Append(string.Format(@"Issue Date : {0} ~ {1};     " 
                , Convert.ToDateTime(issuedate1).ToString("d")
                , Convert.ToDateTime(issuedate2).ToString("d")));
            condition.Append(string.Format(@"Artworktype Type : {0};     " , artworktype));
            condition.Append(string.Format(@"M : {0};    " , mdivision));
            condition.Append(string.Format(@"Factory : {0};   " , factory));
            condition.Append(string.Format(@"Supplier : {0};   ", subcon));
            condition.Append(string.Format(@"SP# : {0};   ", spno));
            condition.Append(string.Format(@"Style : {0};   ", style));
            condition.Append(string.Format(@"Order by : {0};   ", orderby));

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"Select a.mdivisionid
,a.Factoryid
, a.ID
, a.issuedate
, a.localsuppid+'-'+(select abb from localsupp where id = a.localsuppid) supplier
, a.Delivery
, b.ORDERID
, c.styleid
, b.patternDesc
, b.artworkid
, b.CostStitch
, b.Stitch
, b.poqty
, a.Currencyid
, b.UnitPrice
, round(b.UnitPrice*dbo.getRate(s.ExchangeId,a.CurrencyId,'USD',A.ISSUEDATE),4) UnitPriceUSD
, b.Cost 
, b.cost - round(b.UnitPrice*dbo.getRate(s.ExchangeId,a.CurrencyId,'USD',A.ISSUEDATE),4) variance
from dbo.system s,dbo.Artworkpo a
inner join artworkpo_detail b on b.id = a.id
inner join orders c on c.id = b.orderid
where a.issuedate between '{0}' and '{1}'
", Convert.ToDateTime(issuedate1).ToString("d"), Convert.ToDateTime(issuedate2).ToString("d")));

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

            System.Data.SqlClient.SqlParameter sp_spno = new System.Data.SqlClient.SqlParameter();
            sp_spno.ParameterName = "@spno";

            System.Data.SqlClient.SqlParameter sp_style = new System.Data.SqlClient.SqlParameter();
            sp_style.ParameterName = "@style";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();


            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlCmd.Append(" and a.artworktypeid = @artworktype");
                sp_artworktype.Value = artworktype;
                cmds.Add(sp_artworktype);
            }
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and a.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
            }
            if (!MyUtility.Check.Empty(subcon))
            {
                sqlCmd.Append(" and a.localsuppid = @subcon");
                sp_subcon.Value = subcon;
                cmds.Add(sp_subcon);
            }
            if (!MyUtility.Check.Empty(spno))
            {
                sqlCmd.Append(" and c.id = @spno ");
                sp_spno.Value = spno;
                cmds.Add(sp_spno);
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(" and c.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
            }
            sqlCmd.Append(" and a.Status!='Closed'");

            if (orderby.ToUpper() == "ISSUE DATE")
                sqlCmd.Append(" order by a.issuedate ");
            else
                sqlCmd.Append(" order by a.localsuppid ");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(),cmds, out printData);
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R15.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R15.xltx", 3, true, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
