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

namespace Sci.Production.Subcon
{
    public partial class R21 : Sci.Win.Tems.PrintForm
    {
        string category, factory, subcon, mdivision, orderby;
        DateTime? APdate1, APdate2;
        DataTable printData;

        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory", out factory);
            MyUtility.Tool.SetupCombox(cbbFactory, 1, factory);
            cbbFactory.Text = Sci.Env.User.Factory;
            MyUtility.Tool.SetupCombox(cbbOrderBy, 1, 1, "Supplier,Handle");
            cbbOrderBy.SelectedIndex = 0;
            txtMdivision1.Text = Sci.Env.User.Keyword;

            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            int year = DateTime.Today.Year;
            this.dateRange1.Value1 = DateTime.Today.AddMonths(-month+1).AddDays(-day + 1);
            this.dateRange1.Value2 = DateTime.Now;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("AP Date can't empty!!");
                return false;
            }
            APdate1 = dateRange1.Value1;
            APdate2 = dateRange1.Value2;
           
            category = txtartworktype_fty1.Text;
            mdivision = txtMdivision1.Text;
            factory = cbbFactory.Text;
            subcon = txtsubcon1.TextBox1.Text;
            orderby = cbbOrderBy.Text;
            
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- Sql Command --
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"Select 
(select FactoryId from orders where id = b.OrderId) order_factory
,a.MDivisionID
,a.FactoryID
,a.LocalSuppID
,(select abb from LocalSupp where id = a.LocalSuppID) supplier
,a.Id
,a.Status
,a.IssueDate
,dbo.getpass1(a.Handle) handle
,a.CurrencyID
,a.Amount+a.vat apAmount
,a.Category
,b.OrderID
,b.Refno
,b.ThreadColorID
,b.UnitID
,b.Price
,b.Qty
,b.Price*b.Qty ApAmount
,b.LocalPoId
,dbo.getpass1(c.AddName) pohandle
,c.Amount+c.Vat poAmount
,c.IssueDate poDate
,a.InvNo
from localap a inner join LocalAP_Detail b on a.id = b.id 
left join LocalPO c on c.ID = b.LocalPoId
where a.ApvDate is null and a.issuedate between '{0}' and '{1}'
", Convert.ToDateTime(APdate1).ToString("d"), Convert.ToDateTime(APdate2).ToString("d")));
            #endregion

            System.Data.SqlClient.SqlParameter sp_category = new System.Data.SqlClient.SqlParameter();
            sp_category.ParameterName = "@category";

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            System.Data.SqlClient.SqlParameter sp_subcon = new System.Data.SqlClient.SqlParameter();
            sp_subcon.ParameterName = "@subcon";

             IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();

            if (!MyUtility.Check.Empty(category))
            {
                sqlCmd.Append(" and a.category = @category");
                sp_category.Value = category;
                cmds.Add(sp_category);
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

            if (orderby.ToUpper() == "SUPPLIER")
                sqlCmd.Append(" order by a.localsuppid ");
            else
                sqlCmd.Append(" order by a.handle ");

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

            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R21.xltx",2);
            return true;
        }
    }
}
