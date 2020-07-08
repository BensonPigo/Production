using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class R12 : Win.Tems.PrintForm
    {
        string artworktype;
        string factory;
        string subcon;
        string mdivision;
        string orderby;
        DateTime? APdate1;
        DateTime? APdate2;
        DataTable printData;

        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 1, 1, "Supplier,Handle");
            this.comboOrderBy.SelectedIndex = 0;
            this.txtMdivisionM.Text = Sci.Env.User.Keyword;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            this.dateAPDate.Value1 = DateTime.Today.AddMonths(-month + 1).AddDays(-day + 1);
            this.dateAPDate.Value2 = DateTime.Now;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateAPDate.Value1) && MyUtility.Check.Empty(this.dateAPDate.Value2))
            {
                MyUtility.Msg.WarningBox("AP Date can't empty!!");
                return false;
            }

            this.APdate1 = this.dateAPDate.Value1;
            this.APdate2 = this.dateAPDate.Value2;

            this.artworktype = this.txtartworktype_ftyArtworkType.Text;
            this.mdivision = this.txtMdivisionM.Text;
            this.factory = this.comboFactory.Text;
            this.subcon = this.txtsubconSupplier.TextBox1.Text;
            this.orderby = this.comboOrderBy.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
Select  a.MDivisionID
        ,a.FactoryID
        ,a.LocalSuppID
        ,supplier = (select abb from LocalSupp WITH (NOLOCK) where id = a.LocalSuppID) 
        ,a.Id
        ,a.IssueDate
        ,handle = (select concat(Pass1.name, ' Ext.', Pass1.ExtNo) from Pass1 where Pass1.id = a.Handle)
        ,a.CurrencyID
        ,apAmount = CONVERT(VARCHAR(20),CAST(a.Amount+a.vat AS Money),1) 
        ,a.ArtworkTypeID
        ,b.ArtworkPoID
        ,b.OrderID
        ,style = (select orders.StyleID from orders WITH (NOLOCK) where id = b.OrderID) 
        ,pohandle = (select concat(Pass1.name, ' Ext.', Pass1.ExtNo) from Pass1 where Pass1.id = c.Handle)
        --,CONVERT(VARCHAR(20),CAST(c.Amount+c.Vat AS Money),1) as poAmount
        ,poAmount = CONVERT(VARCHAR(20),CAST(b.Amount AS Money),1) 
        ,poDate = c.IssueDate 
        ,a.InvNo
from artworkap a WITH (NOLOCK) 
inner join artworkap_detail b WITH (NOLOCK) on a.id = b.id 
left join ArtworkPO c WITH (NOLOCK) on c.ID = b.ArtworkPoID
where  a.ApvDate is null and 1=1"));
            #endregion

            System.Data.SqlClient.SqlParameter sp_APdate1 = new System.Data.SqlClient.SqlParameter();
            sp_APdate1.ParameterName = "@APdate1";

            System.Data.SqlClient.SqlParameter sp_APdate2 = new System.Data.SqlClient.SqlParameter();
            sp_APdate2.ParameterName = "@APdate2";

            System.Data.SqlClient.SqlParameter sp_artworktype = new System.Data.SqlClient.SqlParameter();
            sp_artworktype.ParameterName = "@artworktype";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            if (!MyUtility.Check.Empty(this.APdate1))
             {
                 sqlCmd.Append(" and a.issuedate >= @APdate1");
                 sp_APdate1.Value = this.APdate1;
                 cmds.Add(sp_APdate1);
             }

            if (!MyUtility.Check.Empty(this.APdate2))
             {
                 sqlCmd.Append(" and a.issuedate <= @APdate2");
                 sp_APdate2.Value = this.APdate2;
                 cmds.Add(sp_APdate2);
             }

            if (!MyUtility.Check.Empty(this.artworktype))
            {
                sqlCmd.Append(" and a.artworktypeid = @artworktype");
                sp_artworktype.Value = this.artworktype;
                cmds.Add(sp_artworktype);
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and a.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.subcon))
            {
                sqlCmd.Append(" and a.localsuppid = @subcon");
                sp_subcon.Value = this.subcon;
                cmds.Add(sp_subcon);
            }

            if (this.orderby.ToUpper() == "SUPPLIER")
            {
                sqlCmd.Append(" order by a.localsuppid ");
            }
            else
            {
                sqlCmd.Append(" order by a.handle ");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
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
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R12.xltx", 2);
            return true;
        }
    }
}
