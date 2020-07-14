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
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboFactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(comboOrderBy, 1, 1, "Issue date,Supplier");
            comboOrderBy.SelectedIndex = 0;
            txtMdivisionM.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateIssueDate.Value1) && MyUtility.Check.Empty(dateIssueDate.Value2))
            {
                MyUtility.Msg.WarningBox("Issue Date can't empty!!");
                return false;
            }
            issuedate1 = dateIssueDate.Value1;
            issuedate2 = dateIssueDate.Value2;

            artworktype = txtartworktype_ftyArtworkType.Text;
            mdivision = txtMdivisionM.Text;
            factory = comboFactory.Text;
            subcon = txtsubconSupplier.TextBox1.Text;
            spno = txtSPNO.Text;
            style = txtstyle.Text;
            orderby = comboOrderBy.Text;

            condition.Clear();
            
            condition.Append(string.Format(@"Issue Date : {0} ~ {1};     " 
                , Convert.ToDateTime(issuedate1).ToString("d")
                , Convert.ToDateTime(issuedate2).ToString("d")));
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(string.Format(@"Select a.mdivisionid
,a.Factoryid
, a.ID
,convert(varchar(10),a.issuedate,111) issuedate
,a.localsuppid+'-'+(select abb from localsupp WITH (NOLOCK) where id = a.localsuppid) supplier
,convert(varchar(10),a.Delivery,111) Delivery
, b.ORDERID
, c.styleid
, b.Article
, b.patternDesc
, b.SizeCode
, a.ArtworkTypeID
, b.artworkid
, b.CostStitch
, b.Stitch
, b.poqty
, a.Currencyid
, b.UnitPrice
, b.UnitPrice*dbo.getRate(s.ExchangeId,a.CurrencyId,'USD',A.ISSUEDATE) UnitPriceUSD
, b.Cost
, b.cost - b.UnitPrice*dbo.getRate(s.ExchangeId,a.CurrencyId,'USD',A.ISSUEDATE) AS variance
from dbo.system s WITH (NOLOCK) ,dbo.Artworkpo a WITH (NOLOCK) 
inner join artworkpo_detail b WITH (NOLOCK) on b.id = a.id
inner join orders c WITH (NOLOCK) on c.id = b.orderid
where 1=1"));

            System.Data.SqlClient.SqlParameter sp_issuedate1 = new System.Data.SqlClient.SqlParameter();
            sp_issuedate1.ParameterName = "@issuedate1";

            System.Data.SqlClient.SqlParameter sp_issuedate2 = new System.Data.SqlClient.SqlParameter();
            sp_issuedate2.ParameterName = "@issuedate2";

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

            if (!MyUtility.Check.Empty(issuedate1))
            {
                sqlCmd.Append(" and a.issuedate >= @issuedate1");
                sp_issuedate1.Value = issuedate1;
                cmds.Add(sp_issuedate1);
            }

            if (!MyUtility.Check.Empty(issuedate2))
            {
                sqlCmd.Append(" and a.issuedate <= @issuedate2");
                sp_issuedate2.Value = issuedate2;
                cmds.Add(sp_issuedate2);
            }
            
            if (!MyUtility.Check.Empty(artworktype))
            {
                sqlCmd.Append(" and a.artworktypeid = @artworktype");
                sp_artworktype.Value = artworktype;
                cmds.Add(sp_artworktype);
                condition.Append(string.Format(@"Artworktype Type : {0};     ", artworktype));
            }
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
                condition.Append(string.Format(@"M : {0};    ", mdivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and a.factoryid = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
                condition.Append(string.Format(@"Factory : {0};   ", factory));
            }
            if (!MyUtility.Check.Empty(subcon))
            {
                sqlCmd.Append(" and a.localsuppid = @subcon");
                sp_subcon.Value = subcon;
                cmds.Add(sp_subcon);
                condition.Append(string.Format(@"Supplier : {0};   ", subcon));
            }
            if (!MyUtility.Check.Empty(spno))
            {
                sqlCmd.Append(" and c.id = @spno ");
                sp_spno.Value = spno;
                cmds.Add(sp_spno);
                condition.Append(string.Format(@"SP# : {0};   ", spno));
            }
            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(" and c.styleid = @style");
                sp_style.Value = style;
                cmds.Add(sp_style);
                condition.Append(string.Format(@"Style : {0};   ", style));
            }

            if (!MyUtility.Check.Empty(orderby))
            {
                condition.Append(string.Format(@"Order by : {0};   ", orderby));
            }

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
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = condition.ToString();   // 條件字串寫入excel
            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R15.xltx", 3, true, null, objApp);      // 將datatable copy to excel
            Marshal.ReleaseComObject(objSheets);
            return true;
        }
    }
}
