using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// R15
    /// </summary>
    public partial class R15 : Win.Tems.PrintForm
    {
        private string artworktype;
        private string factory;
        private string subcon;
        private string spno;
        private string style;
        private string orderby;
        private string mdivision;
        private DateTime? issuedate1;
        private DateTime? issuedate2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();

        /// <summary>
        /// R15
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Issue date,Supplier");
            this.comboOrderBy.SelectedIndex = 0;
            this.txtMdivisionM.Text = Env.User.Keyword;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2))
            {
                MyUtility.Msg.WarningBox("Issue Date can't empty!!");
                return false;
            }

            this.issuedate1 = this.dateIssueDate.Value1;
            this.issuedate2 = this.dateIssueDate.Value2;

            this.artworktype = this.txtartworktype_ftyArtworkType.Text;
            this.mdivision = this.txtMdivisionM.Text;
            this.factory = this.comboFactory.Text;
            this.subcon = this.txtsubconSupplier.TextBox1.Text;
            this.spno = this.txtSPNO.Text;
            this.style = this.txtstyle.Text;
            this.orderby = this.comboOrderBy.Text;

            this.condition.Clear();

            this.condition.Append(string.Format(
                @"Issue Date : {0} ~ {1};     ",
                Convert.ToDateTime(this.issuedate1).ToString("d"),
                Convert.ToDateTime(this.issuedate2).ToString("d")));
            return base.ValidateInput();
        }

        // 非同步取資料

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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

            if (!MyUtility.Check.Empty(this.issuedate1))
            {
                sqlCmd.Append(" and a.issuedate >= @issuedate1");
                sp_issuedate1.Value = this.issuedate1;
                cmds.Add(sp_issuedate1);
            }

            if (!MyUtility.Check.Empty(this.issuedate2))
            {
                sqlCmd.Append(" and a.issuedate <= @issuedate2");
                sp_issuedate2.Value = this.issuedate2;
                cmds.Add(sp_issuedate2);
            }

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(" and a.artworktypeid = @artworktype");
                sp_artworktype.Value = this.artworktype;
                cmds.Add(sp_artworktype);
                this.condition.Append(string.Format(@"Artworktype Type : {0};     ", this.artworktype));
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
                this.condition.Append(string.Format(@"M : {0};    ", this.mdivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and a.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
                this.condition.Append(string.Format(@"Factory : {0};   ", this.factory));
            }

            if (!MyUtility.Check.Empty(this.subcon))
            {
                sqlCmd.Append(" and a.localsuppid = @subcon");
                sp_subcon.Value = this.subcon;
                cmds.Add(sp_subcon);
                this.condition.Append(string.Format(@"Supplier : {0};   ", this.subcon));
            }

            if (!MyUtility.Check.Empty(this.spno))
            {
                sqlCmd.Append(" and c.id = @spno ");
                sp_spno.Value = this.spno;
                cmds.Add(sp_spno);
                this.condition.Append(string.Format(@"SP# : {0};   ", this.spno));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(" and c.styleid = @style");
                sp_style.Value = this.style;
                cmds.Add(sp_style);
                this.condition.Append(string.Format(@"Style : {0};   ", this.style));
            }

            if (!MyUtility.Check.Empty(this.orderby))
            {
                this.condition.Append(string.Format(@"Order by : {0};   ", this.orderby));
            }

            if (this.orderby.ToUpper() == "ISSUE DATE")
            {
                sqlCmd.Append(" order by a.issuedate ");
            }
            else
            {
                sqlCmd.Append(" order by a.localsuppid ");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R15.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[2, 1] = this.condition.ToString();   // 條件字串寫入excel
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R15.xltx", 3, true, null, objApp);      // 將datatable copy to excel
            Marshal.ReleaseComObject(objSheets);
            return true;
        }
    }
}
